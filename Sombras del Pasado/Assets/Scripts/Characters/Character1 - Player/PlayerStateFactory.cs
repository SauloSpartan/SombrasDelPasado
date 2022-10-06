public class PlayerStateFactory
{
    PlayerStateMachine _context;

    /// <summary>
    /// Constructor function that is called when we create a new instance of a class
    /// </summary>
    /// <param name="currentContext"></param>
    public PlayerStateFactory(PlayerStateMachine currentContext) // Expects a PlayerStateMachine instance to be passed in
    {
        _context = currentContext;
    }

    public PlayerBaseState Idle() //Returns a new instance of his respective state
    {
        return new PlayerIdleState(_context, this);
    }

    public PlayerBaseState Walk()
    {
        return new PlayerWalkState(_context, this);
    }

    public PlayerBaseState Attack()
    {
        return new PlayerAttackState(_context, this);
    }

    public PlayerBaseState Damage()
    {
        return new PlayerDamageState(_context, this);
    }

    public PlayerBaseState Death()
    {
        return new PlayerDeathState(_context, this);
    }
}
