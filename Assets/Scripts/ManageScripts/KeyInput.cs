using Assets.Scripts.InteractableSystem;
using UnityEngine;

namespace Assets.Scripts.ManageScripts
{
    public class KeyInput : SystemInput
    {
        [Header("Контроллер движения стана:")]
        [SerializeField] private Interactable ControllerFB;
        [Header("Вперед:")]
        [SerializeField] KeyCode forward;
        [Header("Назад:")]
        [SerializeField] KeyCode back;

        [Header("Контроллер движения каретки:")]
        [SerializeField] private Interactable ControllerLR;
        [Header("Влево:")]
        [SerializeField] KeyCode left;
        [Header("Вправо:")]
        [SerializeField] KeyCode right;

        [Header("Контроллер движения подъемного механизма:")]
        [SerializeField] private Interactable ControllerUD;
        [Header("Вверх:")]
        [SerializeField] KeyCode up;
        [Header("Вниз:")]
        [SerializeField] KeyCode down;

        [Header("Кнопка включения/выключения магнита:")]
        [SerializeField] private Interactable ButtonMagner;
        [Header("Вкл/Выкл:")]
        [SerializeField] KeyCode magnet;

        [Header("Кнопка включения/выключения питания:")]
        [SerializeField] private Interactable ButtonPower;
        [Header("Вкл/Выкл:")]
        [SerializeField] KeyCode power;

        public override void OnAttach()
        {
            throw new System.NotImplementedException();
        }

        public override void OnAttachedUpdate()
        {
            throw new System.NotImplementedException();
        }

        public override void OnDetach()
        {
            throw new System.NotImplementedException();
        }
    }
}
