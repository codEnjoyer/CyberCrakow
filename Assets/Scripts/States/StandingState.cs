using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myStateMachine;
public class StandingState : GroundedState
{
    private bool jump;
    //private bool crouch;
    public bool sprint;

    public StandingState(Character character, StateMachine stateMachine) : base(character, stateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        //crouch = false;
        character.IsJumpPressed = false;
        character.IsSprintPressed = false;
        //character.StopMovement();
    }

    public override void HandleInput()
    {
        base.HandleInput();

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (character.IsSprintPressed && character.staminaController.hasRegenerated)
            stateMachine.ChangeState(character.sprinting);

        if (character.IsJumpPressed && character.readyToJump && character.staminaController.playerStamina >=20)
        {
            stateMachine.ChangeState(character.jumping);
            character.ResetJump();
        }
        if(character.verticalInput != 0 || character.horizontalInput != 0 && !character.IsSprintPressed)
        {
            stateMachine.ChangeState(character.walking);
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        character.MovePlayer(character.horizontalInput, character.verticalInput, character.moveSpeed);
    }
}
