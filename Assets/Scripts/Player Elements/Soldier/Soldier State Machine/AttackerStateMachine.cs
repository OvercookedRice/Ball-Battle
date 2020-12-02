using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerStateMachine : StateMachine
{
    public IState ActivateState
    {
        get { return activate_state; }
    }

    public IState MoveStraightState
    {
        get { return move_straight_state; }
    }

    public IState ChaseBallState
    {
        get { return chase_ball_state; }
    }

    public IState MoveTowardsTheGateState
    {
        get { return move_towards_the_gate_state; }
    }

    // Reference to states for this state machine
    private IState activate_state;
    private IState move_straight_state;
    private IState chase_ball_state;
    private IState move_towards_the_gate_state;
    public AttackerStateMachine(Soldier context) : base(context)
    {}

    public override void Initialize()
    {
        activate_state = new AttackerActivate(this);

        current_state = activate_state;
        current_state.Enter();
    }
}

public class AttackerActivate : State
{
    public AttackerActivate(StateMachine state_machine) : base(state_machine)
    {
    }
    public override void Enter()
    {
        Soldier soldier = state_machine.GetContext();

        GameManager GM = GameManager.GetInstance();
        soldier.GetComponent<MeshRenderer>().material = GM.GetFactionMaterial(GM.GetAttacker());
    }

    public override void Exit()
    { 
    }

    public override void StateUpdate()
    {
        Ball ball = GameManager.GetInstance().GetBall();
        if (ball != null && !ball.IsAttached())
        {
            state_machine.ChangeState((state_machine as AttackerStateMachine).ChaseBallState);
        }
        else
        {
            state_machine.ChangeState((state_machine as AttackerStateMachine).MoveStraightState);
        }
    }
}

public class AttackerMoveStraightState : State
{
    private Vector3 direction;
    public AttackerMoveStraightState(StateMachine state_machine) : base(state_machine)
    { }
    public override void Enter()
    {
        Fence target_fence = GameManager.GetInstance().GetDefenderFence();

        Vector3 _ = target_fence.transform.position - state_machine.GetContext().transform.position;
        direction = (_.z > 0f ? 1 : -1) * Vector3.forward;
    }

    public override void Exit()
    {
        direction = Vector3.zero;
    }

    public override void StateUpdate()
    {
        if (direction == Vector3.zero) return;

        Ball ball = GameManager.GetInstance().GetBall();
        if (ball != null && !ball.IsAttached())
        {
            state_machine.ChangeState((state_machine as AttackerStateMachine).ChaseBallState);
        }
        else
        {
            state_machine.GetContext().transform.Translate(direction * Constants.ATTACKER__NORMAL_SPEED_MULTIPLIER);
        }
    }
}