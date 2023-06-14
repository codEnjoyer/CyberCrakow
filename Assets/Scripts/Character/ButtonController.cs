using myStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Grid_Inventory;
public class ButtonController : MonoBehaviour
{
    [SerializeField] private int rayLength;
    [SerializeField] private LayerMask layermaskInteract;
    [SerializeField] private string excludeLayer = null;
    private InteractableObject raycastedButton;
    public Transform orientation;
    [SerializeField] private Image crosshair = null;
    private bool doOnce;
    private const string buttonTag = "Button";
    private const string inventoryItem = "Item";
    private const string chestTag = "Chest";
    //private Character character;
    public PlayerInput input;
    OpenInventory opener;
    public Camera camera;
    private void Awake()
    {
        opener = GetComponent<OpenInventory>();
        input = new PlayerInput();
        input.Enable();
        //camera = GetComponent<Camera>();
    }


    private void Update()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        //Debug.D(ray);
        //if(Physics.Raycast(transform.position,fwd,out hit,rayLength,layermaskInteract))
        if (Physics.Raycast(ray, out hit,rayLength,layermaskInteract))
        {
            //Debug.Log("see mask");
            if (hit.collider.CompareTag(buttonTag))
            {
                //Debug.Log("see");
                    raycastedButton = hit.collider.gameObject.GetComponent<InteractableObject>();
                if (input.Player.Interact.IsPressed())
                {
                    //Debug.Log("pressed");
                    raycastedButton.Action();
                }
            }
            if (hit.collider.CompareTag(inventoryItem))
            {
                //Debug.Log("see");
                    raycastedButton = hit.collider.gameObject.GetComponent<InteractableObject>();

                if (input.Player.Interact.IsPressed())
                {
                    //Debug.Log("pressed");
                    raycastedButton.Action();
                    //opener.Open();
                }
            }
            if (hit.collider.CompareTag(chestTag))
            {
                //Debug.Log("see");
                raycastedButton = hit.collider.gameObject.GetComponent<InteractableObject>();

                if (input.Player.Interact.IsPressed())
                {
                    //Debug.Log("pressed");
                    raycastedButton.Action();
                    //opener.Open();
                }
            }
        }
    }
}
