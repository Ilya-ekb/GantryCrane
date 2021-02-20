using System;

using UnityEngine;

namespace Assets.Scripts.MotorScripts
{
    public class Motor : MonoBehaviour
    {
        [Header("Управлеяемый с мотором объект:")]
        [Tooltip("Ссылка на Transform:")]
        [SerializeField] private Transform controlingObject;

        [Tooltip("Масса объекта")]
        [SerializeField] private float massControlingObject;

        [Tooltip("Инерция(для расчета замедления)")]
        [SerializeField] private float inertia;

        [Header("Ось перемещения:")]
        [Tooltip("Ось перемещения, вдоль которой будет двигаться управляемый объект")]
        [SerializeField] private Axis axis = Axis.axis_X;

        [Header("Маскимальная скорость:")]
        [Tooltip("Маскимальная скорость управляемого объекта")]
        [SerializeField] private float maxSpeed;

        [Range(-1, 1)]
        [Header("Управляющий сигнал:")]
        [Tooltip("Приходящий управляющий сигнал на мотор, изменяется от -1 до 1, где 0 - состояние покоя")]
        [SerializeField] private float signal = 0f;

        [Header("Экстремум перемещения:")]
        [Tooltip("Ограничение максимальной и минимальной точек движения объекта вдоль выбранной оси:")]
        [SerializeField] private int extremum;

        [Tooltip("Включить для дебага экстремумов")]
        [SerializeField] private bool debug;

        private Vector3 currentVector = Vector3.zero;
        private float prevSignal;
        private float adjPoint0;
        private float adjPoint1;


        /// <summary>
        /// Расчет текущей позиции управляемого объекта
        /// </summary>
        public void ComputePosition()
        {
            if (controlingObject)
            {
                controlingObject.position += currentVector * ComputeVelocity(signal) * Time.deltaTime;
                controlingObject.position = AdjPosition(controlingObject.position, axis, adjPoint0, adjPoint1);
            }
            else
            {
                Debug.LogError("Не назначен управлеяемый объект");
            }
        }

        /// <summary>
        /// Стартовые настройки
        /// </summary>
        private void InitialSettings()
        {
            if (controlingObject)
            {
                currentVector[(int)axis] = 1.0f;
                adjPoint0 = controlingObject.position[(int)axis] - extremum;
                adjPoint1 = controlingObject.position[(int)axis] + extremum;
            }
        }

        private Vector3 AdjPosition(Vector3 position, Axis axis, float min, float max)
        {
            position = controlingObject.position;
            position[(int)axis] = Mathf.Clamp(position[(int)axis], min, max);
            return position;
        }

        private void OnDrawGizmos()
        {                                                          
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                if (controlingObject && debug)
                {
                    InitialSettings();
                    Vector3 point0, point1;
                    point1 = point0 = controlingObject.position;
                    if (axis == Axis.axis_X)
                    {
                        Gizmos.color = Color.red;
                    }
                    if (axis == Axis.axis_Y)
                    {
                        Gizmos.color = Color.green;
                    }
                    if (axis == Axis.axis_Z)
                    {
                        Gizmos.color = Color.blue;
                    }
                    point0[(int)axis] = adjPoint0;
                    point1[(int)axis] = adjPoint1;
                    Gizmos.DrawLine(point0, point1);
                    Gizmos.DrawSphere(point0, 1f);
                    Gizmos.DrawSphere(point1, 1f);

                }
            }
#endif
        }

        /// <summary>
        /// Расчет текущей скорости
        /// </summary>
        /// <param name="signal"></param>Управляющий сигнал: изменяется от -1 до 1
        /// <returns></returns>
        private float ComputeVelocity(float signal)
        {
            var currentSignal = Mathf.Abs(signal);
            currentSignal = Mathf.Lerp(prevSignal, currentSignal, massControlingObject / inertia);
            float velocity = Mathf.Lerp(maxSpeed, 0, 1 - currentSignal);
            prevSignal = currentSignal;
            velocity = signal > 0 ? velocity : -velocity; 
            return velocity;
        }

        private void Start()
        {
            InitialSettings();
        }
#if UNITY_EDITOR
        private void Update()
        {
            ComputePosition();  
        }
#endif
    }

    public enum Axis
    {
        axis_X,
        axis_Y,
        axis_Z
    }
}
