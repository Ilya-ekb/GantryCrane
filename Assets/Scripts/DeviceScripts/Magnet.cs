

using Assets.Scripts.ManageScripts;

using UnityEngine;

namespace Assets.Scripts.DeviceScripts
{
    public class Magnet : Device
    {
        [SerializeField] private float workRadius = 0;
        [SerializeField] private LayerMask magnetMask;
        private Rigidbody rb;
        private Collider[] connectColliders;

        protected override void InitialSettings()
        {
            rb = GetComponent<Rigidbody>();
            if (!rb) { rb = gameObject.AddComponent<Rigidbody>(); }
        }
        bool connected = false;
        public override void Work(float signal)
        {
            if (signal == 1)
            {
                var magneticFieldPos = transform.position;
                magneticFieldPos[1] = transform.position.y - workRadius;
                if (!connected)
                {
                    var colliders = Physics.OverlapSphere(magneticFieldPos, workRadius, magnetMask);
                    foreach (var collider in colliders)
                    {
                        controlledObject = collider.gameObject.GetComponent<MagnetedObject>();
                        var currentVector = (transform.position - controlledObject.position);
                        controlledObject.position = currentVector * ComputeVelocity(signal) * Time.deltaTime;
                    }
                }
                connectColliders = Physics.OverlapBox(transform.position, transform.localScale, transform.rotation, magnetMask);
                     if (connectColliders.Length > 0 && !connected)
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
                }
            }
            else
            {
                if(connectColliders!= null)
                {
                    foreach (var collider in connectColliders)
                    {
                        controlledObject = collider.GetComponent<MagnetedObject>();
                        if (controlledObject) { controlledObject.Disconnected(); connected = false; }
                    }
                }
            }
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Work(1.0f);
            }
            else
            {
                Work(.0f);
            }

        }

        private void OnDrawGizmos()
        {
            if (debug)
            {
                Gizmos.color = Color.yellow;
                var magneticFieldPos = transform.position;
                magneticFieldPos[1] = transform.position.y - workRadius;
                Gizmos.DrawWireSphere(magneticFieldPos, workRadius);
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(transform.position, transform.localScale*2);
            }
        }
    }
}
