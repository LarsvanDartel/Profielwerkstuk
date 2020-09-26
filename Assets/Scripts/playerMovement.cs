using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Profielwerkstuk
{
    public class PlayerMovement : MonoBehaviour
    {

        public Camera cam;
        public NavMeshAgent agent;
        public GameObject walls;
        public GameObject ground;
        public int numTasks;
        private TaskManager taskManager;
        public GameObject targetIndicator;
        public NavMeshObstacle obstacle;
        public CapsuleCollider triggerCollider;

        void Start()
        {
            obstacle.enabled = false;
            agent.autoBraking = false;
            taskManager = new TaskManager(walls, ground, numTasks);
            var target = taskManager.getTask(agent);
            agent.SetDestination(target);
            // targetIndicator.transform.position = target;
            Debug.Log(name + " is now going to: " + target.x + ", " + target.y + ", " + target.z);
        }

        // Update is called once per frame
        void Update()
        {
            // if (Input.GetMouseButtonDown(0))
            // {
            // cast a ray from camera to mouse's position
            // Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            // RaycastHit hit;
            // make the agent move to where the ray hit
            // if (Physics.Raycast(ray, out hit))
            // {
            // agent.SetDestination(hit.point);
            // }
            // }

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
                            Debug.Log(name + " reached destination.");
                            var target = taskManager.getTask(agent);
                            Debug.Log(name + " is now going to: " + target.x + ", " + target.y + ", " + target.z);
                            // targetIndicator.transform.position = target;
                            agent.SetDestination(target);
                            // If agent has done all tasks:
                            if (agent.destination.x == 0f && agent.destination.z == 0f)
                            {
                                agent.isStopped = true;
                                triggerCollider.enabled = false;
                                StartCoroutine(activateObstacle());
                            }
                        }
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.parent.name != "Players") return;
            Debug.Log("Player " + other.gameObject.name + " just entered the collider");
            if (!other.transform.GetComponent<NavMeshObstacle>().isActiveAndEnabled && !obstacle.isActiveAndEnabled)
            {
                if (transform.parent.Find(transform.name).GetSiblingIndex() < transform.parent.Find(other.transform.name).GetSiblingIndex())
                {
                    Debug.Log(name + "'s state is now: INACTIVE");
                    StartCoroutine(activateObstacle());
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.transform.parent.name != "Players") return;
            if (!obstacle.isActiveAndEnabled) return;
            Debug.Log("Player " + other.gameObject.name + " just Left the collider");
            Debug.Log(name + "'s state is now: ACTIVE");

            StartCoroutine(deactivateObstacle());
        }

        IEnumerator activateObstacle()
        {
            agent.isStopped = true;
            agent.enabled = false;
            yield return null;
            obstacle.enabled = true;
        }

        IEnumerator deactivateObstacle()
        {
            obstacle.enabled = false;
            yield return null;
            agent.enabled = true;
            agent.isStopped = false;
        }

    }
}
