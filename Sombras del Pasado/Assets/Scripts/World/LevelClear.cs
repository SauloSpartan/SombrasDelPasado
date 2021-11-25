using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelClear : MonoBehaviour
{
    public int TotalEnemies;
    public int DeadEnemies = -1;
    public BoxCollider Next;

    // Start is called before the first frame update
    void Start()
    {
        DeadEnemies = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (TotalEnemies == DeadEnemies)
        {
            Next.enabled = true;
        }
    }
}
