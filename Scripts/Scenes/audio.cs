using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audio : MonoBehaviour
{
    public float squareHeight = 1f;
    public float squareWidth = 1f;
    public float squareDepth = 0.1f; // Adăugăm o adâncime mică pentru vizualizare
    public AudioSource audioSource;

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void OnDrawGizmosSelected()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            return;

        float squareSize = Mathf.Sqrt(audioSource.volume) * 2f;

        // Folosim dimensiunile customizabile pentru a calcula dimensiunile pătratului
        float customSquareWidth = squareWidth * squareSize;
        float customSquareHeight = squareHeight * squareSize;

        // Desenăm pătratul folosind dimensiunile customizabile
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(customSquareWidth, customSquareHeight, squareDepth));
    }
}
