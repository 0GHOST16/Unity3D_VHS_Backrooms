using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Settings_menu : MonoBehaviour
{
    public GameObject definedButton;
    public GameObject canvas;

    void Start()
    {
        definedButton = this.gameObject;
    }

    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit Hit;

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out Hit) && Hit.collider.gameObject == gameObject)
            {
                canvas.SetActive(true);
            }
        }
    }
}
