using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts.ManageScripts
{
    public class MagnetedObject : ControlledObject
    {
        public override Vector3 position 
        {
            get => base.position; 
            set 
            {
                rigidbody.velocity = value;
            }
        }

        private new Rigidbody rigidbody;
        protected override void Start()
        {
            base.Start();
            InitializateRigidbody();
        }

        /// <summary>
        /// Добавление и настройка Rigidboby
        /// </summary>
        private void InitializateRigidbody()
        {
            rigidbody = GetComponent<Rigidbody>();
            if (!rigidbody) rigidbody = gameObject.AddComponent<Rigidbody>();
            rigidbody.isKinematic = false;
            rigidbody.useGravity = true;
        }


        public override void Connected(Transform transform)
        {
            base.Connected(transform);
            DestroyImmediate(rigidbody);
        }
        public override void Disconnected()
        {
            base.Disconnected();
            InitializateRigidbody();
        }
    }
}
