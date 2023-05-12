using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myStateMachine;
    public class WalkingState : GroundedState
    {
    public WalkingState(Character character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }
        public override void Enter()
        {
            base.Enter();

            character.IsSprintPressed = false;
        }

        public override void HandleInput()
        {
            base.HandleInput();

        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (character.IsSprintPressed && character.staminaController.hasRegenerated && character.playerInput.y != -1)
                stateMachine.ChangeState(character.sprinting);
            if (character.playerInput.x == 0 && character.playerInput.y == 0)
                stateMachine.ChangeState(character.standing);
            if (character.IsJumpPressed && character.staminaController.playerStamina >20)
            {
                character.movementSM.ChangeState(character.jumping);
                character.ResetJump();
            }
    }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            character.MovePlayer(character.moveSpeed);
            character.SpeedControl();
        }
}

