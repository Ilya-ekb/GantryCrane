using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.InputSystem
{
    public class ControllerDrive : Interactable
    {
        public float OutAngle { get { return outValue; } }

        [Header("Ось вращения контроллера:")]
        [SerializeField] private Axis_Rot axisRotation = Axis_Rot.Axis_X;

        private Quaternion startRotation;

        private Vector3 worldPlane = Vector3.zero;

        private Vector3 lastProjectionVector;

        //Чувствительность контроллера
        private float limitDeltaAngle = 0.1f;

        //Увеличить это значение если нужно, чтобы контроллер немного фиксировался на крайних значения
        private float minMaxAngularThreshold = 0.1f;

        protected override void Start()
        {
            base.Start();
            axis = (int)axisRotation;
            worldPlane[axis] = 1.0f;

            if (transform.parent) { worldPlane = transform.parent.localToWorldMatrix.MultiplyVector(worldPlane).normalized; }

            startRotation = Quaternion.identity;
            outValue = transform.localEulerAngles[axis];
            outValue = Mathf.Clamp(outValue, minValue, maxValue);

            Refresh();
        }

        /// <summary>
        /// Вычисление управляющего сигнал контроллера
        /// </summary>
        protected override void ComputeSignal()
        {
            signal = (outValue - minValue) / (maxValue - minValue);
        }

        /// <summary>
        /// Установка угла объекта текущего угла управляемого объекта
        /// </summary>
        protected override void UpdateObjectTrasform()
        {
            transform.localRotation = startRotation * Quaternion.AngleAxis(outValue, worldPlane);
        }

        public override void InteractableBegin(Vector3 input)
        {
            base.InteractableBegin(input);
            lastProjectionVector = ComputeProjection(input);
        }

        /// <summary>
        /// Расчет текущего угла контроллера
        /// </summary>
        /// <param name="inputTransform">Входящая позиция управляющего объекта</param>
        /// <returns></returns>
        protected override void ComputeOutValue(Vector3 inputTransform)
        {
            var inputProjectionVector = ComputeProjection(inputTransform);
            if (!inputProjectionVector.Equals(lastProjectionVector))
            {
                var deltaAngle = Vector3.Angle(inputProjectionVector, lastProjectionVector);

                if (deltaAngle > .0f)
                {
                    if (deltaAngle > limitDeltaAngle)
                    {
                        //Определение направления изменения угла
                        var cross = Vector3.Cross(lastProjectionVector, inputProjectionVector);
                        var dot = Vector3.Dot(worldPlane, cross);
                        var curDeltaAngle = deltaAngle;
                        if (dot < 0) { curDeltaAngle = -curDeltaAngle; }


                        var curAngle = Mathf.Clamp(outValue + curDeltaAngle, minValue, maxValue);
                        if (outValue == minValue)
                        {
                            if (curAngle > minValue && deltaAngle > minMaxAngularThreshold)
                            {
                                outValue = curAngle;
                                lastProjectionVector = inputProjectionVector;
                            }
                        }
                        else if (outValue == maxValue)
                        {
                            if (curAngle < maxValue && deltaAngle > minMaxAngularThreshold)
                            {
                                outValue = curAngle;
                                lastProjectionVector = inputProjectionVector;
                            }
                        }
                        else if (curAngle == minValue || curAngle == maxValue)
                        {
                            outValue = curAngle;
                            lastProjectionVector = inputProjectionVector;
                        }
                        else
                        {
                            outValue = curAngle;
                            lastProjectionVector = inputProjectionVector;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Расчет проекции вектора объекта относительно центра контороллера на выбранную ось вращения
        /// </summary>
        /// <param name="inTrans"></param>Положение объекта для вектора которого узнаем проектцию
        /// <returns></returns>
        private Vector3 ComputeProjection(Vector3 inTrans)
        {
            var directionVector = (inTrans - transform.position).normalized;
            var inputProjection = Vector3.zero;

            //Если поворачивающая точка не в находиться не в центре позиции контроллера, то можно расчитать проекцию
            if(directionVector.sqrMagnitude > .0f)
            {
                inputProjection = Vector3.ProjectOnPlane(directionVector, worldPlane).normalized;
            }
            return inputProjection;
        }

        protected override IEnumerator ToStartState()
        {
            while(Mathf.Abs(outValue) > 0.02f)
            {
                outValue = Mathf.Lerp(outValue, 0, 0.1f);
                Refresh();
                OnInteractableUpdate.Invoke();
                yield return null;
            }
            outValue = .0f;
            signal = .5f;
            OnInteractableUpdate.Invoke();
        }
    }

    enum Axis_Rot
    {
        Axis_X,
        Axis_Y,
        Axis_Z
    }
}
