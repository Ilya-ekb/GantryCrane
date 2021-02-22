using Assets.Scripts.DeviceScripts;
using Assets.Scripts.InputSystem;
using System;
using UnityEngine;

namespace Assets.Scripts.ManageScripts
{
    [Serializable]
    public class Node 
    {
        [Header("Название узла: ")]
        [SerializeField] private string Name;

        [Header("Контроллер узла: ")]
        [SerializeField] private Interactable controller;

        [Header("Устройства: ")]
        [SerializeField] private Device[] devices;

        /// <summary>
        /// Подключение устройств узла к контроллеру узла
        /// </summary>
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
    }
}
