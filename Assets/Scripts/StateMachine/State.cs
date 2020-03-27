using System.Collections;

public abstract class State
{
    protected StateMachine stateMachine;

    public State(StateMachine _stateMachine)
    {
        stateMachine = _stateMachine;
    }

    public virtual IEnumerator Enter()
    {
        yield break;
    }

    public virtual void Execution()
    {

    }

    public virtual IEnumerator Exit()
    {
        yield break;
    }
}
