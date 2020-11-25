using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Profielwerkstuk
{
    public class TaskManager
    {
        public List<Vector3> Tasks;
        public List<Vector3> registerPositions;
        public Vector3 waitingForRegisterPos;
        public Vector3 leavingPos;  

        private NavMeshAgent agent;

        public TaskManager(NavMeshAgent _agent)
        {
            agent = _agent;
            Tasks = new List<Vector3>();
        }

        private bool canReach(Vector3 point)
        {
            NavMeshPath path = new NavMeshPath();
            return agent.CalculatePath(point, path) && path.status == NavMeshPathStatus.PathComplete;
        }

        private float getPathDistance(NavMeshPath path)
        {
            float lng = 0.0f;
            // Debug.Log(path.status);
            //Debug.Log(path.corners.Length);
            if ((path.status != NavMeshPathStatus.PathInvalid) && (path.corners.Length > 1))
            {
                // Debug.Log("Calculating Distance!");
                for (int i = 1; i < path.corners.Length; i++)
                {
                    lng += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                }
            }

            return lng;
        }

        public void removeTask(Vector3 toRemove)
        {
            Tasks.Remove(toRemove);
        }

        public Vector3 getTask()
        {

            // Debug.Log("There are " + Tasks.Count + " tasks left.");
            if (Tasks.Count == 0)
            {
                return new Vector3(0, 0, 0);
            }
            Vector3 closest = Tasks[0];
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(closest, path);
            float minDistance = getPathDistance(path);
            // Debug.Log(minDistance);
            for (int i = 1; i < Tasks.Count; i++)
            {
                if (!canReach(Tasks[i])) continue;
                agent.CalculatePath(Tasks[i], path);
                float distance = getPathDistance(path);
                // Debug.Log(distance); 
                if (distance < minDistance)
                {
                    // Debug.Log("Distance was altered");
                    minDistance = distance;
                    closest = Tasks[i];
                }
            }
            return closest;
        }
        private Vector3 getPos(Transform area)
        {
            float minX = area.position.x - area.localScale.x / 2;
            float maxX = area.position.x + area.localScale.x / 2;
            float minZ = area.position.z - area.localScale.z / 2;
            float maxZ = area.position.z + area.localScale.z / 2;
            float y = area.position.y + area.localScale.y / 2;

            Vector3 target;
            do
            {
                float x = Random.Range(minX, maxX);
                float z = Random.Range(minZ, maxZ);
                target = new Vector3(x, y, z);
            } while (!canReach(target));
            return target;
        }

        public void addPos(string type, Transform area)
        {
            /*if (type == "register")
            {
                registerPos = getPos(area);
            }
            else*/ if (type == "leaving")
            {
                leavingPos = getPos(area);
            }
        }

        public void addTaskInArea(Transform area)
        {
            Tasks.Add(getPos(area));
        }
    }
}
