using UnityEngine;

public abstract class Enemy1BaseState
{
    public abstract void EnterState(Enemy1SateManager enemy1);
   
    public abstract void UpdateState(Enemy1SateManager enemy1);
    
    public abstract void OnTriggerState(Enemy1SateManager enemy1);

}
