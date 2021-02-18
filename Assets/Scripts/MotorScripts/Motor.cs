using System.Timers;
using UnityEngine;
namespace Assets.Scripts.MotorScripts
{
    public class Motor : MonoBehaviour
    {
        [SerializeField] private Transform controlingObject;
        [SerializeField] private Axis axis = Axis.axis_X;
        [SerializeField] private float acceleration;
        [SerializeField] private float movementAmplitude;
        [SerializeField] private Color color;
        [Range(0, 1)]
        [SerializeField] private float value = 0.5f;
        private float minExtremum = 0;
        private float maxExtremum = 0;


        protected Vector3 ComputePosition(Vector3 position, float acc, float speed)
        {
            if(axis == Axis.axis_X)
            {
                ComputePoints(speed, position.x, out float startPoint, out float endPoint);
                var newPosX = Mathf.Clamp(Mathf.Lerp(startPoint, endPoint, acc), minExtremum, maxExtremum);
                return Vector3.Lerp(position, new Vector3(newPosX, position.y, position.z), speed);
            }
            if (axis == Axis.axis_Y)
            {
                ComputePoints(speed, position.y, out float startPoint, out float endPoint);
                var newPosY = Mathf.Clamp(Mathf.SmoothStep(startPoint, endPoint, acc), minExtremum, maxExtremum);
                return Vector3.Lerp(position, new Vector3(position.x, newPosY, position.z), speed);
            }
            if (axis == Axis.axis_Z)
            {
                ComputePoints(speed, position.z, out float startPoint, out float endPoint);
                var newPosZ = Mathf.Clamp(Mathf.SmoothStep(startPoint, endPoint, acc), minExtremum, maxExtremum);
                return Vector3.Lerp(position, new Vector3(position.x, position.y, newPosZ), speed);
            }
            return Vector3.zero;
        }

        private void Start()
        {
            if (axis == Axis.axis_X)
            {
                ComputePoints(movementAmplitude, controlingObject.position.x, out minExtremum, out maxExtremum);
                return;
            }
            if (axis == Axis.axis_Y)
            {
                ComputePoints(movementAmplitude, controlingObject.position.y, out minExtremum, out maxExtremum);
                return;
            }
            if (axis == Axis.axis_Z)
            {
                ComputePoints(movementAmplitude, controlingObject.position.z, out minExtremum, out maxExtremum);
                return;
            }
        }
        private void Update()
        {
            controlingObject.position = ComputePosition(controlingObject.position, value, acceleration);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = color;
            //Gizmos.DrawSphere(startPoint, 1f);
            //Gizmos.DrawSphere(endPoint, 1f);
        }

        private void ComputePoints(float amplitude, float inputValue, out float minLimit, out float maxLimit)
        {
            minLimit = inputValue - amplitude;
            maxLimit = inputValue + amplitude;
        }
    }

    public enum Axis
    {
        axis_X,
        axis_Y,
        axis_Z
    }
}

