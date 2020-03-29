using UnityEngine;

public class StateMachine : MonoBehaviour
{
    protected State state;

    public void SetState(State stateToSet)
    {
        //exit from previous
        if (state != null)
            StartCoroutine(state.Exit());

        //enter in new one
        state = stateToSet;
        StartCoroutine(state.Enter());
    }
}
