using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Profielwerkstuk { 
    [System.Serializable]
    public class EmptyConfig
    {
        public float speed; // the speed of the simulation
        public float coughCloudIncrease; // the rate at which the cough cloud increases and the chance decreases
        public float infectionRateCC; // the starting chance you get infected from a cough cloud
        public float minCough; // the minimal time between each cough
        public float maxCough; // the maximal time between each cough
        public float chanceInfected; // a chance 0-10 someone spawns infected
        public float spawnsPerHour; // how many people spawn per hour       

    }
    public static class Config {
        public static float speed; // the speed of the simulation
        public static float coughCloudIncrease; // the rate at which the cough cloud increases and the chance decreases
        public static float infectionRateCC; // the starting chance you get infected from a cough cloud
        public static float minCough; // the minimal time between each cough
        public static float maxCough; // the maximal time between each cough
        public static float chanceInfected; // a chance 0-10 someone spawns infected
        public static float spawnsPerHour; // how many people spawn per hour       

    }
}