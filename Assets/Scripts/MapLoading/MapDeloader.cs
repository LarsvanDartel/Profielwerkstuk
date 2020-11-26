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
                        map.spawningArea.pos = new Vector3(Mathf.Round(child.position.x*100)/100, Mathf.Round(child.position.y*100)/100, Mathf.Round(child.position.z*100)/100);
                        map.spawningArea.size = new Vector3(Mathf.Round(child.localScale.x*100)/100, Mathf.Round(child.localScale.y*100)/100, Mathf.Round(child.localScale.z*100)/100);
                        break;
                    }
                    case "LeavingGround": {
                        map.leavingArea.pos = new Vector3(Mathf.Round(child.position.x*100)/100, Mathf.Round(child.position.y*100)/100, Mathf.Round(child.position.z*100)/100);
                        map.leavingArea.size = new Vector3(Mathf.Round(child.localScale.x*100)/100, Mathf.Round(child.localScale.y*100)/100, Mathf.Round(child.localScale.z*100)/100);
                        break;
                    }
                    case "RegisterGround": {
                        map.registerArea.pos = new Vector3(Mathf.Round(child.position.x*100)/100, Mathf.Round(child.position.y*100)/100, Mathf.Round(child.position.z*100)/100);
                        map.registerArea.size = new Vector3(Mathf.Round(child.localScale.x*100)/100, Mathf.Round(child.localScale.y*100)/100, Mathf.Round(child.localScale.z*100)/100);
                        break;
                    }
                    case "TaskGround": {
                        map.taskArea.pos = new Vector3(Mathf.Round(child.position.x*100)/100, Mathf.Round(child.position.y*100)/100, Mathf.Round(child.position.z*100)/100);
                        map.taskArea.size = new Vector3(Mathf.Round(child.localScale.x*100)/100, Mathf.Round(child.localScale.y*100)/100, Mathf.Round(child.localScale.z*100)/100);
                        break;
                    }       
                    case "Doors": {
                        foreach(Transform block in child)
                        {
                            map.doors.Add(new Block());
                            map.doors[map.doors.Count-1].pos = new Vector3(Mathf.Round(block.position.x*100)/100, Mathf.Round(block.position.y*100)/100, Mathf.Round(block.position.z*100)/100);
                            map.doors[map.doors.Count-1].size = new Vector3(Mathf.Round(block.localScale.x*100)/100, Mathf.Round(block.localScale.y*100)/100, Mathf.Round(block.localScale.z*100)/100);
                        }
                        break;
                    }
                    case "Registers": {
                        foreach(Transform block in child)
                        {
                            map.registers.Add(new Block());
                            map.registers[map.registers.Count-1].pos = new Vector3(Mathf.Round(block.position.x*100)/100, Mathf.Round(block.position.y*100)/100, Mathf.Round(block.position.z*100)/100);
                            map.registers[map.registers.Count-1].size = new Vector3(Mathf.Round(block.localScale.x*100)/100, Mathf.Round(block.localScale.y*100)/100, Mathf.Round(block.localScale.z*100)/100);
                        }
                        break;
                    }
                    case "Walls": {
                        foreach(Transform block in child)
                        {
                            map.walls.Add(new Block());
                            map.walls[map.walls.Count-1].pos = new Vector3(Mathf.Round(block.position.x*100)/100, Mathf.Round(block.position.y*100)/100, Mathf.Round(block.position.z*100)/100);
                            map.walls[map.walls.Count-1].size = new Vector3(Mathf.Round(block.localScale.x*100)/100, Mathf.Round(block.localScale.y*100)/100, Mathf.Round(block.localScale.z*100)/100);
                        }
                        break;
                    }
                    case "Shelves": {
                        foreach(Transform block in child)
                        {
                            map.shelves.Add(new Block());
                            map.shelves[map.shelves.Count-1].pos = new Vector3(Mathf.Round(block.position.x*100)/100, Mathf.Round(block.position.y*100)/100, Mathf.Round(block.position.z*100)/100);
                            map.shelves[map.shelves.Count-1].size = new Vector3(Mathf.Round(block.localScale.x*100)/100, Mathf.Round(block.localScale.y*100)/100, Mathf.Round(block.localScale.z*100)/100);
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
