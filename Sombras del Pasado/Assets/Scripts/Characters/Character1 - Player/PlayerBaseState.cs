// Establishes methods and variables that all concrete states must have
public abstract class PlayerBaseState
{
    /// <value> Short for CONTEXT, it is a reference to the State Machine Script for the concrete states. </value>
    protected PlayerStateMachine _ctx;
    protected PlayerStateFactory _factory;

    /// <summary>
    /// Constructor that passes the parameters and values from the StateMachine and StateFactory.
    /// </summary>
    public PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactor)
    {
        _ctx = currentContext;
        _factory = playerStateFactor;
    }

    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void ExitState();

    public abstract void CheckSwitchState();

    /// <summary>
    /// It switches the state you are currently on, the format is "SwitchState(_factory.State());".
    /// </summary>
    /// <param name="newState"> It receives a BaseState reference that is _factory.</param>
    protected private void SwitchState(PlayerBaseState newState)
    {
        ExitState();

        newState.EnterState();

        _ctx.CurrentState = newState;
    }
}