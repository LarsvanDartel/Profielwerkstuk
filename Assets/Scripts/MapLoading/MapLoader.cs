using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Profielwerkstuk {
    
    public class MapLoader : MonoBehaviour
    {
        public TextAsset JSONFile;
        private Map map;

        public Transform mapParent;

        public GameObject shelfParent;
        public GameObject registerParent;
        public GameObject doorParent;
        public GameObject wallParent;

        public GameObject shelfPrefab;
        public GameObject registerPrefab;
        public GameObject doorPrefab;
        public GameObject wallPrefab;

        public FlowManager flowManager;
        // Start is called before the first frame update
        void Start()
        {
            map = new Map();
            JsonUtility.FromJsonOverwrite(JSONFile.ToString(), map);
            
           

            buildMap(map);
        
        }

        void buildMap(Map map)
        {
            GameObject taskArea = new GameObject("TaskArea");
            taskArea.transform.position = map.taskArea.pos;
            taskArea.transform.localScale = map.taskArea.size;
            taskArea.transform.rotation = new Quaternion();
            taskArea.transform.parent = mapParent;
            flowManager.taskGround = taskArea.transform;

            GameObject registerArea = new GameObject("RegisterArea");
            registerArea.transform.parent = mapParent;
            registerArea.transform.position = map.registerArea.pos;
            registerArea.transform.rotation = new Quaternion();
            registerArea.transform.localScale = map.registerArea.size;
            flowManager.registerGround = registerArea.transform;

            GameObject leavingArea = new GameObject("LeavingArea");
            leavingArea.transform.parent = mapParent;
            leavingArea.transform.position = map.leavingArea.pos;
            leavingArea.transform.rotation = new Quaternion();
            leavingArea.transform.localScale = map.leavingArea.size;
            flowManager.leavingGround = leavingArea.transform;

            //print(map.spawningArea.pos);
            GameObject spawningArea = new GameObject("SpawningArea");
            spawningArea.transform.parent = mapParent;
            spawningArea.transform.position = map.spawningArea.pos;
            spawningArea.transform.rotation = new Quaternion();
            spawningArea.transform.localScale = map.spawningArea.size;
            flowManager.spawningGround = spawningArea.transform;



            foreach (Block shelf in map.shelves) {
                Instantiate(shelfPrefab, shelf.pos, new Quaternion(), shelfParent.transform).transform.localScale = shelf.size;
                //yield return new WaitForSecondsRealtime(0.1f);
            }
            foreach (Block door in map.doors) {
                Instantiate(doorPrefab, door.pos, new Quaternion(), doorParent.transform).transform.localScale = door.size;
                //yield return new WaitForSecondsRealtime(0.1f);
            }
            foreach (Block wall in map.walls) {
                Instantiate(wallPrefab, wall.pos, new Quaternion(), wallParent.transform).transform.localScale = wall.size;
                //yield return new WaitForSecondsRealtime(0.1f);
            }
            foreach (Block register in map.registers) {
                Instantiate(registerPrefab, register.pos, new Quaternion(), registerParent.transform).transform.localScale = register.size;
                //yield return new WaitForSecondsRealtime(0.1f);
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
