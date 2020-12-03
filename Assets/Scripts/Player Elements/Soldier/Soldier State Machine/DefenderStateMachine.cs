using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderStateMachine : StateMachine
{
    public IState StandbyState
    {
        get { return standby_state; }
    }

    public IState ChaseState
    {
        get { return chase_state; }
    }

    public IState ReturnToOriginalState
    {
        get { return return_to_original_state; }
    }

    private IState standby_state;
    private IState chase_state;
    private IState return_to_original_state;

    private Vector3 original_position;

    public DefenderStateMachine(Soldier context) : base(context)
    {
        original_position = context.transform.position;
    }

    public override void ChangeState()
    {
        ChangeState(StandbyState);
    }

    public override void Initialize()
    {
        inactivate_state = new Inactivate(this, true);
        standby_state = new DefenderStandby(this);
        chase_state = new DefenderChase(this);
        return_to_original_state = new DefenderReturnToOriginal(this);

        current_state = inactivate_state;

        current_state.Enter();
    }

    public Vector3 GetOriginalPos() => original_position;
}

public class DefenderStandby : State
{
    public DefenderStandby(StateMachine state_machine) : base(state_machine)
    { }

    public override void Enter()
    {
        state_machine.GetContext().EnableDetection();
    }

    public override void Exit()
    {
        state_machine.GetContext().DisableDetection();
    }

    public override void StateUpdate()
    {
    }
}

public class DefenderChase : State
{
    Ball ball;
    Transform holder;
    public DefenderChase(StateMachine state_machine) : base(state_machine)
    { }

    public override void Enter()
    {
        ball = GameManager.GetInstance().GetBall();
        holder = ball.GetHolder().transform;
    }

    public override void Exit()
    {
        ball = null;
        holder = null;
    }

    public override void StateUpdate()
    {
        if (ball.GetHolder() != null)
        {
            Soldier context = state_machine.GetContext();

            Vector3 direction = (holder.position - context.transform.position);
            direction = new Vector3(direction.x, 0f, direction.z);

            if (direction.sqrMagnitude <= Constants.DEFENDER__SQUARE_DISTANCE_TO_CATCH)
            {
                holder.GetComponent<Soldier>().Caught();

                state_machine.ChangeState((state_machine as DefenderStateMachine).ReturnToOriginalState);
            }
            else
            {
                direction = direction.normalized;
                context.transform.forward = direction;
                context.transform.position += direction * Constants.DEFENDER__NORMAL_SPEED_MULTIPLIER * Time.deltaTime;
            }
        }
        else
        {
            state_machine.ChangeState((state_machine as DefenderStateMachine).ReturnToOriginalState);
        }

    }
}

public class DefenderReturnToOriginal : State
{
    public DefenderReturnToOriginal(StateMachine state_machine) : base(state_machine)
    { }

    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void StateUpdate()
    {
        DefenderStateMachine machine = state_machine as DefenderStateMachine;
        Soldier context = machine.GetContext();

        Vector3 direction = (machine.GetOriginalPos() - context.transform.position);
        direction = new Vector3(direction.x, 0f, direction.z);

        if (direction.sqrMagnitude <= 0.5f)
        {
            context.transform.position = machine.GetOriginalPos();
            machine.ChangeState(machine.InactivateState);
        }
        else
        {
            direction = direction.normalized;

            context.transform.forward = direction;
            context.transform.position += direction * Constants.DEFENDER__RETURN_TO_ORIGINAL_POSITION_SPEED_MULTIPLIER * Time.deltaTime;
        }
        
    }
}
