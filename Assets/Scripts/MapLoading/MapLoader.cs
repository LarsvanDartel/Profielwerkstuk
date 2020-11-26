using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace Profielwerkstuk {
    
    public class MapLoader : MonoBehaviour
    {
        public TextAsset JSONFile;
        private Map map;

        public Transform mapParent;

        public Transform shelfParent;
        public Transform registerParent;
        public Transform doorParent;
        public Transform wallParent;

        public GameObject shelfPrefab;
        public GameObject registerPrefab;
        public GameObject doorPrefab;
        public GameObject wallPrefab;
        public GameObject groundPrefab;
        public FlowManager flowManager;
        public RegisterManager registerManager;


        // Start is called before the first frame update
        void Start()
        {
            map = new Map();
            JsonUtility.FromJsonOverwrite(JSONFile.ToString(), map);
            buildMap(map);
            StartCoroutine(getTaskPositions());
        }

        void buildMap(Map map)
        {
            // GameObject ground = Instantiate(groundPrefab, map.ground.pos, Quaternion.identity, transform);
            // ground.transform.localScale = map.ground.size;
            // flowManager.ground = ground.transform;

            GameObject taskArea = new GameObject("TaskArea");
            taskArea.transform.parent = mapParent;
            taskArea.transform.position = map.taskArea.pos;
            taskArea.transform.rotation = Quaternion.identity;
            taskArea.transform.localScale = map.taskArea.size;
            flowManager.taskGround = taskArea.transform;

            GameObject registerArea = new GameObject("RegisterArea");
            registerArea.transform.parent = mapParent;
            registerArea.transform.position = map.registerArea.pos;
            registerArea.transform.rotation = Quaternion.identity;
            registerArea.transform.localScale = map.registerArea.size;
            flowManager.registerGround = registerArea.transform;

            GameObject leavingArea = new GameObject("LeavingArea");
            leavingArea.transform.parent = mapParent;
            leavingArea.transform.position = map.leavingArea.pos;
            leavingArea.transform.rotation = Quaternion.identity;
            leavingArea.transform.localScale = map.leavingArea.size;
            flowManager.leavingGround = leavingArea.transform;

            //print(map.spawningArea.pos);
            GameObject spawningArea = new GameObject("SpawningArea");
            spawningArea.transform.parent = mapParent;
            spawningArea.transform.position = map.spawningArea.pos;
            spawningArea.transform.rotation = Quaternion.identity;
            spawningArea.transform.localScale = map.spawningArea.size;
            flowManager.spawningGround = spawningArea.transform;

            registerManager.addRegister(new Vector3(16, 1, 3));
            registerManager.addRegister(new Vector3(16, 1, -1));
            registerManager.addRegister(new Vector3(16, 1, -5));
            registerManager.addRegister(new Vector3(13, 1, -13));
            registerManager.addRegister(new Vector3(19, 1, -13));
            registerManager.addRegister(new Vector3(13, 1, -19));
            registerManager.addRegister(new Vector3(19, 1, -19));
            registerManager.addRegister(new Vector3(13, 1, -21));
            registerManager.addRegister(new Vector3(19, 1, -21));

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

        IEnumerator getTaskPositions()
        {
            yield return null;
            flowManager.taskPositions = new List<Vector3>();
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
                        flowManager.taskPositions.Add(hit.position);
                    }
                    newPos = new Vector3(pos.x + x - scale.x / 2, -1, pos.z - scale.z / 2 - 1);
                    if (NavMesh.SamplePosition(newPos, out hit, 0.1f, NavMesh.AllAreas))
                    {
                        flowManager.taskPositions.Add(hit.position);
                    }
                }
                // find all positions along the x-axis
                for (float z = 2; z < scale.z; z += 2)
                {
                    Vector3 newPos = new Vector3(pos.x + scale.x / 2 + 1, -1, pos.z - scale.z / 2 + z);
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(newPos, out hit, 0.1f, NavMesh.AllAreas))
                    {
                        flowManager.taskPositions.Add(hit.position);
                    }
                    newPos = new Vector3(pos.x - scale.x / 2 - 1, -1, pos.z - scale.z / 2 + z);
                    if (NavMesh.SamplePosition(newPos, out hit, 0.1f, NavMesh.AllAreas))
                    {
                        flowManager.taskPositions.Add(hit.position);
                    }
                }
            }
        }

    }
}
