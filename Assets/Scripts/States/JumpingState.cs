using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myStateMachine;
public class JumpingState : State
{
    private bool grounded;
    public JumpingState(Character character, StateMachine stateMachine) : base(character, stateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        character.Jump();
        grounded = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        grounded = character.CheckIfGrounded();
        if (grounded)
        {
            Debug.Log("grounded");
            stateMachine.ChangeState(character.standing);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
