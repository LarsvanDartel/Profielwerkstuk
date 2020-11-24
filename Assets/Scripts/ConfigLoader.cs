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
            Config.coughCloudIncrease = config.coughCloudIncrease;
            Config.infectionRateCC = config.infectionRateCC; 
            Config.minCough = config.infectionRateCC; 
            Config.maxCough = config.maxCough; 
            Config.chanceInfected = config.chanceInfected;
            Config.spawnsPerHour = config.spawnsPerHour; 
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
