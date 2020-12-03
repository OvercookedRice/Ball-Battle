using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public IState CaughtState
    {
        get { return caught_state; }
    }
    public IState MoveTowardsTheGateState
    {
        get { return move_towards_the_gate_state; }
    }

    // References for states in this state machine
    private IState activate_state;
    private IState move_straight_state;
    private IState chase_ball_state;
    private IState caught_state;
    private IState move_towards_the_gate_state;
    public AttackerStateMachine(Soldier context) : base(context)
    {}

    public override void Initialize()
    {
        inactivate_state = new AttackerInactivate(this);
        activate_state = new AttackerActivate(this);
        move_straight_state = new AttackerMoveStraightState(this);
        chase_ball_state = new AttackerChaseBallState(this);
        caught_state = new AttackerCaughtState(this);
        move_towards_the_gate_state = new AttackerMoveTowardsTheGateState(this);

        current_state = inactivate_state;
        current_state.Enter();
    }

    public override void ChangeState()
    {
        ChangeState(ActivateState);
    }

}

public class AttackerActivate : State
{
    public AttackerActivate(StateMachine state_machine) : base(state_machine)
    {
    }
    public override void Enter()
    {
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

        state_machine.GetContext().transform.forward = direction;
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
            state_machine.GetContext().transform.position += (direction * Constants.ATTACKER__NORMAL_SPEED_MULTIPLIER) * Time.deltaTime;
        }
    }
}

public class AttackerChaseBallState : State
{
    private Ball ball_instance = null;
    public AttackerChaseBallState(StateMachine state_machine) : base(state_machine)
    { }

    public override void Enter()
    {
        ball_instance = GameManager.GetInstance().GetBall();
    }

    public override void Exit()
    {
        ball_instance = null;
    }

    public override void StateUpdate()
    {
        if (!ball_instance.IsAttached())
        {
            Soldier context = state_machine.GetContext();

            if (ball_instance.SqrDistanceTo(context.transform) <= Constants.ATTACKER__SQUARE_DISTANCE_TO_CAPTURE_BALL)
            {
                ball_instance.Attach(context);
            }
            else
            {
                Vector3 direction = (ball_instance.transform.position - context.transform.position).normalized;
                direction = new Vector3(direction.x, 0f, direction.z);

                context.transform.forward = direction;
                context.transform.position += direction * Constants.ATTACKER__NORMAL_SPEED_MULTIPLIER * Time.deltaTime;
            }
        }        
        else if (ball_instance.GetHolder() == state_machine.GetContext())
        {
            state_machine.ChangeState((state_machine as AttackerStateMachine).MoveTowardsTheGateState);
        }
        else
        {
            state_machine.ChangeState((state_machine as AttackerStateMachine).MoveStraightState);
        }
    }
}

public class AttackerMoveTowardsTheGateState : State
{
    private Gate defender_gate;
    public AttackerMoveTowardsTheGateState(StateMachine state_machine) : base(state_machine)
    { }

    public override void Enter()
    {
        defender_gate = GameManager.GetInstance().GetDefenderGate();
    }

    public override void Exit()
    {
        defender_gate = null;
    }

    public override void StateUpdate()
    {
        Soldier context = state_machine.GetContext();
        Vector3 direction = (defender_gate.transform.position - context.transform.position).normalized;
        direction = new Vector3(direction.x, 0f, direction.z);

        context.transform.forward = direction;
        context.transform.position += direction * Constants.ATTACKER__CARRY_SPEED_MULTIPLIER * Time.deltaTime;
    }
}

public class AttackerCaughtState : State
{
    public AttackerCaughtState(StateMachine state_machine) : base(state_machine)
    { }

    public override void Enter()
    {
        Ball ball = GameManager.GetInstance().GetBall();
        Soldier context = state_machine.GetContext();

        if (ball?.GetHolder() == context)
        {
            ball.Detach();
            List<Soldier> others = GameManager.GetInstance().GetAttackers(context);

            if (others.Count < 1)
            {
                // Notify that match is over!
                //GameManager.GetInstance()
                Debug.Log("Defender's Won");
                return;
            }

            Soldier closest = others.Aggregate(others[0], 
                            (x, y) => (context.transform.position - x.transform.position).sqrMagnitude < (context.transform.position - y.transform.position).sqrMagnitude ? 
                            x : y);

            ball.Pass(closest);
        }
    }

    public override void Exit()
    {
    }

    public override void StateUpdate()
    {
        state_machine.ChangeState((state_machine as AttackerStateMachine).InactivateState);
    }
}

public class AttackerInactivate : State
{
    private float elapsed_time;

    bool first_time_using;

    public AttackerInactivate(StateMachine state_machine) : base(state_machine)
    {
        first_time_using = true;
    }

    public override void Enter()
    {
        elapsed_time = 0f;

        Soldier context = state_machine.GetContext();

        context.Bench();
        context.ChangeMaterial(true);
    }

    public override void Exit()
    {
        elapsed_time = 0f;

        Soldier context = state_machine.GetContext();

        context.Attack();
        context.ChangeMaterial(false);
        first_time_using = false;
    }

    public override void StateUpdate()
    {
        elapsed_time += Time.deltaTime;
        if (!first_time_using && elapsed_time >= Constants.ATTACKER__INACTIVATE_WAIT_TIME)
        {
            state_machine.ChangeState();
        }
        else if (first_time_using && elapsed_time >= Constants.ATTACKER__SPAWN_TIME)
        {
            state_machine.ChangeState();
        }
    }
}

