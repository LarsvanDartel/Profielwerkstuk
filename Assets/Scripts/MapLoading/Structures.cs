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
        public Block spawningArea;
        public Block leavingArea;
        public Block registerArea;
        public Block taskArea;
        public Block ground;
    }
    [System.Serializable]
    public class Block
    {
        public Vector3 pos;
        public Vector3 size;

        public Block(Vector3 _pos, Vector3 _size)
        {
            pos = _pos;
            size = _size;
        }

    }
}