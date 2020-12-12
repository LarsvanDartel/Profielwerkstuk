using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Profielwerkstuk { 
    [System.Serializable]
    public class EmptyConfig
    {
        public float speed; // the speed of the simulation
        public float CCMaxSize; // the rate at which the cough cloud increases and the chance decreases
        public float CCStart; // the starting chance you get infected from a cough cloud
        public float CCDuration;
        public float CCSteepness;
        public float minCough; // the minimal time between each cough
        public float maxCough; // the maximal time between each cough
        public float chanceInfected; // a chance 0-1 someone spawns infected
        public float playersPerDay; // how many people spawn per hour
        public float playerDistributionMean; // the mean of the gaussian distribution for the players
        public float playerDistributionStandardDeviation; // the standard deviation of the gaussian distribution for the players
        public float openingTime; // the shop's opening time
        public float closingTime; // the shop's closing time
        public float participationRate;
    }
    public static class Config {
        public static float speed; // the speed of the simulation
        public static float CCMaxSize; // the rate at which the cough cloud increases and the chance decreases
        public static float CCStart; // the starting chance you get infected from a cough cloud
        public static float CCDuration;
        public static float CCSteepness;
        public static float minCough; // the minimal time between each cough
        public static float maxCough; // the maximal time between each cough
        public static float chanceInfected; // a chance 0-1 someone spawns infected
        public static float playersPerDay; // how many people spawn per hour       
        public static float playerDistributionMean; // the mean of the gaussian distribution for the players
        public static float playerDistributionStandardDeviation; // the standard deviation of the gaussian distribution for the players
        public static float openingTime; // the shop's opening time
        public static float closingTime; // the shop's closing time
        public static float participationRate;
    }
}