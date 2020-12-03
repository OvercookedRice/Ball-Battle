using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine
{
    public IState CurrentState
    {
        get { return current_state; }
    }

    public IState InactivateState
    {
        get { return inactivate_state; }
    }

    protected Soldier context;
    protected IState current_state;

    // Reference to states for this state machine
    protected IState inactivate_state;

    public StateMachine(Soldier context)
    {
        this.context = context;
        Initialize();
    }

    public abstract void Initialize();

    /// <summary>
    /// This method is used for changing machine state from Inactivate to Acitvate
    /// </summary>
    public abstract void ChangeState();

    /// <summary>
    /// Change this machine state to <paramref name="state"/>
    /// <para>Parameters:</para>
    /// <para><paramref name="state"/>: next state.</para>
    /// </summary>
    /// <param name="state"></param>
    public virtual void ChangeState(IState state)
    {
        current_state.Exit();

        current_state = state;

        current_state.Enter();
    }

    public void Update() => current_state.StateUpdate();

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
