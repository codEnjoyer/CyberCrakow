using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace myStateMachine
{
    public class GroundedState : State
    {


            public GroundedState(Character character, StateMachine stateMachine) : base(character, stateMachine)
            {

            }
            public override void Enter()
            {
                base.Enter();
                character.IsJumpPressed = false;
            }

            public override void Exit()
            {
                base.Exit();
            }

            public override void HandleInput()
            {
                base.HandleInput();
                //character.IsSprintPressed = Input.GetKey(character.sprintKey);
                //character.IsJumpPressed = Input.GetButtonDown("Jump");
                //character.verticalInput = Input.GetAxisRaw("Vertical");
                //character.horizontalInput = Input.GetAxisRaw("Horizontal");
                character.playerInput = character.input.Player.Move.ReadValue<Vector2>();
            }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!character.CheckIfGrounded())
            {
                stateMachine.ChangeState(character.air);
            }
        }
        public override void PhysicsUpdate()
            {
                base.PhysicsUpdate();
            }
    }
    }
