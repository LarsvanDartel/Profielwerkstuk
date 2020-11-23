using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Profielwerkstuk
{
    public class TaskManager
    {
        public List<Vector3> Tasks;
        private NavMeshAgent agent;
        private GameObject ground;
        private int numTasks;

        public TaskManager(GameObject _ground, int _numTasks, NavMeshAgent _agent)
        {
            Tasks = new List<Vector3>();
            agent = _agent;
            ground = _ground;
            numTasks = _numTasks;

            var minX = ground.transform.position.x - ground.transform.localScale.x / 2;
            var maxX = ground.transform.position.x + ground.transform.localScale.x / 2;
            var minZ = ground.transform.position.z - ground.transform.localScale.z / 2;
            var maxZ = ground.transform.position.z + ground.transform.localScale.z / 2;
            var y = 1;

            // Debug.Log("X-range: " + minX + ", " + maxX);
            // Debug.Log("Z-range: " + minZ + ", " + maxZ);

            for (int i = 0; i < numTasks; i++)
            {
                Vector3 target;
                do
                {
                    float x = (float)Random.Range(minX, maxX);
                    float z = (float)Random.Range(minZ, maxZ);
                    target = new Vector3(x, y, z);
                    // Debug.Log("canReach(target: " + x + ", " + y + ", " + z + ") = " + canReach(target));
                } while (!canReach(target));
                Tasks.Add(target);
                // Debug.Log(target.x + ", " + target.y + ", " + target.z);
            }
            // Debug.Log(Tasks);
            // Debug.Log(Tasks.Count);
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

        public Vector3 getTask(NavMeshAgent agent)
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

    }
}
