using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorController : MonoBehaviour
{
    [Header("Управляемые Joints:")]
    [SerializeField] private HingeJoint[] joints;

    [Range(100, 2000)]
    [Header("Крутящий момент:")]
    [SerializeField] private float force = 1000.0f;

    [Header("Максимальная скорость:")]
    [SerializeField] private float maxSpeed = 500f;

    /// <summary>
    /// Управление работой моторов
    /// </summary>
    /// <param name="singnal"></param>Управляющий сигнал от 0 до 1 или -1, где 0 - нулевая скорость, 1 или -1 - максимальная скорость 
    public void Working(float singnal)
    {
        foreach(var joint in joints)
        {
            if (!joint.useMotor) joint.useMotor = true;
            var motor = joint.motor;
            motor.force = force;
            motor.targetVelocity = maxSpeed * singnal;
            joint.motor = motor;
        }
    }
}
