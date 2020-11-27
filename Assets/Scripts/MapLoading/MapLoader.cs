using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace Profielwerkstuk {
    
    public class MapLoader : MonoBehaviour
    {
        public TextAsset JSONFile;
        private Map map;

        public Transform shelfParent;
        public Transform registerParent;
        public Transform doorParent;
        public Transform wallParent;

        public GameObject shelfPrefab;
        public GameObject registerPrefab;
        public GameObject doorPrefab;
        public GameObject wallPrefab;
        public FlowManager flowManager;
        public RegisterManager registerManager;


        // Start is called before the first frame update
        void Start()
        {
            map = new Map();
            JsonUtility.FromJsonOverwrite(JSONFile.ToString(), map);
            BuildMap(map);
            StartCoroutine(GetTaskPositions());
            StartCoroutine(GetRegisterPositions());
            flowManager.registerManager = registerManager;
        }

        void BuildMap(Map map)
        {
            GameObject leavingArea = new GameObject("LeavingArea");
            
            leavingArea.transform.parent = transform;
            leavingArea.transform.position = map.leavingArea.pos;
            leavingArea.transform.rotation = Quaternion.identity;
            leavingArea.transform.localScale = map.leavingArea.size;
            flowManager.leavingArea = leavingArea.transform;

            //print(map.spawningArea.pos);
            GameObject spawningArea = new GameObject("SpawningArea");
            spawningArea.transform.parent = transform;
            spawningArea.transform.position = map.spawningArea.pos;
            spawningArea.transform.rotation = Quaternion.identity;
            spawningArea.transform.localScale = map.spawningArea.size;
            flowManager.spawningArea = spawningArea.transform;

            foreach (Block shelf in map.shelves) {
                Instantiate(shelfPrefab, shelf.pos, Quaternion.identity, shelfParent).transform.localScale = shelf.size;
                //yield return new WaitForSecondsRealtime(0.1f);
            }
            foreach (Block door in map.doors) {
                Instantiate(doorPrefab, door.pos, Quaternion.identity, doorParent).transform.localScale = door.size;
                //yield return new WaitForSecondsRealtime(0.1f);
            }
            foreach (Block wall in map.walls) {
                Instantiate(wallPrefab, wall.pos, Quaternion.identity, wallParent).transform.localScale = wall.size;
                //yield return new WaitForSecondsRealtime(0.1f);
            }
            foreach (Block register in map.registers)
            {
                Instantiate(registerPrefab, register.pos, Quaternion.identity, registerParent).transform.localScale = register.size;
                //yield return new WaitForSecondsRealtime(0.1f);
            }
        }

        IEnumerator GetTaskPositions()
        {
            yield return null;
            var taskPositions = new List<Vector3>();

            // generates positions for tasks:
            foreach (Transform child in shelfParent)
            {
                Vector3 scale = child.localScale;
                Vector3 pos = child.position;

                // find all positions along the x-axis
                for (float x = 2; x < scale.x; x += 2)
                {
                    Vector3 newPos = new Vector3(pos.x + x - scale.x / 2, -1, pos.z + scale.z / 2 + 1);
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(newPos, out hit, 0.1f, NavMesh.AllAreas))
                    {
                        taskPositions.Add(hit.position);
                    }
                    newPos = new Vector3(pos.x + x - scale.x / 2, -1, pos.z - scale.z / 2 - 1);
                    if (NavMesh.SamplePosition(newPos, out hit, 0.1f, NavMesh.AllAreas))
                    {
                        taskPositions.Add(hit.position);
                    }
                }
                // find all positions along the x-axis
                for (float z = 2; z < scale.z; z += 2)
                {
                    Vector3 newPos = new Vector3(pos.x + scale.x / 2 + 1, -1, pos.z - scale.z / 2 + z);
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(newPos, out hit, 0.1f, NavMesh.AllAreas))
                    {
                        taskPositions.Add(hit.position);
                    }
                    newPos = new Vector3(pos.x - scale.x / 2 - 1, -1, pos.z - scale.z / 2 + z);
                    if (NavMesh.SamplePosition(newPos, out hit, 0.1f, NavMesh.AllAreas))
                    {
                        taskPositions.Add(hit.position);
                    }
                }
            }
            flowManager.taskPositions = taskPositions;
        }
        IEnumerator GetRegisterPositions()
        {
            yield return null;
            // generates positions for tasks:
            foreach (Transform child in registerParent)
            {
                Vector3 scale = child.localScale;
                Vector3 pos = child.position;

                // find all positions along the x-axis
                Vector3 newPos = new Vector3(pos.x, -1, pos.z + scale.z / 2 + 1);
                NavMeshHit hit;
                if (NavMesh.SamplePosition(newPos, out hit, 0.1f, NavMesh.AllAreas))
                {
                    registerManager.AddRegister(hit.position);
                }
                newPos = new Vector3(pos.x, -1, pos.z - scale.z / 2 - 1);
                if (NavMesh.SamplePosition(newPos, out hit, 0.1f, NavMesh.AllAreas))
                {
                    registerManager.AddRegister(hit.position);
                }
            }
            registerManager.waitingForRegisterPos = new Vector3(11, 1, -23);

            //Utility.printDict(registerManager.registerTaken);
        }
    }
}
