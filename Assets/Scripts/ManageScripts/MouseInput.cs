using Assets.Scripts.InteractableSystem;
using Assets.Scripts.ManageScripts;
using UnityEngine;

namespace Assets.Scripts.ManageScripts
{
    public class MouseInput : SystemInput
    {
        private Collider[] colliders;
        [SerializeField] private LayerMask interactableMask;
        [SerializeField] private float interactableRadius;
        [SerializeField] private bool debug;
        private float distance;

        public override void OnAttach()
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit, 100))
            {
                distance = raycastHit.distance;
                var interactablePoint = Camera.main.ScreenPointToRay(Input.mousePosition).GetPoint(distance);
                colliders = Physics.OverlapSphere(raycastHit.point, interactableRadius, interactableMask);
                if (colliders != null)
                {
                    var col = Nearest(transform.position, colliders);
                    attachedObject = col?.GetComponentInParent<Interactable>();
                    attachedObject?.InteractableBegin(interactablePoint);
                }
            }
        }

        public override void OnAttachedUpdate() => attachedObject?.InteractableUpdate(Camera.main.ScreenPointToRay(Input.mousePosition).GetPoint(distance));

        public override void OnDetach()
        {
            attachedObject?.InteractableEnd();
            attachedObject = null;
            distance = 0;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnAttach();
            }
            if (Input.GetMouseButton(0))
            {
                OnAttachedUpdate();
            }
            if (Input.GetMouseButtonUp(0))
            {
                OnDetach();
            }
        }

        private void OnDrawGizmos()
        { 
            if(debug)
            {
                Gizmos.color = Color.green;
                var point = !Application.isPlaying ? Camera.main.transform.position : Camera.main.ScreenPointToRay(Input.mousePosition).GetPoint(distance);
                Gizmos.DrawWireSphere(Camera.main.ScreenPointToRay(Input.mousePosition).GetPoint(distance), interactableRadius);
            }
        }

        /// <summary>
        /// Получение ближайшего коллайдера
        /// </summary>
        /// <param name="point"></param>Точка, относительно которой считается растояние 
        /// <param name="colliders"></param>Список коллайдеров
        /// <returns></returns>
        private Collider Nearest(Vector3 point, Collider[] colliders)
        {
            if (colliders.Length == 0) return null;
            var nearestColl = colliders[0];
            var minDistance = Vector3.Distance(point, nearestColl.transform.position);
            foreach (var collider in colliders)
            {
                var curDisance = Vector3.Distance(point, collider.transform.position);
                if (minDistance > curDisance) { nearestColl = collider; }
            }
            return nearestColl;
        }
    }
}
