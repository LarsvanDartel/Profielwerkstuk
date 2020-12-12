using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Profielwerkstuk { 
    public class ConfigLoader : MonoBehaviour
    {
        public TextAsset JSONFile;
        public EmptyConfig config;
        // Start is called before the first frame update
        void Start()
        {
            config = JsonUtility.FromJson<EmptyConfig>(JSONFile.ToString());

            Config.speed = config.speed; 

            Config.CCMaxSize = config.CCMaxSize; 
            Config.CCStart = config.CCStart; 
            Config.CCSteepness = config.CCSteepness; 
            Config.CCDuration = config.CCDuration; 

            Config.minCough = config.minCough;  
            Config.maxCough = config.maxCough;  
            Config.chanceInfected = config.chanceInfected; 
            Config.playersPerDay = config.playersPerDay;
            Config.playerDistributionMean = config.playerDistributionMean;
            Config.playerDistributionStandardDeviation = config.playerDistributionStandardDeviation;
            Config.openingTime = config.openingTime;
            Config.closingTime = config.closingTime;
            Config.participationRate = config.participationRate;
        }
    }
}
