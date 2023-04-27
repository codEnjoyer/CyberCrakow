using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myStateMachine;
    public class WalkingState : GroundedState
    {
        private float horizontalInput;
        private float verticalInput;
    public WalkingState(Character character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }
        public override void Enter()
        {
            base.Enter();
            //crouch = false;

            character.IsSprintPressed = false;
        }

        public override void HandleInput()
        {
            base.HandleInput();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (character.IsSprintPressed)
                stateMachine.ChangeState(character.sprinting);
            if (character.verticalInput == 0 && character.horizontalInput == 0)
                stateMachine.ChangeState(character.standing);
            if (character.IsJumpPressed)
            {
                character.movementSM.ChangeState(character.jumping);
                character.ResetJump();
            }
    }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            character.MovePlayer(character.horizontalInput, character.verticalInput, character.moveSpeed);
            character.SpeedControl();
        }
}

