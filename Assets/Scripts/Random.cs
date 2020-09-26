using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Profielwerkstuk
{
    public class Random
    {
        private static readonly System.Random _random = new System.Random();

        public static double random(float min, float max)
        {
            return min + _random.NextDouble() * (max - min);
        }
    }

}