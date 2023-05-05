using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myStateMachine;
public class AirState : State
{
    public AirState(Character character, StateMachine stateMachine) : base(character, stateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
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
        character.verticalInput = Input.GetAxisRaw("Vertical");
        character.horizontalInput = Input.GetAxisRaw("Horizontal");
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        character.AirMovement(character.horizontalInput, character.verticalInput,character.moveSpeed  * 0.5f);
        character.SpeedControl();
    }
}
