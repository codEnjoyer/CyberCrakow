using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace myStateMachine
{
    public class GroundedState : State
    {

            private float horizontalInput;
            private float verticalInput;
            private bool jump;

            public GroundedState(Character character, StateMachine stateMachine) : base(character, stateMachine)
            {

            }
            public override void Enter()
            {
                base.Enter();
                horizontalInput = verticalInput = 0.0f;
                jump = false;
            }

            public override void Exit()
            {
                base.Exit();
            }

            public override void HandleInput()
            {
                base.HandleInput();
                verticalInput = Input.GetAxisRaw("Vertical");
                horizontalInput = Input.GetAxisRaw("Horizontal");
                jump = Input.GetButtonDown("Jump");
            }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (jump && character.readyToJump)
            {
                stateMachine.ChangeState(character.jumping);
                character.ResetJump();
            }
        }
        public override void PhysicsUpdate()
            {
                base.PhysicsUpdate();
                character.MovePlayer(horizontalInput, verticalInput,true, character.moveSpeed);
                character.SpeedControl();
            }
    }
    }
