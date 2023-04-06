using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myStateMachine;

namespace myStateMachine
{
    public class SprintingState : GroundedState
    {
        private bool sprint;
        public SprintingState(Character character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }
        public override void Enter()
        {
            base.Enter();
            character.moveSpeed = character.sprintSpeed;
            sprint = true;
        }
        public override void HandleInput()
        {
            base.HandleInput();
            sprint = Input.GetKey(character.sprintKey);
        }
        public override void Exit()
        {
            base.Exit();
            character.moveSpeed = character.walkingSpeed;
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!sprint)
                character.movementSM.ChangeState(character.standing);
        }
    }
}
