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
        public int numTasks;
        public TaskManager taskManager;
        public NavMeshObstacle obstacle;
        public CapsuleCollider triggerCollider;
        public Vector3 target;
        public string status;
        public List<GameObject> waitingFor;
        public GameObject coughCloudPrefab;
        public Material infectedMaterial;
        public Transform coughCloudParent;
        public bool infected = false;

        private float timeSinceLastCough = 0.0f;
        private float timeUntilCough = 0.0f;

        void Start()
        {
            taskManager = new TaskManager(agent);
            timeUntilCough = Random.Range(10.0f, 30.0f);
            obstacle.enabled = false;
            agent.autoBraking = false;
            status = "ASSIGNING";  
            waitingFor = new List<GameObject>();
        }

        // Update is called once per frame
        void Update()
        {
            if (status == "ASSIGNING") return;
            if (status == "DONE")
            {
                Destroy(gameObject);
                return;
            }

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
                    GameObject p = Instantiate(coughCloudPrefab, transform.position, transform.rotation, coughCloudParent);
                    p.transform.GetComponent<Rigidbody>().velocity = transform.forward * 0.7f;
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
                        Debug.Log(name + " reached final task");
                        status = "CHECKING OUT";
                        target = taskManager.registerPos;
                        agent.SetDestination(target);
                    }
                    else if(taskManager.Tasks.Count == 0)
                    {
                        if (status == "CHECKING OUT")
                        {
                            print(name + " is at the register");
                            status = "AT REGISTER";
                            StartCoroutine(waitForNextTask(Random.Range(20000, 40000)));
                        }
                        if(status == "LEAVING")
                        {
                            print(name + " has left");
                            status = "DONE";
                        }
                    }
                    else
                    {
                        // Debug.Log(name + " reached target: " + (int)target.x + ", " + (int)target.z + ". Postition was: " + (int)pos.x + ", " + (int)pos.z);
                        status = "SHOPPING";
                        taskManager.removeTask(target);
                        StartCoroutine(waitForNextTask(Random.Range(500, 2000)));
                    }
                }
            }
            if (agent.enabled)
            {
                for (int i = waitingFor.Count - 1; i >= 0; i--)
                {
                    GameObject player = waitingFor[i];
                    if (player == null)
                    {
                        waitingFor.Remove(player);
                        continue;
                    }
                    PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
                    /* if (playerMovement.status == "DONE")
                    {
                        waitingFor.Remove(player);
                        if (waitingFor.Count == 0)
                        {
                            // Debug.Log(name + " is no longer waiting.");
                            // Debug.Log(name + "'s state is now: ACTIVE");
                            StartCoroutine(deactivateObstacle());
                        }
                    }
                    else*/
                    if (playerMovement.status == "SHOPPING" || playerMovement.status == "AT REGISTER")
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
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.parent.name != "Players") return;
            // Debug.Log("Player " + other.gameObject.name + " just entered the collider");
            // if (other.GetComponent<PlayerMovement>().status == "DONE") return;
            if (other.GetComponent<PlayerMovement>().status == "SHOPPING") return;
            if (other.GetComponent<PlayerMovement>().status == "AT REGISTER") return;
            if (transform.GetSiblingIndex() < other.transform.GetSiblingIndex())
            {
                if (waitingFor.Contains(other.gameObject)) return;
                // Debug.Log(name + " is now waiting for " + other.name);
                waitingFor.Add(other.gameObject);
                // Debug.Log(name + "'s state is now: INACTIVE");
                StartCoroutine(activateObstacle());
                // status = "INACTIVE";
            }
            else
            {
                if (agent.enabled)
                {
                    if (status == "ACTIVE")
                    {
                        target = taskManager.getTask();
                        agent.SetDestination(target);
                    }
                }                    
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.parent.name != "Players") return;
            // Debug.Log(name + ": " + other.gameObject.name + " just left the collider");
            waitingFor.Remove(other.gameObject);
            if (waitingFor.Count == 0)
            {
                // Debug.Log(name + " is no longer waiting.");
                // Debug.Log(name + "'s state is now: ACTIVE");
                StartCoroutine(deactivateObstacle());
            }
        }

        IEnumerator waitForNextTask(float ms)
        {
            // Debug.Log(name + " is waiting for " + ms + "ms");
            StartCoroutine(activateObstacle());
            yield return new WaitForSeconds(ms / 1000);
            // print(name + " is done waiting.");
            if (waitingFor.Count == 0) { StartCoroutine(deactivateObstacle()); }
        }
        IEnumerator activateObstacle()
        {
            agent.enabled = false;
            yield return null;  
            obstacle.enabled = true;
            yield return null;
        }

        IEnumerator deactivateObstacle()
        {
            obstacle.enabled = false;
            yield return null;
            agent.enabled = true;
            yield return new WaitForEndOfFrame();
            if (taskManager.Tasks.Count == 0)
            {
                if (status == "CHECKING OUT")
                {
                    target = taskManager.registerPos;
                }
                if (status == "AT REGISTER" || status == "LEAVING")
                {
                    target = taskManager.leavingPos;
                    status = "LEAVING";
                }
            }
            else target = taskManager.getTask();
            agent.SetDestination(target);
        }

        public void infect()
        {
            Debug.Log(name + " is infected");
            infected = true;
            gameObject.GetComponent<MeshRenderer>().material = infectedMaterial;
        }
    }
}

