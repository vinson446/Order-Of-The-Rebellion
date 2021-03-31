using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    public string State;

    protected State currentState;
    public State CurrentState => currentState;
    protected State previousState;
    public State PreviousState => previousState;

    protected bool InTransition { get; private set; }

    public void ChangeState<T>() where T : State
    {
        T targetState = GetComponent<T>();

        if (targetState == null)
        {
            Debug.Log("Cannot change state- state isn't attached to SM");
        }

        InitiateStateChange(targetState);
    }

    public void RevertState()
    {
        if (previousState != null)
        {
            InitiateStateChange(previousState);
        }
    }

    void InitiateStateChange(State targetState)
    {
        if (currentState != targetState && !InTransition)
        {
            Transition(targetState);
        }
    }

    void Transition(State newState)
    {
        // start transition
        InTransition = true;

        // transitioning
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();

        // end transition
        InTransition = false;
    }

    private void Update()
    {
        // simulate update in state
        if (CurrentState != null && !InTransition)
        {
            CurrentState.Tick();
        }
    }
}
