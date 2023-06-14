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
                character.playerInput = character.input.Player.Move.ReadValue<Vector2>();
            }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!character.CheckIfGrounded())
            {
                stateMachine.ChangeState(character.air);
            }

            if (character.input.Player.Slide.IsPressed() && (character.playerInput.x != 0 | character.playerInput.y != 0))
                stateMachine.ChangeState(character.slide);
        }
        public override void PhysicsUpdate()
            {
                base.PhysicsUpdate();
            }
    }
    }
