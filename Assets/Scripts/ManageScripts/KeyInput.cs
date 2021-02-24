using Assets.Scripts.InteractableSystem;
using Assets.Scripts.ManageScripts;

using System.Collections;

using UnityEngine;

public class KeyInput : SystemInput
{
    [SerializeField] private KeyCode KeyCode;
    [SerializeField] private Interactable interactable;
    private Vector3 position = Vector3.up;

    public override void OnAttach()
    {
        attachedObject = interactable;
        attachedObject?.InteractableBegin(position);
    }

    public override void OnAttachedUpdate()
    {
        position += Vector3.down;
        attachedObject?.InteractableUpdate(position);
        Debug.Log(position);
    }

    public override void OnDetach()
    {
        position = Vector3.zero;
        attachedObject.InteractableEnd();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode))
        {
            OnAttach();
        }
        if (Input.GetKey(KeyCode))
        {
            OnAttachedUpdate();
        }
        if (Input.GetKeyUp(KeyCode))
        {
            OnDetach();
        }
    }

}
