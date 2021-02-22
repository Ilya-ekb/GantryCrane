using System.Collections;

using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.InputSystem
{
    public abstract class Interactable : MonoBehaviour
    {
        /// <summary>
        /// Управляющийся сигнал
        /// </summary>
        public virtual float Signal => signal;

        [SerializeField] protected Collider childCollider;

        [Header("Минимальное значение перемещения по оси:")]
        [SerializeField] protected float minValue = -35.0f;

        [Header("Максимальное значение перемещения по оси:")]
        [SerializeField] protected float maxValue = 35.0f;

        [Header("Текущее значение перемещения по оси:")]
        [SerializeField] protected float outValue;

        [Header("Событие при взаимодействии с объектом:")]
        public UnityEvent OnInteractableUpdate;
        public int Axis { get { return axis; } }

        protected int axis = 0;
        protected float signal;
        protected Coroutine startStateCor = null;

        protected virtual void Start()
        {
            if (!childCollider) { childCollider = GetComponentInChildren<Collider>(); }
        }

        /// <summary>
        /// Обновление состояния объекта
        /// </summary>
        protected virtual void Refresh()
        {
            UpdateObjectTrasform();
            ComputeSignal();
        }

        /// <summary>
        /// Обновление Transform объекта взаимодействия
        /// </summary>
        protected abstract void UpdateObjectTrasform();

        /// <summary>
        /// Вычисление управляющего
        /// </summary>
        protected abstract void ComputeSignal();

        /// <summary>
        /// Расчет текущего значения outValue объекта
        /// </summary>
        /// <param name="inputTransform"></param>
        protected abstract void ComputeOutValue(Vector3 inputTransform);

        /// <summary>
        /// Старт взаимодействия
        /// </summary>
        public virtual void InteractableBegin (Vector3 input)
        {
            if (startStateCor != null)
            {
                StopCoroutine(startStateCor);
                startStateCor = null;
            }
        }

        /// <summary>
        /// Обновление при взаимодействии
        /// </summary>
        public virtual void InteractableUpdate(Vector3 input) 
        {
            OnInteractableUpdate?.Invoke();
            ComputeOutValue(input);
            Refresh();
        }

        /// <summary>
        /// Окончание взаимодействий
        /// </summary>
        public virtual void InteractableEnd()
        {
            startStateCor = StartCoroutine(ToStartState());
        }
        public virtual void OnFocusBegin() { }

        /// <summary>
        /// Возврат на стартовую позицию
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerator ToStartState();
    }
}
