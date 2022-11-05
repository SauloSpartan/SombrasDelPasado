using UnityEngine;

public class LevelClear : MonoBehaviour
{
    public int TotalEnemies;
    public int DeadEnemies = -1;
    public BoxCollider Next;

    void Start()
    {
        DeadEnemies = 0;
    }

    void Update()
    {
        if (TotalEnemies == DeadEnemies)
        {
            Next.enabled = true;
        }
    }
}
