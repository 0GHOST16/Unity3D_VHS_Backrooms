using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Video : MonoBehaviour
{
    public VideoPlayer VideoPlayer_i0;
    public GameObject Video_object_i0;
    public GameObject Video_canvas_i0;

    public VideoPlayer VideoPlayer_i1;
    public GameObject Video_object_i1;
    public GameObject Video_canvas_i1;

    public VideoPlayer VideoPlayer_i2;
    public GameObject Video_object_i2;
    public GameObject Video_canvas_i2;

    public VideoPlayer VideoPlayer_i3;
    public GameObject Video_object_i3;
    public GameObject Video_canvas_i3;

    void Start()
    {
        VideoPlayer_i0.loopPointReached += EndReached0;
        VideoPlayer_i1.loopPointReached += EndReached1;
        VideoPlayer_i2.loopPointReached += EndReached2;
        VideoPlayer_i3.loopPointReached += EndReached3;
    }

    void EndReached0(VideoPlayer vp)
    {
        Video_object_i0.SetActive(false);
        Video_canvas_i0.SetActive(false);

        Debug.Log("Video End");

        Video_object_i1.SetActive(true);
        Video_canvas_i1.SetActive(true);
    }

    void EndReached1(VideoPlayer vp)
    {
        Video_object_i1.SetActive(false);
        Video_canvas_i1.SetActive(false);

        Debug.Log("Video End");

        Video_object_i2.SetActive(true);
        Video_canvas_i2.SetActive(true);
    }

    void EndReached2(VideoPlayer vp)
    {
        Video_object_i2.SetActive(false);
        Video_canvas_i2.SetActive(false);

        Debug.Log("Video End");

        Video_object_i3.SetActive(true);
        Video_canvas_i3.SetActive(true);
    }

    void EndReached3(VideoPlayer vp)
    {
        Video_object_i3.SetActive(false);
        Video_canvas_i3.SetActive(false);

        Debug.Log("Video End");

        SceneManager.LoadScene(2);
    }
}
