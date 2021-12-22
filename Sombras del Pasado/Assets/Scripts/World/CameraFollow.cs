using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //Variable for offset
    [SerializeField] private Vector3 offset;

    private float duration = 0.5f;
    private float magnitude = 1.0f;

    void Start()
    {
        
    }

    void Update()
    {
        gameObject.transform.position = GameObject.Find("Character1").transform.position + offset;

        //This is the distance between player and camera in X, Y and Z
        Vector3 playerPos = new Vector3(GameObject.Find("Main Camera").transform.position.x, 
            gameObject.transform.position.y, gameObject.transform.position.z);
        gameObject.transform.position = playerPos;

        //To change camera distance just write trasnform.position.? +/- c
    }

    public IEnumerator CameraShake()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x_Position = Random.Range(-0.5f, 0.5f) * magnitude;
            float y_Position = Random.Range(2.5f, 3.5f) * magnitude;

            offset = new Vector3(x_Position, y_Position, offset.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        offset = new Vector3(0, 3, -4);
    }
}
