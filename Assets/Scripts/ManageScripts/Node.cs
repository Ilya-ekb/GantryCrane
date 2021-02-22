using Assets.Scripts.DeviceScripts;
using Assets.Scripts.InputSystem;
using System;
using UnityEngine;

namespace Assets.Scripts.ManageScripts
{
    [Serializable]
    public class Node 
    {
        [SerializeField] private string Name;
        [SerializeField] private MovingDevice device;
        [SerializeField] private Interactable controller;

        public void Connect()
        {
            controller.OnInteractableUpdate.AddListener(() => { device.Work(controller.Signal); });
        }
    }
}
