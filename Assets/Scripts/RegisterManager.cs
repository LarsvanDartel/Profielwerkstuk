using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Profielwerkstuk {
    public class RegisterManager : MonoBehaviour
    {
        public Queue<PlayerMovement> playersWaiting;
        public List<Vector3> registers;
        public Dictionary<Vector3, bool> registerTaken;
        void Start()
        {
            playersWaiting = new Queue<PlayerMovement>();
            registers = new List<Vector3>();
            registerTaken = new Dictionary<Vector3, bool>();
        }

        public void onLeaveRegister(Vector3 register)
        {
            if (!registerTaken.ContainsKey(register))
            {
                print("THERE IS NO SUCH REGISTER");
                return;
            }
            registerTaken[register] = false;
            if(playersWaiting.Count > 0)
            {
                var player = playersWaiting.Peek();
                player.register = register;
                player.target = register;
                player.waiting = false;
                playersWaiting.Dequeue();
            }
        }

        public void onEnterRegister(Vector3 register)
        {
            if (!registerTaken.ContainsKey(register))
            {
                print("THERE IS NO SUCH REGISTER");
                return;
            }
            registerTaken[register] = true;
        }

        public void addRegister(Vector3 register)
        {
            registerTaken.Add(register, false);
        }

        public bool waitForRegister(PlayerMovement player)
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
                return false;
            } else
            {
                player.target = registerPos;
                onEnterRegister(registerPos);
                return true;
            }
        }
    }
}
