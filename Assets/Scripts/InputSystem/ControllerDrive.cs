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
        public float OutAngle { get { return outAngle; } }
        public float Signal { get { return signal; } }

        [Header("Ось вращения контроллера:")]
        [SerializeField] private Axis_Rot axisRotation = Axis_Rot.Axis_X;

        [SerializeField] private Collider rotationCollider;

        [SerializeField] private float minAngle = -35.0f;
        [SerializeField] private float maxAngle = 35.0f;
        [SerializeField] private float outAngle;
        [SerializeField] private float signal;

        [Header("Событие при обновлении угла контроллера:")]
        public UnityEvent OnAngleUpdate;

        private Quaternion startRotation;

        private Vector3 worldPlane = Vector3.zero;
        private Vector3 localPlane = Vector3.zero;

        private Vector3 lastProjectionVector;

        private Coroutine StartRotationCor = null;
        private float limitDeltaAngle = 0.1f;
        private float minMaxAngularThreshold = 0.1f;

        private void Start()
        {
            axis = (int)axisRotation;
            if (!rotationCollider) { rotationCollider = GetComponentInChildren<Collider>(); }
            worldPlane[axis] = 1.0f;
            localPlane = worldPlane;

            if (transform.parent) { worldPlane = transform.parent.localToWorldMatrix.MultiplyVector(worldPlane).normalized; }

            startRotation = Quaternion.identity;
            outAngle = transform.localEulerAngles[axis];
            outAngle = Mathf.Clamp(outAngle, minAngle, maxAngle);

            Refresh();
        }

        /// <summary>
        /// Обновление контроллера
        /// </summary>
        public void Refresh()
        {
            RotateObject();
            ComputeSignal();
        }

        private void ComputeSignal()
        {
            signal = (outAngle - minAngle) / (maxAngle - minAngle);
        }

        /// <summary>
        /// Установка угла объекта текущего угла управляемого объекта
        /// </summary>
        private void RotateObject()
        {
            transform.localRotation = startRotation * Quaternion.AngleAxis(outAngle, worldPlane);
            OnAngleUpdate?.Invoke();
        }

        /// <summary>
        /// Старт взаимодействия
        /// </summary>
        public override void InteractableBegin(Vector3 input)
        {
            if(StartRotationCor != null)
            {
                StopCoroutine(StartRotationCor);
                StartRotationCor = null;
            }
            lastProjectionVector = ComputeProjection(input);
        }

        /// <summary>
        /// Обновление при взаимодействии
        /// </summary>
        public override void InteractableUpdate(Vector3 input)
        {
            ComputeOutAngle(input);
            Refresh();
        }

        /// <summary>
        /// Окончание взаимодействий
        /// </summary>
        public override void InteractableEnd()
        {
            StartRotationCor = StartCoroutine(ToStartRotation());
        }

        /// <summary>
        /// Расчет угла между 
        /// </summary>
        /// <param name="inputTransform"></param>
        /// <returns></returns>
        private void ComputeOutAngle(Vector3 inputTransform)
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


                        var curAngle = Mathf.Clamp(outAngle + curDeltaAngle, minAngle, maxAngle);
                        if (outAngle == minAngle)
                        {
                            if (curAngle > minAngle && deltaAngle > minMaxAngularThreshold)
                            {
                                outAngle = curAngle;
                                lastProjectionVector = inputProjectionVector;
                            }
                        }
                        else if (outAngle == maxAngle)
                        {
                            if (curAngle < maxAngle && deltaAngle > minMaxAngularThreshold)
                            {
                                outAngle = curAngle;
                                lastProjectionVector = inputProjectionVector;
                            }
                        }
                        else if (curAngle == minAngle || curAngle == maxAngle)
                        {
                            outAngle = curAngle;
                            lastProjectionVector = inputProjectionVector;
                        }
                        else
                        {
                            outAngle = curAngle;
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
                Debug.Log(inputProjection);
            }
            return inputProjection;
        }

        private IEnumerator ToStartRotation()
        {
            while(Mathf.Abs(outAngle) > 0.01f)
            {
                outAngle = Mathf.Lerp(outAngle, 0, 0.1f);
                Refresh();
                yield return null;
            }
            outAngle = .0f;
            signal = .5f;
        }
    }

    public enum Axis_Rot
    {
        Axis_X,
        Axis_Y,
        Axis_Z
    }
}
