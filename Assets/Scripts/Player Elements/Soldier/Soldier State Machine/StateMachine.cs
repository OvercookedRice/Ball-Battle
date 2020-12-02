using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine
{
    protected Soldier context;
    protected IState current_state;

    public StateMachine(Soldier context)
    {
        this.context = context;
        Initialize();
    }

    public abstract void Initialize();

    public virtual void ChangeState(IState state)
    {
        current_state.Exit();

        current_state = state;

        current_state.Enter();
    }

    public Soldier GetContext() => context;
}

public abstract class State : IState
{
    protected StateMachine state_machine;

    public State(StateMachine state_machine)
    {
        this.state_machine = state_machine;
    }

    public abstract void Enter();
    public abstract void Exit();
    public abstract void StateUpdate();
}
