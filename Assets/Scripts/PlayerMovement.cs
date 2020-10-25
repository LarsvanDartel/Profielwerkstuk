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
        void Start()
        {
            agent.enabled = false;
            agent.autoBraking = false;
            StartCoroutine(deactivateObstacle(false));   
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
            if (agent.enabled && !agent.isStopped && status != "SHOPPING")
            {
                var pos = transform.position;
                if (Vector3.Distance(new Vector3(pos.x, target.y, pos.z), target) <= agent.stoppingDistance + gameObject.GetComponent<CapsuleCollider>().radius)
                {
                    if (taskManager.Tasks.Count == 1)
                    {
                        taskManager.removeTask(target);
                        Debug.Log(name + " reached final destination");
                        StartCoroutine(activateObstacle());
                    } else {
                        Debug.Log(name + " reached target: " + (int)target.x + ", " + (int)target.z + ". Postition was: " + (int)pos.x + ", " + (int)pos.z);
                        StartCoroutine(waitForNextTask(Random.Range(500, 2000)));
                    }
                }
            }
            if (status != "DONE" && taskManager.Tasks.Count == 0) 
            { 
                status = "DONE";
                StartCoroutine(activateObstacle());
            }
            for(int i = waitingFor.Count-1; i >= 0; i--)
            {
                GameObject player = waitingFor[i];
                PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
                if (playerMovement.status == "DONE")
                {
                    waitingFor.Remove(player);
                    if (waitingFor.Count == 0)
                    {
                        Debug.Log(name + " is no longer waiting.");
                        // Debug.Log(name + "'s state is now: ACTIVE");
                        StartCoroutine(deactivateObstacle(false));
                    }
                }
                else if(playerMovement.status == "SHOPPING")
                {
                    waitingFor.Remove(player);
                    playerMovement.waitingFor.Add(gameObject);
                    if (waitingFor.Count == 0)
                    {
                        Debug.Log(name + " is no longer waiting.");
                        // Debug.Log(name + "'s state is now: ACTIVE");
                        StartCoroutine(deactivateObstacle(false));
                    }
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
            if (other.transform.parent.name != "Players") return;
            if (status == "ACTIVE") return;
            Debug.Log(name + ": " + other.gameObject.name + " just left the collider");
            waitingFor.Remove(other.gameObject);
            if (waitingFor.Count == 0)
            {
                Debug.Log(name + " is no longer waiting.");
                // Debug.Log(name + "'s state is now: ACTIVE");
                StartCoroutine(deactivateObstacle(false));
            }
        }

        IEnumerator waitForNextTask(float ms)
        {
            Debug.Log(name + " is waiting for " + ms + "ms");
            status = "SHOPPING";
            StartCoroutine(activateObstacle());
            yield return new WaitForSeconds(ms / 1000);
            Debug.Log(name + " is done waiting.");
            if (waitingFor.Count == 0)
            {
                StartCoroutine(deactivateObstacle(true));
            }
            else status = "INACTIVE";
        }
        IEnumerator activateObstacle()
        {
            agent.enabled = false;
            yield return null;
            obstacle.enabled = true;
            status = "INACTIVE";
        }

        IEnumerator deactivateObstacle(bool newTarget)
        {
            obstacle.enabled = false;
            yield return null;
            agent.enabled = true;
            yield return null;
            if (newTarget)
            {
                taskManager.removeTask(target);
                target = taskManager.getTask(agent);
            }
            else agent.SetDestination(target);
            yield return null;
            status = "ACTIVE";
        }

    }
}
