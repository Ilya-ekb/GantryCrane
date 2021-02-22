using Assets.Scripts.InputSystem;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts.ManageScripts
{
    public class Dashboard : MonoBehaviour
    {
        public CursorController input;
        public Node[] nodes;

        private void Start()
        {
            
            foreach(var node in nodes)
            {
                node.Connect();
            }
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                input.OnAttach();
            }
            if (Input.GetMouseButtonUp(0))
            {
                input.OnDetach();
            }
        }
    }
}
