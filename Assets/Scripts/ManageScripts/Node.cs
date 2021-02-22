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

        public void Connect()
        {
            controller.OnInteractableUpdate.AddListener(() => { foreach (var device in devices) device.Work(controller.Signal); });
        }
    }
}
