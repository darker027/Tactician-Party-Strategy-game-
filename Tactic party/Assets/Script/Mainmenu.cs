using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{
    public AudioMixer masterVolume;

    public void SetMasterVolume(float volume)
    {
        masterVolume.SetFloat("MasterVolume", volume);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
