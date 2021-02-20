using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts.InputSystem
{
    public class Interactable : MonoBehaviour
    {
        public int Axis { get { return axis; } }

        protected int axis = 0;
        public virtual void InteractableBegin (Vector3 input) { }
        public virtual void InteractableUpdate(Vector3 input) { }
        public virtual void InteractableEnd() { }

        public virtual void OnFocusBegin()
        {

        }
    }
}
