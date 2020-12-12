using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Profielwerkstuk {
    public class RegisterManager : MonoBehaviour
    {
        public Queue<PlayerMovement> playersWaiting;
        public List<Vector3> registers;
        public Dictionary<Vector3, bool> registerTaken;
        public Vector3 waitingForRegisterPos;
        void Start()
        {
            playersWaiting = new Queue<PlayerMovement>();
            registers = new List<Vector3>();
            registerTaken = new Dictionary<Vector3, bool>();
        }

        public void OnLeaveRegister(Vector3 register)
        {
            if (!registerTaken.ContainsKey(register))
            {
                //print("THERE IS NO SUCH REGISTER");
                return;
            }
            registerTaken[register] = false;
            if(playersWaiting.Count > 0)
            {
                var player = playersWaiting.Dequeue();
                player.register = register;
                player.target = register;
                player.waiting = false;
            }
        }

        public void OnEnterRegister(Vector3 register)
        {
            if (!registerTaken.ContainsKey(register))
            {
                //print("THERE IS NO SUCH REGISTER");
                return;
            }
            registerTaken[register] = true;
        }

        public void AddRegister(Vector3 register)
        {
            if (registerTaken.ContainsKey(register)) return;
            registerTaken.Add(register, false);
        }

        public bool WaitForRegister(PlayerMovement player, out Vector3 target)
        {
            bool registerFree = false;
            Vector3 registerPos = new Vector3();
            foreach(var pair in registerTaken)
            {
                if (!pair.Value)
                {
                    registerFree = true;
                    registerPos = pair.Key;
                    break;
                }
            }
            if (!registerFree)
            {
                playersWaiting.Enqueue(player);
                target = waitingForRegisterPos;
                return false;
            } else
            {
                target = registerPos;
                OnEnterRegister(registerPos);
                return true;
            }
        }
    }
}
