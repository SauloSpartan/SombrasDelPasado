using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    void Start()
    {
        
    }
    //Variable for offset
    public Vector3 offset;
    void Update()
    {

        gameObject.transform.position = GameObject.Find("Character1").transform.position + offset;

        Vector3 cameraPos = GameObject.Find("Main Camera").transform.position;

        //This is the distance between player and camera in X, Y and Z
        Vector3 playerPos = new Vector3(GameObject.Find("Main Camera").transform.position.x, 
            gameObject.transform.position.y, gameObject.transform.position.z);
        gameObject.transform.position = playerPos;

        //To change camera distance just write trasnform.position.? +/- c
    }
}
