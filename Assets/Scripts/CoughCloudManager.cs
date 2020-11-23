using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Profielwerkstuk { 
    public class CoughCloudManager : MonoBehaviour
    {
        public float infectionRate;
        private float sizeIncrease = 0.02f;
        private List<PlayerMovement> playersInCloud;
        void Start()
        {
            playersInCloud = new List<PlayerMovement>();
            infectionRate = 1.0f;
            StartCoroutine(destroyIn(5000));

        }

        void Update()
        {
            var newScale = transform.localScale;
            newScale += new Vector3(sizeIncrease, sizeIncrease, sizeIncrease);
            transform.localScale = newScale;

            infectionRate -= Mathf.Pow(sizeIncrease, 3);

            for(int i = playersInCloud.Count-1; i >= 0; i++)
            {
                var player = playersInCloud[i];
                if (Random.Range(0.0f, 1.0f) < infectionRate)
                {
                    player.infect();
                    playersInCloud.Remove(player);
                }
            }
        }


        IEnumerator destroyIn(int ms)
        {
            yield return new WaitForSeconds(ms / 1000);
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