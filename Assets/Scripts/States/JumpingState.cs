using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myStateMachine;
public class JumpingState : State
{
    private float horizontalInput = 0f;
    private float verticalInput = 0f;
    public JumpingState(Character character, StateMachine stateMachine) : base(character, stateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        character.staminaController.StaminaJump();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (character.CheckIfGrounded())
        {
            //Debug.Log("grounded");
            stateMachine.ChangeState(character.standing);
        }
    }
    public override void HandleInput()
    {
        base.HandleInput();
        verticalInput = Input.GetAxisRaw("Vertical");
        horizontalInput = Input.GetAxisRaw("Horizontal");
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        character.AirMovement(horizontalInput, verticalInput, character.moveSpeed);
        character.SpeedControl();
    }
}
