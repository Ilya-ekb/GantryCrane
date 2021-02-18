using UnityEngine;

namespace Assets.Scripts.MotorScripts
{

    public class MotorController : MonoBehaviour
    {
        [Header("Управляемые Joints:")]
        [SerializeField] private Motor[] motors;

        [Header("Максимальная скорость:")]
        [SerializeField] private float maxSpeed = 500f;

        /// <summary>
        /// Управление работой моторов
        /// </summary>
        /// <param name="singnal"></param>Управляющий сигнал от 0 до 1 или -1, где 0 - нулевая скорость, 1 или -1 - максимальная скорость 
        public void Working(float singnal)
        {
            foreach(var motor in motors)
            {
            }
        }
    }
}
