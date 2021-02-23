using UnityEngine;

namespace Assets.Scripts.DeviceScripts
{
    public class Motor : MovingDevice
    {
        [Header("Ось перемещения:")]
        [Tooltip("Ось перемещения, вдоль которой будет двигаться управляемый объект")]
        [SerializeField] private Axis axis = Axis.axis_X;
        [SerializeField] private bool reverse = false;

        [Header("Экстремум перемещения:")]
        [Tooltip("Ограничение максимальной и минимальной точек движения объекта вдоль выбранной оси:")]
        [SerializeField] private int extremum;

        private Vector3 currentVector = Vector3.zero;
        private float adjPoint0;
        private float adjPoint1;


        /// <summary>
        /// Расчет текущей позиции управляемого объекта
        /// </summary>
        public override void Work(float signal)
        {
                if (controlledObject)
                {
                    var deltaTime = !reverse ? Time.deltaTime : -Time.deltaTime;
                    controlledObject.position += currentVector * ComputeVelocity(signal) * deltaTime;
                    controlledObject.position = AdjPosition(controlledObject.position, axis, adjPoint0, adjPoint1);
                }
                else
                {
                    Debug.LogError("Не назначен управлеяемый объект");
                }
        }

        protected override void InitialSettings()
        {
            if (controlledObject)
            {
                currentVector[(int)axis] = 1.0f;
                adjPoint0 = controlledObject.position[(int)axis] - extremum;
                adjPoint1 = controlledObject.position[(int)axis] + extremum;
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
            if (!Application.isPlaying)
            {
                if (controlledObject && debug)
                {
                    InitialSettings();
                    Vector3 point0, point1;
                    point1 = point0 = controlledObject.position;
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
        }
    }
    public enum Axis
    {
        axis_X,
        axis_Y,
        axis_Z
    }
}
