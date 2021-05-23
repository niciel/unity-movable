using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChunkSystem
{
    public struct MerchBlockVertex 
    {
        public Vector3[] vertex;
        public int[] conrnerBlockID;
        public Vector3[] cornerBlockVector;
        public int[] opositCornerBlockID;
        //public bool isEmpty;
    }
}
