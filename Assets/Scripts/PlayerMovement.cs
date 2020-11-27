using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

namespace Profielwerkstuk
{
    public class PlayerMovement : MonoBehaviour
    {

        public NavMeshAgent agent;
        public TaskManager taskManager;
        public NavMeshObstacle obstacle;
        public CapsuleCollider triggerCollider;
        public Vector3 target;
        public string status;
        public List<GameObject> waitingFor;
        public GameObject coughCloudPrefab;
        public Material infectedMaterial;
        public Material asymptomaticMaterial;
        public Transform coughCloudParent;
        public bool infected = false;
        public bool asymptomatic = false;
        public bool waiting;
        private float timeSinceLastCough = 0.0f;
        private float timeUntilCough = 0.0f;
        // public Camera cam;
        public string id;
        public DataHoarder dataHoarder;
        public RegisterManager registerManager;
        public Vector3 register;
        void Start()
        {
            taskManager = new TaskManager(agent);
            timeUntilCough = Random.Range(Config.minCough, Config.maxCough);
            obstacle.enabled = false;
            agent.autoBraking = false;
            status = "ASSIGNING";
            waiting = false;
            register = new Vector3();
            waitingFor = new List<GameObject>();
        }

        // Update is called once per frame
        void Update()
        {
            /*if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit);
                target = hit.point;
                agent.SetDestination(target);
            }*/
            if (status == "ASSIGNING") return;
            if (infected && !asymptomatic)
            {
                // print(timeSinceLastCough);
                timeSinceLastCough += Time.deltaTime;
                if(timeUntilCough < timeSinceLastCough)
                {
                    timeSinceLastCough = 0.0f;
                    timeUntilCough = Random.Range(Config.minCough, Config.maxCough);
                    GameObject p = Instantiate(coughCloudPrefab, transform.position, transform.rotation, coughCloudParent);
                    p.transform.GetComponent<Rigidbody>().velocity = transform.forward * 0.7f;
                }
            }

            // check if agent has reached goal
            if (agent.enabled)
            {
                var pos = transform.position;
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        if (status == "ACTIVE") taskManager.RemoveTask(target);
                        if (taskManager.Tasks.Count == 0)
                        {
                            if (status == "ACTIVE")
                            {
                                if (registerManager.WaitForRegister(this, out target))
                                    status = "CHECKING OUT";
                                else
                                    status = "WAITING FOR REGISTER";

                                print(name + " " + status);
                                StartCoroutine(WaitForNextTask(Random.Range(5, 15)));
                            }
                            else if (status == "WAITING FOR REGISTER")
                            {
                                waiting = true;
                                StartCoroutine(ActivateObstacle());
                            }
                            else if (status == "CHECKING OUT")
                            {
                                register = target;
                                target = taskManager.leavingPos;
                                status = "LEAVING";
                                StartCoroutine(WaitForNextTask(Random.Range(20, 40)));
                            }
                            else if (status == "LEAVING")
                            {
                                dataHoarder.OnLeave(id, infected);
                                // print(name + " HAS LEFT");
                                Destroy(gameObject);
                            }
                        }
                        else
                        {
                            target = taskManager.GetTask();
                            StartCoroutine(WaitForNextTask(Random.Range(5, 15)));
                        }
                    }
                }
            }
            if (!waiting)
            {
                for (int i = waitingFor.Count - 1; i >= 0; i--)
                {
                    GameObject player = waitingFor[i];
                    if (player == null)
                    {
                        waitingFor.Remove(player);
                        continue;
                    }
                    PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
                    if (playerMovement.waiting)
                    {
                        waitingFor.Remove(player);
                        playerMovement.waitingFor.Add(gameObject);
                    }
                }
            }
            if (!waiting && waitingFor.Count == 0 && !agent.enabled)
            {
                StartCoroutine(DeactivateObstacle());
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.parent.name != "Players") return;
            if (other.GetComponent<NavMeshObstacle>().enabled) return;
            if (transform.GetSiblingIndex() < other.transform.GetSiblingIndex())
            {
                if (waitingFor.Contains(other.gameObject)) return;
                waitingFor.Add(other.gameObject);
                StartCoroutine(ActivateObstacle());
            }
            else
            {
                if (agent.enabled)
                {
                    if (status == "ACTIVE")
                    {
                        target = taskManager.GetTask();
                        agent.SetDestination(target);
                    }
                }                    
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.parent.name != "Players") return;
            waitingFor.Remove(other.gameObject);
        }

        IEnumerator WaitForNextTask(int s)
        {
            waiting = true;
            StartCoroutine(ActivateObstacle());
            yield return new WaitForSeconds(s);
            waiting = false;
            if(status == "LEAVING")
            {
                transform.parent.GetComponent<RegisterManager>().OnLeaveRegister(register);
            }
        }
        IEnumerator ActivateObstacle()
        {
            agent.enabled = false;
            yield return new WaitForEndOfFrame();
            while (agent.enabled)
            {
                agent.enabled = false;
                yield return null;
            }
            obstacle.enabled = true;
            yield return null;
        }

        IEnumerator DeactivateObstacle()
        {
            obstacle.enabled = false;
            yield return null;
            if(!obstacle.enabled) agent.enabled = true;
            yield return new WaitForEndOfFrame();
            if (agent.enabled) agent.SetDestination(target);
        }

        public void Infect()
        {
            Debug.Log(name + " is infected");
            infected = true;
            gameObject.GetComponent<MeshRenderer>().material = infectedMaterial;
        }
        public void Asymptomatic()
        {
            asymptomatic = true;
            infected = true;
            gameObject.GetComponent<MeshRenderer>().material = asymptomaticMaterial;
        }

        IEnumerator DestroyIn(float s)
        {
            yield return new WaitForSeconds(s);
            Destroy(gameObject);
        }

    }
}

