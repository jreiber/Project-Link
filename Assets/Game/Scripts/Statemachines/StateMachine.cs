using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    private State currentState;
    public State previousState { get; private set; }

    public void SwitchState(State newState)
    {
        if (currentState != null && currentState != newState) { previousState= currentState; }
        Debug.Log(this + " exiting " + currentState?.ToString());
        currentState?.Exit();
        currentState = newState;
        Debug.Log(this + " entering " + currentState?.ToString());
        currentState?.Enter();
    }

    private void Update()
    {
        currentState?.Tick(Time.deltaTime);
    }
}
