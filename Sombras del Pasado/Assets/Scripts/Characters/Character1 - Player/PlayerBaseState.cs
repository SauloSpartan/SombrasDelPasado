public abstract class PlayerBaseState
{
    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void ExitState();

    public abstract void CheckSwitchState();

    private void SwitchState(PlayerBaseState newState)
    {
        ExitState();

        newState.EnterState();
    }
}
