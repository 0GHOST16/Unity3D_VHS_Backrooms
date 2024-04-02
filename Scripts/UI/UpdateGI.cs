using UnityEngine;
using UnityEngine.Video;

public class UpdateGI : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Light videoLight;

    public int textureWidth = 256; // Lățimea texturii
    public int textureHeight = 256; // Înălțimea texturii
    public float updateInterval = 0.1f; // Actualizează culorile luminii la fiecare 0.1 secunde

    private RenderTexture renderTexture;
    private float lastUpdateTime;

    void Start()
    {
        renderTexture = new RenderTexture(textureWidth, textureHeight, 0);
        videoPlayer.targetTexture = renderTexture;
        lastUpdateTime = Time.time;
    }

    void Update()
    {
        if (Time.time - lastUpdateTime >= updateInterval)
        {
            RenderTexture scaledTexture = RenderTexture.GetTemporary(textureWidth / 2, textureHeight / 2); // Redimensionare textură
            Graphics.Blit(renderTexture, scaledTexture);

            Texture2D tex = new Texture2D(scaledTexture.width, scaledTexture.height, TextureFormat.RGB24, false);
            RenderTexture.active = scaledTexture;
            tex.ReadPixels(new Rect(0, 0, scaledTexture.width, scaledTexture.height), 0, 0);
            tex.Apply();

            Color[] pixels = tex.GetPixels();
            Color averageColor = Color.black;
            foreach (Color pixel in pixels)
            {
                averageColor += pixel;
            }
            averageColor /= pixels.Length;

            videoLight.color = averageColor;

            RenderTexture.ReleaseTemporary(scaledTexture); // Eliberare textură temporară

            lastUpdateTime = Time.time;
        }
    }
}
