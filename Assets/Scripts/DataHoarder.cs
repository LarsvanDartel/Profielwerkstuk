using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
namespace Profielwerkstuk { 
    public class DataHoarder : MonoBehaviour {
        public FlowManager flowManager;
        private OutputData outputData = new OutputData();
        public void OnSpawn(string id, bool infected) {
            outputData.shoppers.Add(new Shopper(id, infected));
            // Debug.Log(id);
        }
        public void OnLeave(string id, bool infected) {
            Shopper shopper = outputData.shoppers.Find(s => s.id == id);
            shopper.infectedEnd = infected;
            shopper.timeDespawned = Time.time;
            outputData.peopleLeft++;
            //print(shopper.timeDespawned);
            if(outputData.peopleLeft == Config.playersPerDay) OnEnd();
        }
        public void OnEnd() {
            //string csvFile = "";
            CultureInfo ci = new CultureInfo("nl-NL");
            var sr = File.CreateText("woopers.csv");
            sr.WriteLine("ID;Infected Start;Infected End;Time Start;Time End");
            foreach(Shopper shopper in outputData.shoppers) {
                sr.WriteLine(shopper.id + ";" + shopper.infectedStart + ";" + shopper.infectedEnd + ";" + shopper.timeSpawned.ToString(ci) + ";" + shopper.timeDespawned.ToString(ci));
            }
            sr.Close();
            flowManager.ReStart();
            outputData = new OutputData();
        }
    }

    [System.Serializable]
    public class OutputData {
        public List<Shopper> shoppers = new List<Shopper>();
        public int peopleLeft = 0;
    }

    [System.Serializable]
    public class Shopper {
        public string id;
        public bool infectedStart;
        public bool infectedEnd;
        public float timeSpawned;
        public float timeDespawned;
    
        public Shopper(string _id, bool _infected) {
            id = _id;
            infectedStart = _infected;

            timeSpawned = Time.time;
        }

    }
}