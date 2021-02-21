using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts.DeviceScripts
{
    public class Device : MonoBehaviour
    {
        private void Start()
        {
            InitialSettings();
        }
        protected virtual void InitialSettings() { }
        public virtual void Work(float signal) { }

    }
}
