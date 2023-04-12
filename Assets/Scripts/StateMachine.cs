using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace myStateMachine
{ 
public class StateMachine
{
      public  StandingState standingState;
      public  JumpingState airState;
      public State CurrentState { get; private set; }

        public void Initialize(State startingState)
    {
        CurrentState = startingState;
        startingState.Enter();
    }

    public void ChangeState(State newState)
    {
            CurrentState.Exit();
            CurrentState = newState;
        newState.Enter();
    }
}
}
