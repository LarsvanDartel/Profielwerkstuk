using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Profielwerkstuk { 
    public class ConfigLoader : MonoBehaviour
    {
        public TextAsset JSONFile;
        private EmptyConfig config;

        public PlayerMovement playerMovement;
        // Start is called before the first frame update
        void Start()
        {
            config = JsonUtility.FromJson<EmptyConfig>(JSONFile.ToString());

            print(EmptyConfig.speed);
            // Time.timeScale = config.speed;
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
