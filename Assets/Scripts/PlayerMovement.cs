using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

namespace Profielwerkstuk
{
    public class PlayerMovement : MonoBehaviour
    {

        public NavMeshAgent agent;
        public GameObject ground;
        public int numTasks;
        private TaskManager taskManager;
        public NavMeshObstacle obstacle;
        public CapsuleCollider triggerCollider;
        private Vector3 target;
        public string status;
        public List<GameObject> waitingFor;
        public GameObject coughCloudPrefab;
        public Material infectedMaterial;
        public GameObject coughCloudParent;
        public bool infected = false;

        private float timeSinceLastCough = 0.0f;
        private float timeUntilCough = 0.0f;

        void Start()
        {
            Time.timeScale = 10.0f;
            if(Random.Range(0, 100) >= 50)
            {
                Debug.Log(name + " is infected");
                infected = true;
                gameObject.GetComponent<MeshRenderer>().material = infectedMaterial;
            }

            timeUntilCough = Random.Range(10.0f, 30.0f);

            obstacle.enabled = false;
            agent.autoBraking = false;
            taskManager = new TaskManager(ground, numTasks, agent);
            target = taskManager.getTask(agent);
            agent.SetDestination(target);
            // targetIndicator.transform.position = target;
            // Debug.Log(name + " is now going to: " + target.x + ", " + target.y + ", " + target.z);
            status = "ACTIVE";  
            waitingFor = new List<GameObject>();
        }

        // Update is called once per frame
        void Update()
        {
            transform.GetComponent<Rigidbody>().velocity.Set(0, 0, 0);
            transform.GetComponent<Rigidbody>().rotation.Set(0, 0, 0, 0);

            

            if (infected)
            {
                // print(timeSinceLastCough);
                timeSinceLastCough += Time.deltaTime;
                if(timeUntilCough < timeSinceLastCough)
                {
                    timeSinceLastCough = 0.0f;
                    timeUntilCough = Random.Range(10.0f, 30.0f);
                    GameObject p = Instantiate(coughCloudPrefab, transform.position, transform.rotation);
                    p.transform.GetComponent<Rigidbody>().velocity = transform.forward * 0.7f;
                    p.transform.SetParent(coughCloudParent.transform);
                }
            }
            // check if agent has reached goal
            if (agent.enabled && !agent.isStopped)
            {
                var pos = transform.position;
                if (Vector3.Distance(new Vector3(pos.x, target.y, pos.z), target) <= agent.stoppingDistance + gameObject.GetComponent<CapsuleCollider>().radius)
                {
                    if (taskManager.Tasks.Count == 1)
                    {
                        taskManager.removeTask(target);
                        Debug.Log(name + " reached final destination");
                        StartCoroutine(activateObstacle());
                        status = "DONE";
                    }
                    else
                    {
                        // Debug.Log(name + " reached target: " + (int)target.x + ", " + (int)target.z + ". Postition was: " + (int)pos.x + ", " + (int)pos.z);
                        StartCoroutine(waitForNextTask(Random.Range(500, 2000)));
                    }
                }
            }
            if (status != "DONE" && taskManager.Tasks.Count == 0)
            {
                Debug.Log(name + " reached final destination");
                StartCoroutine(activateObstacle());
                status = "DONE";
            }
            for (int i = waitingFor.Count - 1; i >= 0; i--)
            {
                GameObject player = waitingFor[i];
                PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
                if (playerMovement.status == "DONE")
                {
                    waitingFor.Remove(player);
                    if (waitingFor.Count == 0)
                    {
                        // Debug.Log(name + " is no longer waiting.");
                        // Debug.Log(name + "'s state is now: ACTIVE");
                        StartCoroutine(deactivateObstacle());
                    }
                }
                else if (playerMovement.status == "SHOPPING")
                {
                    waitingFor.Remove(player);
                    playerMovement.waitingFor.Add(gameObject);
                    if (waitingFor.Count == 0)
                    {
                        // Debug.Log(name + " is no longer waiting.");
                        // Debug.Log(name + "'s state is now: ACTIVE");
                        StartCoroutine(deactivateObstacle());
                    }
                }
            }

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.parent.name != "Players") return;
            // Debug.Log("Player " + other.gameObject.name + " just entered the collider");
            if (other.GetComponent<PlayerMovement>().status == "DONE") return;
            if (other.GetComponent<PlayerMovement>().status == "SHOPPING") return;
            if (transform.GetSiblingIndex() < other.transform.GetSiblingIndex())
            {
                if (waitingFor.Contains(other.gameObject)) return;
                Debug.Log(name + " is now waiting for " + other.name);
                waitingFor.Add(other.gameObject);
                // Debug.Log(name + "'s state is now: INACTIVE");
                StartCoroutine(activateObstacle());
                status = "INACTIVE";
            }
            else
            {
                if(status == "ACTIVE") target = taskManager.getTask(agent);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.parent.name != "Players") return;
            if (status == "ACTIVE") return;
            // Debug.Log(name + ": " + other.gameObject.name + " just left the collider");
            waitingFor.Remove(other.gameObject);
            if (waitingFor.Count == 0)
            {
                Debug.Log(name + " is no longer waiting.");
                // Debug.Log(name + "'s state is now: ACTIVE");
                StartCoroutine(deactivateObstacle());
            }
        }

        IEnumerator waitForNextTask(float ms)
        {
            Debug.Log(name + " is waiting for " + ms + "ms");
            StartCoroutine(activateObstacle());
            status = "SHOPPING";
            taskManager.removeTask(target);
            yield return new WaitForSeconds(ms / 1000);
            Debug.Log(name + " is done waiting.");
            if (waitingFor.Count == 0) { StartCoroutine(deactivateObstacle()); }
            else status = "INACTIVE";
        }
        IEnumerator activateObstacle()
        {
            agent.enabled = false;
            yield return null;
            obstacle.enabled = true;
        }

        IEnumerator deactivateObstacle()
        {
            obstacle.enabled = false;
            yield return null;
            agent.enabled = true;
            yield return new WaitForEndOfFrame();
            target = taskManager.getTask(agent);
            agent.SetDestination(target);
            yield return null;
            status = "ACTIVE";
        }

        public void infect()
        {
            Debug.Log(name + " is infected");
            infected = true;
            gameObject.GetComponent<MeshRenderer>().material = infectedMaterial;
        }
    }
}
