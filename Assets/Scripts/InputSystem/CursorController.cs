using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.InputSystem
{
    public class CursorController : MonoBehaviour
    {
        private SphereCollider sphere;
        private Collider[] colliders;
        [SerializeField] private LayerMask interactableMask;
        private Interactable attachedObject = null;

        private void Start()
        {
            sphere = GetComponent<SphereCollider>();
            if (!sphere)
            {
                sphere = gameObject.AddComponent<SphereCollider>();
                sphere.radius = .5f; 
            }
            transform.position = Camera.main.transform.position;

        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                OnAttach();
            }
            if (Input.GetMouseButtonUp(0))
            {
                OnDetach();
            }
        }

        private void OnverlapBegin()
        {

        }

        /// <summary>
        /// Начало взаимодействия с предметом
        /// </summary>
        private void OnAttach()
        {
            if (!attachedObject)
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit, 100))
                {
                    transform.position = raycastHit.point;
                    colliders = Physics.OverlapSphere(sphere.transform.position, sphere.radius, interactableMask);
                    if (colliders != null)
                    {
                        var col = Nearest(transform.position, colliders);
                        attachedObject = col.GetComponentInParent<Interactable>();
                        attachedObject?.InteractableBegin(Camera.main.ScreenPointToRay(Input.mousePosition).GetPoint(3));
                    }
                }
            }
        }

        /// <summary>
        /// Завершение взаимодействия с предметом
        /// </summary>
        private void OnDetach()
        {
            if (attachedObject)
            {
                attachedObject.InteractableEnd();
                attachedObject = null;
            }
            transform.position = Camera.main.transform.position;
        }

        private void Idle()
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit, 100, interactableMask))
            {

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
        int little_crunch(int x) => x == 0 ? 2 : x == 1 ? 0 : 1;
    }

}
