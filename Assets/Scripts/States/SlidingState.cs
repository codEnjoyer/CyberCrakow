using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myStateMachine;
public class SlidingState : State
{
    public CapsuleCollider capsule;
    public SlidingState(Character character, StateMachine stateMachine) : base(character, stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        character.gameObject.transform.localScale = new Vector3(character.gameObject.transform.localScale.x, character.slideYScale, character.gameObject.transform.localScale.z);
        character.weapon.gameObject.transform.localScale = new Vector3(character.weapon.gameObject.transform.localScale.x, character.adjustedWeaponYScale, character.weapon.gameObject.transform.localScale.z);
        character.StartSlide();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (character.input.Player.Jump.IsPressed())
            character.movementSM.ChangeState(character.walking);

    }
    public override void Exit()
    {
        base.Exit();
        character.gameObject.transform.localScale = new Vector3(character.gameObject.transform.localScale.x, character.startYScale, character.gameObject.transform.localScale.z);
        character.weapon.gameObject.transform.localScale = new Vector3(character.weapon.gameObject.transform.localScale.x, character.startWeaponYScale, character.weapon.gameObject.transform.localScale.z);
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        character.SlideMovement();
    }
}
