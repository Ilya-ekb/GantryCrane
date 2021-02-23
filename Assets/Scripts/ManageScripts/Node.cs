using Assets.Scripts.DeviceScripts;
using Assets.Scripts.InteractableSystem;
using System;
using UnityEngine;

namespace Assets.Scripts.ManageScripts
{
    [Serializable]
    public class Node: INode 
    {
        [Header("Название узла: ")]
        [SerializeField] private string Name;

        [Header("Контроллер узла: ")]
        [SerializeField] private Interactable controller;

        [Header("Устройства: ")]
        [SerializeField] private Device[] devices;

        public void Connect()
        {
            if (!controller) { Debug.LogWarning($"Для узла \"{Name}\" не назначен контроллер"); return; }
            controller.OnInteractableUpdate.AddListener(() => 
            { 
                foreach (var device in devices)
                {
                    if(devices != null)
                    {
                        device.Work(controller.Signal);
                    }
                    else
                    {
                        Debug.LogWarning($"Для узла \"{Name}\" нет ни одного устройства");
                    }
                }
            });

        }
        public void Disconnect()
        {
            if (!controller) { Debug.LogWarning($"Для узла \"{Name}\" не назначен контроллер"); return; }
            controller.ForceDisadle();
            controller.OnInteractableUpdate.RemoveAllListeners();
        }
    }
    interface INode
    {
        /// <summary>
        /// Подключение устройств узла к контроллеру узла
        /// </summary>
        void Connect();

        /// <summary>
        /// Отключение устройства узла от контроллера узла
        /// </summary>
        void Disconnect();
    }

}
