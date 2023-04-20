using System;
using UnityEditorInternal;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] public InputReader InputReader { get; private set; }
    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public Targeter Targeter { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public WeaponDamage WeaponDamage { get; private set; }
    [field: SerializeField] public Health Health { get; private set; }
    //[field: SerializeField] public Ragdoll Ragdoll { get; private set; }
    //[field: SerializeField] public LedgeDetector LedgeDetector { get; private set; }
    [field: SerializeField] public float FreeLookMovementSpeed { get; private set; }
    [field: SerializeField] public float WalkingSpeed { get; private set; }
    [field: SerializeField] public float TargetingMovementSpeed { get; private set; }
    [field: SerializeField] public float BlockingMovementSpeed { get; private set; }
    [field: SerializeField] public float RotationDamping { get; private set; }
    [field: SerializeField] public float DodgeDuration { get; private set; }
    [field: SerializeField] public float DodgeDistance { get; private set; }

    //[field: SerializeField] public float JumpForce { get; private set; }
    [field: SerializeField] public Attack[] Attacks { get; private set; }
    

    public Transform MainCameraTransform { get; private set; }
    public bool IsRolling { get; private set; } = false;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        MainCameraTransform = Camera.main.transform;
        SwitchState(new PlayerFreeLookState(this));
    }

    private void OnEnable()
    {
        Health.OnTakeDamage += HandleTakeDamage;
        //Health.OnDie += HandleDeath;
        Health.OnBlockDamage += HandleBlockDamage;
    }



    private void OnDisable()
    {
        Health.OnTakeDamage -= HandleTakeDamage;
        //Health.OnDie -= HandleDeath;
        Health.OnBlockDamage -= HandleBlockDamage;
    }

    private void HandleBlockDamage()
    {
        SwitchState(new PlayerBlockImpactState(this));
    }

    private void HandleTakeDamage()
    {
        SwitchState(new PlayerImpactState(this));
    }

    /*private void HandleDeath()
    {
        SwitchState(new PlayerDeadState(this));
    }*/

    public void SwitchToPreviousState()
    {

        switch (previousState)
        {
            case PlayerFreeLookState:
                SwitchState(new PlayerFreeLookState(this));
                break;
            case PlayerTargetingState:
                SwitchState(new PlayerTargetingState(this));
                break;
            case PlayerImpactState:
                SwitchState(new PlayerImpactState(this));
                break;
            case PlayerBlockingState:
                SwitchState(new PlayerBlockingState(this));
                break;
            // Add cases for other states as necessary
            default:
                Debug.LogWarning("Unknown previous state: " + previousState);
                break;
        }
    }


}
