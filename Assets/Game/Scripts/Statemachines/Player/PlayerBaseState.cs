using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine stateMachine;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
    }

    protected void FaceTarget()
    {
        if (stateMachine.Targeter.CurrentTarget == null) { return; }

        Vector3 lookPos = stateMachine.Targeter.CurrentTarget.transform.position - stateMachine.transform.position;
        lookPos.y = 0f;

        stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
    }


    protected void ReturnToLocomotion()
    {

        if (stateMachine.InputReader.IsBlocking)
        {
            stateMachine.SwitchState(new PlayerBlockingState(stateMachine));
        }
        else if (stateMachine.Targeter.CurrentTarget != null)
        {
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
        }
        else
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        }
    }

    protected Vector3 CalculateTargetingMovement(float deltaTime)
    {
        Vector3 movement = new Vector3();

        movement += stateMachine.transform.right * stateMachine.InputReader.MovementValue.x;
        movement += stateMachine.transform.forward * stateMachine.InputReader.MovementValue.y;

        return movement;
    }

    protected void UpdateMovementAnimator(float deltaTime, int parameterForwardHash, int parameterRightHash, float animatorDampTime)
    {
        float y = stateMachine.InputReader.MovementValue.y;
        float x = stateMachine.InputReader.MovementValue.x;

        if (y == 0)
        {
            stateMachine.Animator.SetFloat(parameterForwardHash, 0f, animatorDampTime, deltaTime);
        }
        else if (y > 0)
        {
            stateMachine.Animator.SetFloat(parameterForwardHash, 1f, animatorDampTime, deltaTime);
        }
        else if (y < 0)
        {
            stateMachine.Animator.SetFloat(parameterForwardHash, -1f, animatorDampTime, deltaTime);
        }

        if (x == 0)
        {
            stateMachine.Animator.SetFloat(parameterRightHash, 0f, animatorDampTime, deltaTime);
        }
        else if (x > 0)
        {
            stateMachine.Animator.SetFloat(parameterRightHash, 1f, animatorDampTime, deltaTime);
        }
        else if (x < 0)
        {
            stateMachine.Animator.SetFloat(parameterRightHash, -1f, animatorDampTime, deltaTime);
        }

    }


}
