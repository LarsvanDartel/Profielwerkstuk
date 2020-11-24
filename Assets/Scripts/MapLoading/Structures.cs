using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Profielwerkstuk { 
    [System.Serializable]
    public class Map
    {
        public Block[] shelves;
        public Block[] registers;
        public Block[] walls;
        public Block[] doors;
    }
    [System.Serializable]
    public class Block
    {
        public Vector3 pos;
        public Vector3 size;
        public Block(float x, float y, float z, float x1, float y1, float z1) // length
        {
            pos = new Vector3(x, y, z);
            size = new Vector3(x1, y1, z1);
            
        }
    }
}