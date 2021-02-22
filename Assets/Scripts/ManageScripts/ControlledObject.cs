
using System;
using System.Collections;

using UnityEngine;

namespace Assets.Scripts.ManageScripts
{
    public class ControlledObject : MonoBehaviour
    {
        public float Mass { get { return mass; } }
        public virtual Vector3 position { get => transform.position; set => transform.position = value; }
        public virtual Transform Parent
        {
            set
            {
                transform.parent = value;
                ReportMass(mass);
            }
        }

        [SerializeField] protected float mass;
        [SerializeField] private ControlledObject parent;

        protected virtual void Start()
        {
            ReportMass(mass);
        }

        private void ReportMass(float mass)
        {
            this.mass = mass;
            if (!parent) parent = transform.parent?.GetComponentInParent<ControlledObject>();
            if (!parent) return;
            var childs = GetComponentsInChildren<ControlledObject>();
            if (childs.Length < 2)
            {

                mass += parent.mass;
                parent.ReportMass(mass);
            }
        }

        public virtual void Connected(Transform parent) { Parent = parent; }
        public virtual void Disconnected() { transform.parent = null; }
    }
}
