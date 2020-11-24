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
                print(child.name);
                switch(child.name) {
                    case "SpawningGround": {
                        map.spawningArea.pos = Vector3Int.RoundToInt(child.position);
                        map.spawningArea.size = Vector3Int.RoundToInt(child.localScale);
                        break;
                    }
                    case "LeavingGround": {
                        map.leavingArea.pos = Vector3Int.RoundToInt(child.position);
                        map.leavingArea.size = Vector3Int.RoundToInt(child.localScale);
                        break;
                    }
                    case "RegisterGround": {
                        map.registerArea.pos = Vector3Int.RoundToInt(child.position);
                        map.registerArea.size = Vector3Int.RoundToInt(child.localScale);
                        break;
                    }
                    case "TaskGround": {
                        map.taskArea.pos = Vector3Int.RoundToInt(child.position);
                        map.taskArea.size = Vector3Int.RoundToInt(child.localScale);
                        break;
                    }       
                    case "Doors": {
                        foreach(Transform block in child)
                        {
                            map.doors.Add(new Block());
                            map.doors[map.doors.Count-1].pos = new Vector3(Mathf.Round(block.position.x), Mathf.Round(block.position.y), Mathf.Round(block.position.z));
                            map.doors[map.doors.Count-1].size = Vector3Int.RoundToInt(block.localScale);
                        }
                        break;
                    }
                    case "Registers": {
                        foreach(Transform block in child)
                        {
                            map.registers.Add(new Block());
                            map.registers[map.registers.Count-1].pos = Vector3Int.RoundToInt(block.position);
                            map.registers[map.registers.Count-1].size = Vector3Int.RoundToInt(block.localScale);
                        }
                        break;
                    }
                    case "Walls": {
                        foreach(Transform block in child)
                        {
                            map.walls.Add(new Block());
                            map.walls[map.walls.Count-1].pos = Vector3Int.RoundToInt(block.position);
                            map.walls[map.walls.Count-1].size = Vector3Int.RoundToInt(block.localScale);
                        }
                        break;
                    }
                    case "Shelves": {
                        foreach(Transform block in child)
                        {
                            map.shelves.Add(new Block());
                            map.shelves[map.shelves.Count-1].pos = Vector3Int.RoundToInt(block.position);
                            map.shelves[map.shelves.Count-1].size = Vector3Int.RoundToInt(block.localScale);
                        }
                        break;
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
