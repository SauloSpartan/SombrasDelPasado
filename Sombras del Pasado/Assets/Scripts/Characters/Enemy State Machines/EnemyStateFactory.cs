// Used to create concrete states
public class EnemyStateFactory
{
    EnemyStateMachine _context;

    /// <summary>
    /// Constructor function that is called when we create a new instance of a class.
    /// </summary>
    /// <param name="currentContext"> Holds reference for our State Machine. </param>
    public EnemyStateFactory(EnemyStateMachine currentContext) // Expects a EnemyStateMachine instance to be passed in
    {
        _context = currentContext;
    }

    public EnemyBaseState Idle() //Returns a new instance of his respective state
    {
        return new EnemyIdleState(_context, this); // We pass in the same context that we have in StateFactory constructor
    }

    public EnemyBaseState Walk()
    {
        return new EnemyWalkState(_context, this);
    }

    public EnemyBaseState Provoke()
    {
        return new EnemyProvokeState(_context, this);
    }

    public EnemyBaseState Attack()
    {
        return new EnemyAttackState(_context, this);
    }

    public EnemyBaseState Damage()
    {
        return new EnemyDamageState(_context, this);
    }

    public EnemyBaseState Death()
    {
        return new EnemyDeathState(_context, this);
    }
}