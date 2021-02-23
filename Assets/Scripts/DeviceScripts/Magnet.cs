using Assets.Scripts.ManageScripts;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.DeviceScripts
{
    public class Magnet : MovingDevice
    {
        [Header("Радиус действия магнита")]
        [SerializeField] private float workRadius = 0;

        [SerializeField] private LayerMask magnetMask;
        
        private Rigidbody rb;
        private Transform childObject;
        private Collider[] connectColliders;
        private Coroutine magnetCor;

        protected override void InitialSettings()
        {
            rb = GetComponent<Rigidbody>();
            if (!rb) { rb = gameObject.AddComponent<Rigidbody>(); }
            childObject = transform.GetChild(0);
            if (!childObject) { Debug.LogError($"Добавьте дочерний объект для {gameObject.name}"); }
        }

        bool connected = false;

        /// <summary>
        /// Включение\Выключение магнита
        /// </summary>
        /// <param name="signal">1 - включить, 0 - выключить</param>
        public override void Work(float signal)
        {
            if (signal == 1)
            {
                if(magnetCor == null)
                {
                    magnetCor = StartCoroutine(Magneting());
                }
            }
            else
            {
                if(magnetCor!= null)
                {
                    StopCoroutine(magnetCor);
                    magnetCor = null;
                }
                if (connectColliders != null)
                {
                    foreach (var collider in connectColliders)
                    {
                        controlledObject = collider.GetComponent<MagnetedObject>();
                        if (controlledObject)
                        {
                            connected = false;
                            controlledObject.Disconnected();
                            controlledObject = null;
                        }
                    }
                    connectColliders = null;
                }
            }
        }

        //Процесс примагничивания
        private IEnumerator Magneting()
        {
            while (!connected)
            {
                var magneticFieldPos = transform.position;
                magneticFieldPos[1] = transform.position.y - workRadius;
                var colliders = Physics.OverlapSphere(magneticFieldPos, workRadius, magnetMask);
                foreach (var collider in colliders)
                {
                    controlledObject = collider.gameObject.GetComponent<MagnetedObject>();
                    var currentVector = (transform.position - controlledObject.position);
                    controlledObject.position = currentVector * ComputeVelocity(1.0f) * Time.deltaTime;
                }
                connectColliders = Physics.OverlapBox(childObject.position, childObject.localScale, childObject.rotation, magnetMask);
                if (connectColliders.Length > 0)
                {
                    connected = true;
                    foreach (var coll in connectColliders)
                    {
                        controlledObject = coll.gameObject.GetComponent<MagnetedObject>();
                        if (controlledObject)
                        {
                            controlledObject.Connected(transform);
                        }
                    }
                    magnetCor = null;
                }
                yield return null;
            }
        }

        private void OnDrawGizmos()
        {
            if (debug)
            {
                if(!childObject) { childObject = transform.GetChild(0); }
                Gizmos.color = Color.yellow;
                var magneticFieldPos = transform.position;
                magneticFieldPos[1] = transform.position.y - workRadius;
                Gizmos.DrawWireSphere(magneticFieldPos, workRadius);
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(childObject.position, childObject.localScale * 2);
            }
        }
    }
}
