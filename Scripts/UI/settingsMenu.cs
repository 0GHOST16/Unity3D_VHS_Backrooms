using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class settingsMenu : MonoBehaviour
{
    public AudioMixer mainMixer;
    public TMP_Dropdown graphicsDropdown;
    public TMP_Dropdown resolutionDropdown;


    public void SetVolume(float volume)
    {
        mainMixer.SetFloat("volume", volume);
        Debug.Log("+ volume: " + volume);
    }

    public void SetGraphics(int selectedGraphicsIndex)
    {
        selectedGraphicsIndex = graphicsDropdown.value;

        if(selectedGraphicsIndex == 0)
        {
            QualitySettings.SetQualityLevel(2);
            Debug.Log("+ Performant");
        }

        if (selectedGraphicsIndex == 1)
        {
            QualitySettings.SetQualityLevel(1);
            Debug.Log("+ Balanced");
        }

        if (selectedGraphicsIndex == 2)
        {
            QualitySettings.SetQualityLevel(0);
            Debug.Log("+ High Fidelity");
        }
    }

    public void SetResolutin(int selectedResolutionsIndex)
    {
        selectedResolutionsIndex = resolutionDropdown.value;

        if (selectedResolutionsIndex == 0)
        {
            Screen.SetResolution(3840, 2160, true);
            Debug.Log("+ Resolution: 3840 X 2160");
        }

        if (selectedResolutionsIndex == 1)
        {
            Screen.SetResolution(2560, 1440, true);
            Debug.Log("+ Resolution: 2560 X 1440");
        }

        if (selectedResolutionsIndex == 2)
        {
            Screen.SetResolution(1920, 1080, true);
            Debug.Log("+ Resolution: 1920 X 1080");
        }

        if (selectedResolutionsIndex == 3)
        {
            Screen.SetResolution(1600, 900, true);
            Debug.Log("+ Resolution: 1600 X 900");
        }

        if (selectedResolutionsIndex == 4)
        {
            Screen.SetResolution(1280, 720, true);
            Debug.Log("+ Resolution: 1280 X 720");
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Settings()
    {
        // INSUFFICIENT TIME
    }

    public void Exit()
    {
        Application.Quit();
        Debug.Log("exit");
    }
}
