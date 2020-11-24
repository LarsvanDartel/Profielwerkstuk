using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Profielwerkstuk { 
    public class CoughCloudManager : MonoBehaviour
    {
        public float infectionRate;
        private float sizeIncrease = Config.coughCloudIncrease * Config.speed * 60;
        private List<PlayerMovement> playersInCloud;
        void Start()
        {
            playersInCloud = new List<PlayerMovement>();
            infectionRate = Config.infectionRateCC;
            StartCoroutine(destroyIn(Config.coughDuration/Config.speed));

        }

        void Update()
        {
            var newScale = transform.localScale;
            newScale *= (1+sizeIncrease);
            transform.localScale = newScale;

            infectionRate /= Mathf.Pow((1+sizeIncrease), 3);

            for(int i = playersInCloud.Count-1; i >= 0; i--)
            {
                var player = playersInCloud[i];
                if (Random.Range(0.0f, 1.0f) < infectionRate)
                {
                    player.infect();
                    playersInCloud.Remove(player);
                }
            }
        }


        IEnumerator destroyIn(float s)
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