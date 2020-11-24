using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Profielwerkstuk { 
    [System.Serializable]
    public class Map
    {
        public List<Block> shelves = new List<Block>();
        public List<Block> registers = new List<Block>();
        public List<Block> walls = new List<Block>();
        public List<Block> doors = new List<Block>();

    }
    [System.Serializable]
    public class Block
    {
        public Vector3 pos;
        public Vector3 size;
    }
}