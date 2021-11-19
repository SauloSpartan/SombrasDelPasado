using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerRight : MonoBehaviour
{
    public BoxCollider daggerRight;

    void Start()
    {
        daggerRight = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
