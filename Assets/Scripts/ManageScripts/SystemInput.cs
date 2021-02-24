using Assets.Scripts.InteractableSystem;
using UnityEngine;

namespace Assets.Scripts.ManageScripts
{
    public abstract class SystemInput : MonoBehaviour
    {
        protected Interactable attachedObject;
        [SerializeField] protected LayerMask interactableMask;

        /// <summary>
        /// Начало взаимодействия с предметом
        /// </summary>
        public abstract void OnAttach();

        /// <summary>
        /// Взаимодействие с предметом
        /// </summary>
        public abstract void OnAttachedUpdate();

        /// <summary>
        /// Завершение взаимодействия с предметом
        /// </summary>
        public abstract void OnDetach();

    }
}
