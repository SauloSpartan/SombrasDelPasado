using UnityEngine;

public abstract class Enemy1BaseState
{
    public abstract void EnterState(Enemy1StateManager enemy1);
   
    public abstract void UpdateState(Enemy1StateManager enemy1);
    
    public abstract void OnTriggerState(Enemy1StateManager enemy1, Collider other);

}
