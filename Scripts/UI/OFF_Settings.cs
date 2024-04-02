using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OFF_Settings : MonoBehaviour
{
    public GameObject canvas;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            canvas.SetActive(false);
        }
    }
}