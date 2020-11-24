using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Profielwerkstuk { 
    public class MapDeloader : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Map map = new Map();
            foreach (Transform child in transform) 
            {
                foreach(Transform block in child)
                {
                    print(child.name);
                    switch(child.name.ToLower()) {
                        case "doors": {
                            map.doors.Add(new Block());
                            map.doors[map.doors.Count-1].pos = new Vector3(Mathf.Round(block.position.x), Mathf.Round(block.position.y), Mathf.Round(block.position.z));
                            map.doors[map.doors.Count-1].size = Vector3Int.RoundToInt(block.localScale);
                            break;
                        }
                        case "registers": {
                            map.registers.Add(new Block());
                            map.registers[map.registers.Count-1].pos = Vector3Int.RoundToInt(block.position);
                            map.registers[map.registers.Count-1].size = Vector3Int.RoundToInt(block.localScale);
                            break;
                        }
                        case "walls": {
                            map.walls.Add(new Block());
                            map.walls[map.walls.Count-1].pos = Vector3Int.RoundToInt(block.position);
                            map.walls[map.walls.Count-1].size = Vector3Int.RoundToInt(block.localScale);
                            break;
                        }
                        case "shelves": {
                            map.shelves.Add(new Block());
                            map.shelves[map.shelves.Count-1].pos = Vector3Int.RoundToInt(block.position);
                            map.shelves[map.shelves.Count-1].size = Vector3Int.RoundToInt(block.localScale);
                            break;
                        }
                    }
                    
                }
            }
            print(JsonUtility.ToJson(map));
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
