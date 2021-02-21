
using System;
using System.Collections;

using UnityEngine;

namespace Assets.Scripts.ManageScripts
{
    public class ControlledObject : MonoBehaviour
    {
        public virtual Vector3 position { get => transform.position; set => transform.position = value; }
        public float Mass { get { return mass; } }
        [SerializeField] protected float mass;
        [SerializeField] protected ControlledObject parent;

        protected virtual void Start()
        {
            if(!parent) parent = transform.parent?.GetComponentInParent<ControlledObject>();
            var childs = GetComponentsInChildren<ControlledObject>();
            if (childs.Length < 2)
            {
                ReportMass(mass);
            }
        }

        public void ReportMass(float mass)
        {
            this.mass = mass;
            if (!parent) return;
            mass += parent.Mass;
            parent.ReportMass(mass);
        }

        public virtual void Connected(Rigidbody rb) { }
        public virtual void Disconnect() { }
    }
}
