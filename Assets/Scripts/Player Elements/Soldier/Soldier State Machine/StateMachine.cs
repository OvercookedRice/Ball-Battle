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

public class Inactivate : State
{
    private float elapsed_time;
    protected float inactivate_time;

    bool first_time_using ;
    bool is_defender;

    public Inactivate(StateMachine state_machine, bool is_defender) : base(state_machine)
    {
        this.is_defender = is_defender;

        inactivate_time = (is_defender ? Constants.DEFENDER__INACTIVATE_WAIT_TIME : Constants.ATTACKER__INACTIVATE_WAIT_TIME);
        first_time_using = true;
    }

    public override void Enter()
    {
        elapsed_time = 0f;
        state_machine.GetContext().ChangeMaterial(true);
    }

    public override void Exit()
    {
        elapsed_time = 0f;
        state_machine.GetContext().ChangeMaterial(false);
        first_time_using = false;
    }

    public override void StateUpdate()
    {
        elapsed_time += Time.deltaTime;
        if (!first_time_using && elapsed_time >= inactivate_time)
        {
            state_machine.ChangeState();
        }
        else if (first_time_using && elapsed_time >= (is_defender ? Constants.DEFENDER__SPAWN_TIME : Constants.ATTACKER__SPAWN_TIME))
        {
            state_machine.ChangeState();
        }
    }
}
