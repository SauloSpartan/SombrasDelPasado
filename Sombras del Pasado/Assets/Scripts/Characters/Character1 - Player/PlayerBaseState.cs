public abstract class PlayerBaseState
{
    protected PlayerStateMachine _context;
    protected PlayerStateFactory _factory;
    public PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactor)
    {
        _context = currentContext;
        _factory = playerStateFactor;
    }

    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void ExitState();

    public abstract void CheckSwitchState();

    protected private void SwitchState(PlayerBaseState newState)
    {
        ExitState();

        newState.EnterState();

        _context.CurrentState = newState;
    }
}
