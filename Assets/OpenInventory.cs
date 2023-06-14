using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using myStateMachine;
namespace Grid_Inventory
{ 
public class OpenInventory : MonoBehaviour
{
    public PlayerInput input;
    public Canvas HUD;
    public GameObject InventoryCanvas;
    public bool isOpen;
    public Camera playerCam;
    CameraMovement cameraMovement;
    Character character;
    public InventoryController controller;
        [SerializeField]
    public Chest[] chests;
        //[HideInInspector] public bool isChestOpen; 
    private void Start()
    {
        input = new PlayerInput();
        character = GetComponent<Character>();
        cameraMovement = playerCam.GetComponent<CameraMovement>();        
        input.Enable();
        input.Inventory.Open.performed += Inventory_performed; 
        controller.input.Disable();
        InventoryCanvas.gameObject.SetActive(false);
        }
    //private void OnEnable()
    //{
    //    InventoryCanvas.gameObject.SetActive(false);
    //}
    private void Inventory_performed(InputAction.CallbackContext obj)
    {
        if (!isOpen)
        {
            Open();
        }
        else
        {
            Close();
        }
    }

    public void Open()
    {
        Debug.Log("Inventory Open");
        HUD.gameObject.SetActive(false);
        controller.SelectedItemGrid = null; 
        InventoryCanvas.gameObject.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        isOpen = true;
        cameraMovement.enabled = false;
        character.input.Player.Disable();
        controller.input.Enable();
    }
    public void Close()
    {
        Debug.Log("Inventory Closed");
        HUD.gameObject.SetActive(true);
        InventoryCanvas.gameObject.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isOpen = false;
        cameraMovement.enabled = true;
        character.input.Player.Enable();
        controller.input.Disable();
            for(int i =0;i<chests.Length;i++)
            {
                chests[i].CloseChest();
            }
    }
}
}
