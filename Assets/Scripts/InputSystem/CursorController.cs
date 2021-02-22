using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.InputSystem
{
    public class CursorController : MonoBehaviour, ISystemInput
    {
        private Collider[] colliders;
        [SerializeField] private LayerMask interactableMask;
        private Interactable attachedObject = null;
        private float distance;

        private void Start()
        {
            transform.position = Camera.main.transform.position;
        }

        /// <summary>
        /// Начало взаимодействия с предметом
        /// </summary>
        public void OnAttach()
        {
            if (!attachedObject)
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit, 100))
                {
                    transform.position = raycastHit.point;
                    distance = raycastHit.distance;
                    colliders = Physics.OverlapSphere(transform.position, transform.localScale.x/2, interactableMask);
                    if (colliders != null)
                    {
                        var col = Nearest(transform.position, colliders);
                        attachedObject = col?.GetComponentInParent<Interactable>();
                        attachedObject?.InteractableBegin(Camera.main.ScreenPointToRay(Input.mousePosition).GetPoint(distance));
                    }
                }
            }
            else
            {
                var ind = little_crunch(attachedObject.Axis);
                var attachedPos = transform.position;
                attachedPos[ind] = Camera.main.ScreenPointToRay(Input.mousePosition).GetPoint(distance)[ind];
                attachedObject.InteractableUpdate(attachedPos);
            }
        }

        /// <summary>
        /// Завершение взаимодействия с предметом
        /// </summary>
        public void OnDetach()
        {
            attachedObject?.InteractableEnd();
            attachedObject = null;
            transform.position = Camera.main.transform.position;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, transform.localScale.x/2);
        }

        /// <summary>
        /// Получение ближайшего коллайдера
        /// </summary>
        /// <param name="point"></param>Точка, относительно которой считается растояние 
        /// <param name="colliders"></param>Список коллайдеров
        /// <returns></returns>
        private Collider Nearest(Vector3 point, Collider[] colliders)
        {
            if (!colliders[0]) return null;
            var nearestColl = colliders[0];
            var minDistance = Vector3.Distance(point, nearestColl.transform.position);
            foreach (var collider in colliders)
            {
                var curDisance = Vector3.Distance(point, collider.transform.position);
                if (minDistance > curDisance) { nearestColl = collider; }
            }
            return nearestColl;
        }

        //who, without sin, let him be the first to throw a stone at me)
        int little_crunch(int x) => x == 0 ? 2 : 0;
    }

    public interface ISystemInput
    {
        void OnAttach();
        void OnDetach();
    }

}
