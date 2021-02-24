using UnityEngine;

namespace Assets.Scripts.DeviceScripts
{
    public abstract class Device  : MonoBehaviour
    {
        protected virtual void Start()
        {
            InitialSettings();
        }

        /// <summary>
        /// Стартовые настройки
        /// </summary>
        protected abstract void InitialSettings();

        /// <summary>
        /// Работа устройства в зависимости от подаваемого сигнала
        /// </summary>
        /// <param name="signal">Подаваемый сигнал от 0 до 1</param>
        public abstract void Work(float signal);
    }
}
