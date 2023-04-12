using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myStateMachine;
public class StandingState : GroundedState
{
    //private bool jump;
    //private bool crouch;
    public bool sprint;

    public StandingState(Character character, StateMachine stateMachine) : base(character, stateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        //crouch = false;
        
        sprint = false;
    }

    public override void HandleInput()
    {
        base.HandleInput();
        sprint = Input.GetKey(character.sprintKey);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (sprint)
            stateMachine.ChangeState(character.sprinting);
    }
}
