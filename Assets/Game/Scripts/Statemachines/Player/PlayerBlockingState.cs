using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockingState : PlayerBaseState
{
    private readonly int BlockingBlendTreeHash = Animator.StringToHash("BlockingBlendTree");
    private readonly int BlockingForwardSpeedHash = Animator.StringToHash("BlockingForwardSpeed");
    private readonly int BlockingRightSpeedHash = Animator.StringToHash("BlockingRightSpeed");
    private readonly int BlockHash = Animator.StringToHash("Block");
    private const float AnimatorDampTime = 0.1f;
    private const float CrossFadeDuration = 0.1f;

    public PlayerBlockingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(BlockingBlendTreeHash, CrossFadeDuration);
        stateMachine.Health.SetInvulnerable(true);


    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        if (!stateMachine.InputReader.IsBlocking)
        {
            ReturnToLocomotion();
            return;
        }

        if (stateMachine.Targeter.CurrentTarget != null)
        {
            Vector3 movement = CalculateTargetingMovement(deltaTime);
            Move(movement * stateMachine.BlockingMovementSpeed, deltaTime);

            UpdateMovementAnimator(deltaTime, BlockingForwardSpeedHash, BlockingRightSpeedHash, AnimatorDampTime);

            FaceTarget();
        }

    }

    public override void Exit()
    {
        stateMachine.Health.SetInvulnerable(false);

    }

}
