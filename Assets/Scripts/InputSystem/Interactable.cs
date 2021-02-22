using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.InputSystem
{
    public abstract class Interactable : MonoBehaviour
    {
        public float Signal { get { return signal; } }

        [Header("Событие при взаимодействии с объектом:")]
        public UnityEvent OnInteractableUpdate;
        public int Axis { get { return axis; } }

        protected int axis = 0;
        protected float signal;


        public virtual void InteractableBegin (Vector3 input) { }
        public virtual void InteractableUpdate(Vector3 input) { OnInteractableUpdate?.Invoke(); }
        public virtual void InteractableEnd() { }
        public virtual void OnFocusBegin() { }
    }
}
