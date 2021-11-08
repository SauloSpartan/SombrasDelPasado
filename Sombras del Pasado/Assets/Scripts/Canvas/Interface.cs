using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    //Variables
    private int menuSet = 0;
    private GameObject playerInterface;

    void Start()
    {
        playerInterface = GameObject.Find("Player Interface");
    }

    void Update()
    {
        ChangeMenu();
    }

    private void ChangeMenu()
    {
        if (menuSet == 0 && Input.GetKeyDown(KeyCode.Escape))
        {
            playerInterface.SetActive(false);
            menuSet = menuSet + 1;
        }
        else if (menuSet == 1 && Input.GetKeyDown(KeyCode.Escape))
        {
            playerInterface.SetActive(true);
            menuSet = menuSet - 1;
        }
    }
}
