using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myStateMachine;

    public class SprintingState : GroundedState
    {
        public SprintingState(Character character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }
        public override void Enter()
        {
            base.Enter();
            character.moveSpeed = character.sprintSpeed;
            character.IsSprintPressed = true;
        }
        public override void HandleInput()
        {
            base.HandleInput();
            character.IsSprintPressed = Input.GetKey(character.sprintKey);
        }
        public override void Exit()
        {
            base.Exit();
            character.moveSpeed = character.walkingSpeed;
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            character.staminaController.Sprinting();
            if (!character.IsSprintPressed || !character.staminaController.hasRegenerated || character.playerInput.y ==-1)
                character.movementSM.ChangeState(character.standing);
            if(character.IsJumpPressed && character.staminaController.playerStamina >=20)
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

