// Establishes methods and variables that all concrete states must have
public abstract class EnemyBaseState
{
    /// <value> Short for CONTEXT, it is a reference to the State Machine Script for the concrete states. </value>
    protected EnemyStateMachine _ctx;
    protected EnemyStateFactory _factory;

    /// <summary>
    /// Constructor that passes the parameters and values from the StateMachine and StateFactory.
    /// </summary>
    public EnemyBaseState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactor)
    {
        _ctx = currentContext;
        _factory = enemyStateFactor;
    }

    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void ExitState();

    public abstract void CheckSwitchState();

    /// <summary>
    /// It switches the state you are currently on, the format is "SwitchState(_factory.State());".
    /// </summary>
    /// <param name="newState"> It receives a BaseState reference that is _factory.</param>
    protected private void SwitchState(EnemyBaseState newState)
    {
        ExitState();

        newState.EnterState();

        _ctx.CurrentState = newState;
    }
}