using System;

using UnityEngine;

namespace Assets.Scripts.DeviceScripts
{
    public class Motor : Device
    {
        [Header("Управлеяемый с мотором объект:")]
        [Tooltip("Ссылка на Transform:")]
        [SerializeField] private Transform controlingObject;

        [Tooltip("Условная мощность мотора")]
        [SerializeField] private float motorPower;

        [Tooltip("Условная масса объекта")]
        [SerializeField] private float mass;

        [Header("Ось перемещения:")]
        [Tooltip("Ось перемещения, вдоль которой будет двигаться управляемый объект")]
        [SerializeField] private Axis axis = Axis.axis_X;

        [Header("Маскимальная скорость:")]
        [Tooltip("Маскимальная скорость управляемого объекта")]
        [SerializeField] private float maxSpeed;

        //[Range(.0f, 1.0f)]
        //[Header("Управляющий сигнал:")]
        //[Tooltip("Приходящий управляющий сигнал на мотор, изменяется от -1 до 1, где 0 - состояние покоя")]
        //[SerializeField] private float signal = .5f;

        [Header("Экстремум перемещения:")]
        [Tooltip("Ограничение максимальной и минимальной точек движения объекта вдоль выбранной оси:")]
        [SerializeField] private int extremum;

        [Tooltip("Включить для дебага экстремумов")]
        [SerializeField] private bool debug;

        private Vector3 currentVector = Vector3.zero;
        private float prevSignal;
        private float adjPoint0;
        private float adjPoint1;
        private const float deaccel = 10000.0f;


        /// <summary>
        /// Расчет текущей позиции управляемого объекта
        /// </summary>
        public override void Work(float signal)
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
        protected override void InitialSettings()
        {
            if (controlingObject)
            {
                currentVector[(int)axis] = 1.0f;
                adjPoint0 = controlingObject.position[(int)axis] - extremum;
                adjPoint1 = controlingObject.position[(int)axis] + extremum;
            }
        }

        /// <summary>
        /// Корректировка вектора по выбранной оси относительно min и max значений
        /// </summary>
        /// <param name="position">Вектор относительно, которого возвращается скорректированный по выбранной оси результирующий вектор</param>
        /// <param name="axis">Ось корректировки</param>
        /// <param name="min">Максимальное значение</param>
        /// <param name="max">Минимальное значение</param>
        /// <returns>Вектор скорректированный в пределах min и max значений по выбранной оси относительно входного вектора</returns>
        private Vector3 AdjPosition(Vector3 position, Axis axis, float min, float max)
        {
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
        /// <param name="signal">Управляющий сигнал: изменяется от 0 до 1, где 0.5 равно нулевой скорости, 0 и 1 - maxSpeed</param>
        /// <returns>Скорость от 0 до maxSpeed интерполированную в зависимости от сигнала</returns>
        private float ComputeVelocity(float signal)
        {
            signal = Mathf.Lerp(prevSignal, signal, (motorPower / deaccel) / mass);
            float velocity = Mathf.Lerp(0, maxSpeed, 4 * ((signal - .5f) * (signal - .5f)));
            prevSignal = signal;
            velocity = signal > .5f ? velocity : signal < .5f ? -velocity : 0;
            return velocity;
        }
    }
    public enum Axis
    {
        axis_X,
        axis_Y,
        axis_Z
    }
}
