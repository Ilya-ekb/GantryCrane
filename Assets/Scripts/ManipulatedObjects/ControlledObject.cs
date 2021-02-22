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
                ReportMass();
            }
        }

        [SerializeField] protected float mass;
        [SerializeField] private ControlledObject parent;
        [SerializeField] private bool end;
        private float startMass;

        private void Awake()
        {
            startMass = mass;
        }

        protected virtual void Start()
        {
            if (end) ReportMass();
        }

        /// <summary>
        /// Передача массы сочленнным объектам
        /// </summary>
        /// <param name="mass"></param>
        private void ReportMass(float mass = .0f)
        {
            this.mass = startMass +  mass;
            if (!parent) parent = transform.parent?.GetComponentInParent<ControlledObject>();
            if (!parent) return;
            {
                parent.ReportMass(this.mass);
            }
        }

        /// <summary>
        /// Присоедиение массы объекта к общему сочленению
        /// </summary>
        /// <param name="parent"></param>
        public virtual void Connected(Transform parent) { Parent = parent; }
        
        /// <summary>
        /// Отсоединение от общего сочленения
        /// </summary>
        public virtual void Disconnected() 
        {
            transform.parent = null;
            parent.ReportMass();
            parent = null;
        }
    }
}
