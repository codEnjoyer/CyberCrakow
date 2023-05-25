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
    public override void Exit()
    {
        base.Exit();
        character.StopMovement();
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
        character.playerInput = character.input.Player.Move.ReadValue<Vector2>();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        character.AirMovement(character.moveSpeed  * 0.5f);
        character.SpeedControl();
    }
}
