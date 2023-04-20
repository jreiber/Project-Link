using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgingState : PlayerBaseState
{
    private readonly int DodgeRightHash = Animator.StringToHash("DodgeRight");
    private readonly int DodgeLeftHash = Animator.StringToHash("DodgeLeft");
    private readonly int ForwardRollHash = Animator.StringToHash("ForwardRoll");
    private readonly int BackFlipHash = Animator.StringToHash("BackFlip");
    private const float AnimatorDampTime = 0.1f;
    private const float CrossFadeDuration = 0.1f;

    private Vector3 dodgingDirectionInput;
    private bool forceForwardRoll;
    private float remainingDodgeTime;

    public PlayerDodgingState(PlayerStateMachine stateMachine, Vector3 dodgingDirectionInput, bool forceForwardRoll = false) : base(stateMachine)
    {
        this.dodgingDirectionInput = dodgingDirectionInput;
        this.forceForwardRoll = forceForwardRoll;
    }

    public override void Enter()
    {

        remainingDodgeTime = stateMachine.DodgeDuration;

        if(forceForwardRoll)
        {
            stateMachine.Animator.CrossFadeInFixedTime(ForwardRollHash, CrossFadeDuration);
        }
        else if (dodgingDirectionInput.x > 0)
        {
            stateMachine.Animator.CrossFadeInFixedTime(DodgeRightHash, CrossFadeDuration);
        }
        else if (dodgingDirectionInput.x < 0)
        {
            stateMachine.Animator.CrossFadeInFixedTime(DodgeLeftHash, CrossFadeDuration);
        }
        else if (dodgingDirectionInput.y > 0)
        {
            stateMachine.Animator.CrossFadeInFixedTime(ForwardRollHash, CrossFadeDuration);
        }
        else if(dodgingDirectionInput.y < 0)
        {
            stateMachine.Animator.CrossFadeInFixedTime(BackFlipHash, CrossFadeDuration);
        }
        
        

        stateMachine.Health.SetInvulnerable(true);
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = new Vector3();

        movement += Camera.main.transform.right * dodgingDirectionInput.x * stateMachine.DodgeDistance / stateMachine.DodgeDuration;
        movement += Camera.main.transform.forward * dodgingDirectionInput.y * stateMachine.DodgeDistance / stateMachine.DodgeDuration;

        //movement += stateMachine.transform.right * dodgingDirectionInput.x * stateMachine.DodgeDistance / stateMachine.DodgeDuration;
        //movement += stateMachine.transform.forward * dodgingDirectionInput.y * stateMachine.DodgeDistance / stateMachine.DodgeDuration;

        Move(movement, deltaTime);

        FaceTarget();

        remainingDodgeTime -= deltaTime;

        if (remainingDodgeTime <= 0f)
        {
            stateMachine.SwitchToPreviousState();
        }

    }

    public override void Exit()
    {
        stateMachine.Health.SetInvulnerable(false);
    }


}
