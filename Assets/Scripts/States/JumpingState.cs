using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myStateMachine;
public class JumpingState : State
{
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
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        character.AirMovement(character.moveSpeed);
        character.SpeedControl();
    }
}
