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
        public bool waiting;
        private float timeSinceLastCough = 0.0f;
        private float timeUntilCough = 0.0f;

        void Start()
        {
            taskManager = new TaskManager(agent);
            timeUntilCough = Random.Range(10.0f, 30.0f);
            obstacle.enabled = false;
            agent.autoBraking = false;
            status = "ASSIGNING";
            waiting = false;

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
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (status == "ACTIVE") taskManager.removeTask(target);
                    if (taskManager.Tasks.Count == 0)
                    {
                        if (status == "ACTIVE")
                        {
                            target = taskManager.registerPos;
                            status = "CHECKING OUT";
                            StartCoroutine(waitForNextTask(Random.Range(5, 15)));
                        }
                        else if (status == "CHECKING OUT")
                        {
                            target = taskManager.leavingPos;
                            status = "LEAVING";
                            StartCoroutine(waitForNextTask(Random.Range(20, 40)));
                        }
                        else if (status == "LEAVING")
                        {
                            Destroy(gameObject);
                        }
                    } else
                    {
                        target = taskManager.getTask();
                        StartCoroutine(waitForNextTask(Random.Range(5, 15)));
                    }
                }
            }
            if (!waiting)
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
                    if (playerMovement.waiting)
                    {
                        waitingFor.Remove(player);
                        playerMovement.waitingFor.Add(gameObject);
                    }
                }
            }
            if (!waiting && waitingFor.Count == 0 && !agent.enabled)
            {
                StartCoroutine(deactivateObstacle());
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.parent.name != "Players") return;
            if (other.GetComponent<NavMeshObstacle>().enabled) return;
            if (transform.GetSiblingIndex() < other.transform.GetSiblingIndex())
            {
                if (waitingFor.Contains(other.gameObject)) return;
                waitingFor.Add(other.gameObject);
                StartCoroutine(activateObstacle());
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
            waitingFor.Remove(other.gameObject);
        }

        IEnumerator waitForNextTask(int s)
        {
            waiting = true;
            StartCoroutine(activateObstacle());
            yield return new WaitForSeconds(s);
            waiting = false;
        }
        IEnumerator activateObstacle()
        {
            agent.enabled = false;
            yield return new WaitForEndOfFrame();
            while (agent.enabled)
            {
                agent.enabled = false;
                yield return null;
            }
            obstacle.enabled = true;
            yield return null;
        }

        IEnumerator deactivateObstacle()
        {
            obstacle.enabled = false;
            yield return null;
            if(!obstacle.enabled) agent.enabled = true;
            yield return new WaitForEndOfFrame();
            if (agent.enabled) agent.SetDestination(target);
        }

        public void infect()
        {
            Debug.Log(name + " is infected");
            infected = true;
            gameObject.GetComponent<MeshRenderer>().material = infectedMaterial;
        }
    }
}

