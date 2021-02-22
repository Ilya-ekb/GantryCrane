using Assets.Scripts.ManageScripts;
using UnityEngine;

namespace Assets.Scripts.DeviceScripts
{
    public abstract class MovingDevice : Device
    {
        [Header("Управлеяемые устройством объекты:")]
        [Tooltip("Ссылка на Transform:")]
        [SerializeField] protected ControlledObject controlledObject;

        [Tooltip("Условная мощность устройства")]
        [SerializeField] protected float devicePower;

        [Header("Маскимальная скорость:")]
        [Tooltip("Маскимальная скорость управляемого объекта")]
        [SerializeField] protected float maxSpeed;
        [SerializeField] protected bool debug = false;

        protected float prevSignal;
        private const float deaccel = 10000.0f;


        /// <summary>
        /// Расчет текущей скорости управляемого объекта
        /// </summary>
        /// <param name="signal">Управляющий сигнал: изменяется от 0 до 1, где 0.5 равно нулевой скорости, 0 и 1 - maxSpeed</param>
        /// <returns>Скорость от 0 до maxSpeed интерполированную в зависимости от сигнала</returns>
        protected virtual float ComputeVelocity(float signal)
        {
            signal = prevSignal < .5f ? Mathf.Clamp(signal, .0f, .5f) : prevSignal > .5f ? Mathf.Clamp(signal, .5f, 1.0f) : signal;
            var curSign = Mathf.Lerp(prevSignal, signal, (devicePower / deaccel) / controlledObject.Mass);
            float velocity = Mathf.Lerp(0, maxSpeed, 4 * ((curSign - .5f) * (curSign - .5f)));
            prevSignal = (signal == .5f) ? .5f : curSign;
            velocity = signal > .5f ? velocity : signal < .5f ? -velocity : velocity;
            return velocity;
        }
    }
}
