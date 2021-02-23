using Assets.Scripts.DeviceScripts;
using Assets.Scripts.InputSystem;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts.ManageScripts
{
    public class Dashboard : Device
    {
        [Tooltip("Установить true, если нужно включать связи в рабочих узлах при старте")]
        public bool connectOnStart;
        [Header("Рабочие узлы:")]
        public Node[] nodes;

        /// <summary>
        /// Включение/Отключение связи между контроллером и устройствами всех узлов на дашборте
        /// </summary>
        /// <param name="turn"></param>
        private void TurnNodes (Turn turn)
        {
            foreach(var node in nodes)
            {
                if(turn == Turn.On) { node.Connect(); }
                if(turn == Turn.Off) { node.Disconnect(); }
            }
        }

        public override void Work(float signal)
        {
            TurnNodes((Turn)signal);
        }

        protected override void InitialSettings() 
        {
            if (connectOnStart) TurnNodes(Turn.On);
        }

        enum Turn
        {
            Off,
            On
        }
    }
}
