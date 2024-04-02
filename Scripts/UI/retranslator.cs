using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Video;

public class retranslator : MonoBehaviour
{
    public VideoPlayer videoPlayerPrefab; 
    private VideoPlayer videoPlayerInstance;
    private AudioSource audioSource;

    void Start()
    {
        videoPlayerInstance = Instantiate(videoPlayerPrefab);
        videoPlayerInstance.Play();
        videoPlayerInstance.audioOutputMode = VideoAudioOutputMode.AudioSource; // modul de iesire audio pe AudioSource

        audioSource = videoPlayerInstance.GetComponent<AudioSource>(); // AudioSource-ul asociat VideoPlayer-ului

        if (audioSource == null) 
        {
            audioSource = videoPlayerInstance.gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0; // sunet stereo
        }
    }

    void Update()
    {
        // redarea
        if (videoPlayerInstance.isPrepared && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
