using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Profielwerkstuk { 
    public class CoughCloudManager : MonoBehaviour
    {
        public float infectionRate;
        public float startTime;
        private List<PlayerMovement> playersInCloud;
        void Start()
        {
            playersInCloud = new List<PlayerMovement>();
            infectionRate = Config.CCStart;
    	    startTime = Time.time;
            StartCoroutine(DestroyIn(Config.CCDuration));

        }

        void Update()
        {
            float size = SizeFormula(Time.time-startTime);
            //print(size);
            transform.localScale = new Vector3(size, size, size);
            float velocity = SpeedFormula(Time.time-startTime);
            //print(velocity + " : " + (Time.time-startTime));
            transform.GetComponent<Rigidbody>().velocity = transform.GetComponent<Rigidbody>().velocity.normalized*velocity;
            infectionRate = InfectionFormula(Time.time-startTime);

            for(int i = playersInCloud.Count-1; i >= 0; i--)
            {
                var player = playersInCloud[i];
                // print(player.infected);
                // print(1-Mathf.Pow(1-infectionRate,Time.deltaTime*60));

                if(player == null)
                {
                    playersInCloud.Remove(player);
                    continue;
                }

                if (Random.Range(0.0f, 1.0f) < 1-Mathf.Pow(1-infectionRate,Time.deltaTime*60))
                {
                    player.Asymptomatic();
                    playersInCloud.Remove(player);
                }
            }
        }

        float SizeFormula(float x) {
            return -Config.CCSteepness/(x+(Config.CCSteepness/Config.CCMaxSize))+Config.CCMaxSize;
        }

        float SpeedFormula(float x) {
            return 1/((Config.CCSteepness/10)*x+(1/3f));
        }

        float InfectionFormula(float x) {
            return 1/(Config.CCSteepness*x + 1/Config.CCStart);
        }

        IEnumerator DestroyIn(float s)
        {
            yield return new WaitForSeconds(s);
            Destroy(gameObject);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.transform.parent.name != "Players") return;
            if (other.GetComponent<PlayerMovement>().infected) return;
            if (!playersInCloud.Contains(other.gameObject.GetComponent<PlayerMovement>())) { playersInCloud.Add(other.GetComponent<PlayerMovement>()); }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.transform.parent.name != "Players") return;
            if (other.GetComponent<PlayerMovement>().infected) return;
            playersInCloud.Remove(other.GetComponent<PlayerMovement>());
        }

    }
}