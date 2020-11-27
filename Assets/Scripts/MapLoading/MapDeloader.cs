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
                Vector3 pos = Utility.Round(child.position, 2);
                Vector3 scale = Utility.Round(Utility.Abs(Utility.Rotate(child.localScale) * 2), 2);
                switch (child.name) { 
                    case "SpawningArea":
                        {
                            map.spawningArea = new Block(pos, scale);
                            break;
                        }
                    case "LeavingArea":
                        {
                            map.leavingArea = new Block(pos, scale);
                            break;
                        }
                    case "Doors":
                        {
                            foreach (Transform block in child)
                            {
                                scale = Utility.Round(Utility.Abs(Utility.Rotate(block.localScale) * 2), 2);
                                map.doors.Add(new Block(Utility.Round(block.position, 2), scale));
                            }
                            break;
                        }
                    case "Registers":
                        {
                            foreach (Transform block in child)
                            {
                                scale = Utility.Round(Utility.Abs(Utility.Rotate(block.localScale) * 2), 2);
                                map.registers.Add(new Block(Utility.Round(block.position, 2), scale));
                            }
                            break;
                        }
                    case "Walls":
                        {
                            foreach (Transform block in child)
                            {
                                scale = Utility.Round(Utility.Abs(Utility.Rotate(block.localScale) * 2), 2);
                                map.walls.Add(new Block(Utility.Round(block.position, 2), scale));
                            }
                            break;
                        }
                    case "Shelves":
                        {
                            foreach (Transform block in child)
                            {
                                scale = Utility.Round(Utility.Abs(Utility.Rotate(block.localScale) * 2), 2);
                                map.shelves.Add(new Block(Utility.Round(block.position, 2), scale));
                            }
                            break;
                        }
                }    
            }
                
            
            print(JsonUtility.ToJson(map));
        }
    }
}
