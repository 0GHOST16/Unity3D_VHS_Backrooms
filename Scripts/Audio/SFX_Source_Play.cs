using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_Source_Play : MonoBehaviour
{
    public AudioSource sursaAudio;
    private float _randomNumber;

    void Start()
    {
        sursaAudio = GetComponent<AudioSource>();
        StartCoroutine(RandomNumber());
    }

    private IEnumerator RandomNumber()
    {
        _randomNumber = Random.Range(0.01f, 10.0f);

        yield return new WaitForSeconds(_randomNumber);

        Debug.Log(_randomNumber);

        sursaAudio.Play();
    }
}
