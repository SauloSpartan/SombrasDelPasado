using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRandomNumber : MonoBehaviour
{

    void Start()
    {
        StartCoroutine(RandomNumber());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator RandomNumber()
    {
        int randomNumber;
        for (int i = 0; i < 50; i++)
        {
            randomNumber = Random.Range(1, 3);
            Debug.Log(randomNumber);
        }
        yield return null;
    }
}
