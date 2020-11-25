using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace Profielwerkstuk
{
    public class FlowManager : MonoBehaviour
    {

        // private float time = 0.0f;

        /* When spawning a new player, the following variables need to be assigned:
         * Position
         * Ground
         * Number of tasks
         * Parent of coughclouds
        */

        public Transform spawningGround;
        public Transform taskGround;
        public Transform registerGround;
        public Transform leavingGround;
    
        public Transform ground;
        public Transform coughClouds;

        public GameObject playerPrefab;

        public RegisterManager registerManager;
        public List<Vector3> registerPositions = new List<Vector3>();

        public DataHoarder dataHoarder;

        // Start is called before the first frame update
        void Start()
        {
            Time.timeScale = Config.speed;
            StartCoroutine(spawnPlayers((int)Config.spawnsPerHour));
        }

        IEnumerator spawnPlayers(int numPlayers)
        {
            yield return null;
            foreach(Vector3 pos in registerPositions)
            {
                registerManager.addRegister(pos);
            }
            var minX = spawningGround.position.x - spawningGround.localScale.x / 2;
            var maxX = spawningGround.position.x + spawningGround.localScale.x / 2;
            var minZ = spawningGround.position.z - spawningGround.localScale.z / 2;
            var maxZ = spawningGround.position.z + spawningGround.localScale.z / 2;
            var y = spawningGround.position.y + spawningGround.localScale.y / 2;


            for (int i = 0; i < numPlayers; i++)
            {
                // print("Spawning player " + (i + 1));

                // Generates starting position
                float x = Random.Range(minX, maxX);
                float z = Random.Range(minZ, maxZ);
                var spawnPosition = new Vector3(x, y, z);

                // Spawns player    
                GameObject player = Instantiate(playerPrefab, spawnPosition, Quaternion.identity, transform);
                yield return new WaitForEndOfFrame();
                player.name = "" + (i + 1);
                PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
                playerMovement.coughCloudParent = coughClouds;
                playerMovement.dataHoarder = dataHoarder;
                playerMovement.id = "" + (i + 1);
                // Infects player
                if (Random.Range(0.0f, 1.0f) >= Config.chanceInfected)
                {
                    playerMovement.infected = true;
                    player.GetComponent<MeshRenderer>().material = playerMovement.infectedMaterial;
                }

                dataHoarder.onSpawn(player.name, playerMovement.infected);
                // Assigns Tasks
                TaskManager taskManager = playerMovement.taskManager;
                NavMeshAgent agent = playerMovement.agent;
                int numTasks = Random.Range(5, 10);
                for(int t = 0; t < numTasks; t++)
                {
                    taskManager.addTaskInArea(taskGround);
                }
                // taskManager.addPos("register", registerGround);
                taskManager.addPos("leaving", leavingGround);

                foreach(Vector3 pos in registerGround)
                {
                    taskManager.registerPositions.Add(pos);
                }
                taskManager.waitingForRegisterPos = new Vector3(11, 1, -23);

                playerMovement.target = taskManager.getTask();
                agent.SetDestination(playerMovement.target);

                playerMovement.status = "ACTIVE";
                yield return new WaitForSeconds(35);

            }
        }

        /*void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit))
                {
                    registerPositions.Add(hit.point);
                    //print("x: " + hit.point.x + ", z: " + hit.point.z);
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                StartCoroutine(showPositions());
            }
        }

        IEnumerator showPositions()
        {
            foreach(Vector3 pos in registerPositions)
            {
                print("x: " + pos.x + ", z: " + pos.z);
                showingThingy.transform.position = pos;
                yield return new WaitForSeconds(1);
            }
        }*/
    }
}