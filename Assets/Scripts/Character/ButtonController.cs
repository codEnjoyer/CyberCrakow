using myStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private int rayLength;
    [SerializeField] private LayerMask layermaskInteract;
    [SerializeField] private string excludeLayer = null;
    private PushButton raycastedButton;
    public Transform orientation;
    [SerializeField] private Image crosshair = null;
    private bool doOnce;
    private const string interactableTag = "Button";
    //private Character character;
    public PlayerInput input;
    private void Awake()
    {
        input = new PlayerInput();
        input.Enable();
    }


    private void Update()
    {
        RaycastHit hit;
        Vector3 fwd = orientation.TransformDirection(Vector3.forward);

        int mask = 1<< LayerMask.NameToLayer(excludeLayer)| layermaskInteract.value;

        //Debug.DrawRay(orientation.position, fwd);
        if(Physics.Raycast(transform.position,fwd,out hit,rayLength,mask))
        {
            if(hit.collider.CompareTag(interactableTag))
            {
                //Debug.Log("see");
                if (!doOnce)
                {
                    raycastedButton = hit.collider.gameObject.GetComponent<PushButton>();
                }
                doOnce = true;
                if (input.Player.Interact.IsPressed())
                {
                    Debug.Log("pressed");
                    raycastedButton.Action();
                }
            }

        }
    }
}
