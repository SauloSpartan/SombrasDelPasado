using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerLeft : MonoBehaviour
{
    public BoxCollider daggerLeft;

    void Start()
    {
        daggerLeft = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
