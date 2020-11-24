using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Profielwerkstuk {
    
    public class MapLoader : MonoBehaviour
    {
        public TextAsset JSONFile;
        private Map map;
        // Start is called before the first frame update
        void Start()
        {
            print("what");
            map = JsonUtility.FromJson<Map>(JSONFile.ToString());

            print(map.shelves[0].pos.x);
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
