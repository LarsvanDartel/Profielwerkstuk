using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Profielwerkstuk {
    
    public class MapLoader : MonoBehaviour
    {
        public TextAsset JSONFile;
        private Map map;

        public GameObject shelfParent;
        public GameObject registerParent;
        public GameObject doorParent;
        public GameObject wallParent;

        public GameObject shelfPrefab;
        public GameObject registerPrefab;
        public GameObject doorPrefab;
        public GameObject wallPrefab;

        // Start is called before the first frame update
        void Start()
        {
            map = JsonUtility.FromJson<Map>(JSONFile.ToString());
            
            buildMap(map);
        }

        void buildMap(Map map)
        {
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
