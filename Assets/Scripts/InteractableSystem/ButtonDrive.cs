using System.Collections;
using UnityEngine;

namespace Assets.Scripts.InteractableSystem
{
    public class ButtonDrive : Interactable
    {
        public override float Signal 
        {
            get
            {
                if (buttonType == ButtonType.Switcher)
                {
                    if (outValue == minValue)
                    {
                        if (prevSignal == 0) { signal = 1; }
                        if (prevSignal == 1) { signal = 0; }
                    }
                    if (outValue == maxValue)
                    {
                        if (signal == 1) { prevSignal = 1; }
                        if (signal == 0) { prevSignal = 0; }
                    }
                }
                else if (buttonType == ButtonType.Trigger)
                {
                    if (outValue == minValue) { signal = 1; }
                    else { signal = 0; }
                }
                return signal;
            }
        }

        [Header("Тип кнопки:")]
        [SerializeField] private ButtonType buttonType;
        private Transform childTransform;
        private Vector3 lastButtonPosition;
        private float limitDelta = .005f;
        protected override void Start()
        {
            base.Start();
            childTransform = childCollider.transform; 
            axis = 0;
            maxValue = childTransform.localPosition.y;
        }

        float prevSignal = 0;

        /// <summary>
        /// Вычисление управляющего сигнала кнопки
        /// </summary>
        protected override void ComputeSignal() { }

        /// <summary>
        /// Обновление положения кнопки
        /// </summary>
        protected override void UpdateObjectTrasform()
        {
            var currentPos = childTransform.localPosition;
            currentPos[1] = outValue;
            childTransform.localPosition = currentPos;
        }

        /// <summary>
        /// Расчет текущего положения кнопки
        /// </summary>
        /// <param name="inputTransform">Входящая позиция управляющего объекта</param>
        protected override void ComputeOutValue(Vector3 inputTransform)
        {
            var delta = Vector3.Distance(inputTransform, lastButtonPosition);
            if(delta > .0f)
            {
                if(delta > limitDelta)
                {
                    var cross = Vector3.Cross(lastButtonPosition.normalized, inputTransform.normalized);
                    var dot = Vector3.Dot(Vector3.right, cross);
                    if (dot < 0) { delta = -delta; }
                    outValue = Mathf.Clamp(outValue + delta, minValue, maxValue);
                }
            }
            lastButtonPosition = inputTransform;
        }

        public override void InteractableBegin(Vector3 input)
        {
            base.InteractableBegin(input);
            lastButtonPosition = input;
        }

        public void ForceDisable()
        {

        }

        protected override IEnumerator ToStartState()
        {
            while ((maxValue + outValue) < -.01f)  
            {
                outValue = Mathf.Lerp(outValue, maxValue, 0.1f);
                Refresh();
                OnInteractableUpdate.Invoke();
                yield return null;
            }
            outValue = maxValue;
            OnInteractableUpdate.Invoke();
        }

        enum ButtonType
        {
            Trigger,
            Switcher,
        }
    }
}
