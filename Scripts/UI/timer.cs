using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float startTime;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        float t = Time.time - startTime;

        // Calculare
        int hours = (int)(t / 3600);
        int minutes = (int)((t % 3600) / 60);
        int seconds = (int)(t % 60);
        int milliseconds = (int)((t * 1000) % 1000);

        // Formatarea
        string timerFormatted = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);

        timerText.text = timerFormatted;


    }
}
