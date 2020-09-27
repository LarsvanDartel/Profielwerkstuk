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
        private List<GameObject> waitingFor;
        void Start()
        {
            agent.autoBraking = false;
            StartCoroutine(deactivateObstacle());   
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
            // check if agent has reached goal
            if (agent.enabled)
            {
                if (!agent.pathPending)
                {
                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {
                        if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                        {
                            // if reached goal
                            taskManager.removeTask(target);
                            // If agent has done all tasks:
                            if (taskManager.Tasks.Count == 0)
                            {
                                Debug.Log(name + " reached final destination.");
                                agent.isStopped = true;
                                StartCoroutine(activateObstacle());
                            }
                            else
                            {
                                Debug.Log(name + " reached destination.");
                                target = taskManager.getTask(agent);
                                Debug.Log(name + " is now going to: " + target.x + ", " + target.y + ", " + target.z);
                                // targetIndicator.transform.position = target;
                                agent.SetDestination(target);
                            }
                        }
                    }
                }
            }
            if(status != "DONE" && taskManager.Tasks.Count == 0) status = "DONE";
            for(int i = waitingFor.Count-1; i >= 0; i--)
            {
                GameObject player = waitingFor[i];
                if(player.GetComponent<PlayerMovement>().status == "DONE")
                {
                    waitingFor.Remove(player);
                    if (waitingFor.Count == 0)
                    {
                        Debug.Log(name + " is no longer waiting.");
                        // Debug.Log(name + "'s state is now: ACTIVE");
                        StartCoroutine(deactivateObstacle());
                    }
                    continue;
                }
            }

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.parent.name != "Players") return;
            // Debug.Log("Player " + other.gameObject.name + " just entered the collider");
            if (other.GetComponent<PlayerMovement>().status == "DONE") return;
            if (transform.GetSiblingIndex() < other.transform.GetSiblingIndex())
            {
                Debug.Log(name + " is now waiting for " + other.name);
                waitingFor.Add(other.gameObject);
                // Debug.Log(name + "'s state is now: INACTIVE");
                StartCoroutine(activateObstacle());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.transform.parent.name != "Players") return;
            if (status == "ACTIVE") return;
            Debug.Log(name + ": " + other.gameObject.name + " just left the collider");
            waitingFor.Remove(other.gameObject);
            if (waitingFor.Count == 0)
            {
                Debug.Log(name + " is no longer waiting.");
                // Debug.Log(name + "'s state is now: ACTIVE");
                StartCoroutine(deactivateObstacle());
            }
        }

        IEnumerator activateObstacle()
        {
            agent.enabled = false;
            yield return null;
            obstacle.enabled = true;
            status = "INACTIVE";
        }

        IEnumerator deactivateObstacle()
        {
            yield return new WaitForSeconds(0.1f);
            obstacle.enabled = false;
            yield return null;
            agent.enabled = true;
            target = taskManager.getTask(agent);
            agent.SetDestination(target);   
            yield return null;
            status = "ACTIVE";
        }

    }
}
