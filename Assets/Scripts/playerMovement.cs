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

        void Start()
        {
            taskManager = new TaskManager(walls, ground, numTasks);
            var target = taskManager.getTask(agent);
            agent.SetDestination(target);
            targetIndicator.transform.position = target;
            Debug.Log("Agent is now going to: " + target.x + ", " + target.y + ", " + target.z);
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
            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= agent.stoppingDistance) 
                {
                    if(!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        // if reached goal
                        Debug.Log("Agent reached destination.");
                        var target = taskManager.getTask(agent);
                        Debug.Log("Agent is now going to: " + target.x + ", " + target.y + ", " + target.z);
                        targetIndicator.transform.position = target;
                        agent.SetDestination(target);
                        // If agent has done all tasks:
                        if(agent.destination.x == 0f && agent.destination.z == 0f)
                        { 
                            agent.isStopped = true;
                        }
                    }
                }
            }
        }
    }
}
