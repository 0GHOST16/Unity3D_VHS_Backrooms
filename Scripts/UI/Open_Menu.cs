using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Open_Menu : MonoBehaviour
{
    public GameObject menuObject;
    public GameObject menubuttonObject;
    private bool isActivated = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isActivated = !isActivated; // Inverseaza starea activa

            if (isActivated)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked; 
                Cursor.visible = false;
            }

            menuObject.SetActive(isActivated);
            menubuttonObject.SetActive(isActivated);
        }
    }
}
