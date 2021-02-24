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

        [Header("Контроллеры узла: ")]
        [SerializeField] private Interactable[] controllers;

        [Header("Устройства: ")]
        [SerializeField] private Device[] devices;

        public void Connect()
        {
            foreach(var controller in controllers)
            {
                if (!controller) { Debug.LogWarning($"Для узла \"{Name}\" не назначен контроллер"); return; }
                controller.OnInteractableUpdate.AddListener(() =>
                {
                    foreach (var device in devices)
                    {
                        if (devices != null)
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
        }
        public void Disconnect()
        {
            foreach (var controller in controllers)
            {
                if (!controller) { Debug.LogWarning($"Для узла \"{Name}\" не назначен контроллер"); return; }
                controller.ForceDisadle();
                controller.OnInteractableUpdate.RemoveAllListeners();
            }
        }
    }
    interface INode
    {
        /// <summary>
        /// Подключение устройств узла к контроллеру узла
        /// </summary>
        void Connect();

        /// <summary>
        /// Отключение устройств узла от контроллеров узла
        /// </summary>
        void Disconnect();
    }

}
