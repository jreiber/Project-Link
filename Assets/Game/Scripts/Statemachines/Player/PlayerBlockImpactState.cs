using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockImpactState : PlayerBaseState
{
    private readonly int BlockImpactHash = Animator.StringToHash("BlockImpact");
    private const float CrossFadeDuration = 0.1f;

    private float duration = 1f;

    public PlayerBlockImpactState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(BlockImpactHash, CrossFadeDuration);
        stateMachine.Health.SetInvulnerable(true);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        duration -= deltaTime;

        if (duration <= 0f)
        {
            ReturnToLocomotion();
        }
    }

    public override void Exit()
    {
        stateMachine.Health.SetInvulnerable(false);
    }



}
