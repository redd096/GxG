using UnityEngine;

public class StateMachine : MonoBehaviour
{
    protected State state;

    public void SetState(State _state)
    {
        //exit from previous
        if (state != null)
            StartCoroutine(state.Exit());

        //enter in new one
        state = _state;
        StartCoroutine(state.Enter());
    }
}
