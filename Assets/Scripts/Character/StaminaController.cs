using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using myStateMachine;

public class StaminaController : MonoBehaviour
{
    [Header("Stamina main parameters")]
    public float playerStamina = 100.0f;
    [SerializeField]
    private float maxStamina = 100f;
    [SerializeField]
    private float jumpCost = 20f;
    [HideInInspector] public bool hasRegenerated = true;

    [Header("Stamina Regen")]
    [Range(0, 50)] [SerializeField] private float staminaDrain = 0.5f;
    [Range(0, 50)] [SerializeField] private float staminaRegen = 0.5f;
    [Header("Stamina UI")]
    [SerializeField] private Slider staminaUI = null;
    [SerializeField] private CanvasGroup sliderCanvasGroup = null;

    private Character character;

    private void Start()
    {
        character = GetComponent<Character>();
    }
    private void Update()
    {
        if(character.movementSM.CurrentState != character.sprinting || character.movementSM.CurrentState != character.air)
            if(playerStamina<=maxStamina - 0.001 )
            {
                if(!character.IdleCheck())
                    playerStamina += staminaRegen * Time.deltaTime;
                else
                    playerStamina += staminaRegen * Time.deltaTime * 2;

                UpdateStamina(1);
            }
        if (playerStamina >= maxStamina)
        {
            sliderCanvasGroup.alpha = 0;
            hasRegenerated = true;
        }
    }

    public void Sprinting()
    {
        if (hasRegenerated)
        {
            //Debug.Log(playerStamina);
            if(!character.IdleCheck())
                playerStamina -= staminaDrain * Time.deltaTime;
            UpdateStamina(1);
            if(playerStamina <= 0)
            {
                hasRegenerated = false;
            }
        }
    }
    public void StaminaJump()
    {
        if(playerStamina >= (maxStamina * jumpCost/maxStamina))
        {
            character.Jump();
            playerStamina -= jumpCost;
            UpdateStamina(1);
        }
    }
    void UpdateStamina(int value)
    {
        staminaUI.value = playerStamina;
        if (value ==0)
        {
            sliderCanvasGroup.alpha = 0;
        }
        else
        {
            sliderCanvasGroup.alpha = 1;
        }
    }
}
