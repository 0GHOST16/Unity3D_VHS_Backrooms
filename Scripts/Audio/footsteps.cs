using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class footsteps : MonoBehaviour
{
    public float frequency = 10f;

    public UnityEvent onFootStep;

    float sin;

    bool isTriggered = false;

    void Update()
    {
        float inputMagnitude = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).magnitude;
        if (inputMagnitude > 0)
        {
            StartFootsteps();
        }
    }

    private void StartFootsteps()
    {
        sin = Mathf.Sin(Time.time * frequency);

        if (sin > 0.97f && isTriggered == false)
        {
            isTriggered = true;
            Debug.Log("Tic");
            onFootStep.Invoke();
        }
        else if (isTriggered == true && sin < -0.97f)
        {
            isTriggered = false;
        }
    }

}
