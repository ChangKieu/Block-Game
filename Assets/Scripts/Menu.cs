using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public UI UIScript;
    public void Resume()
    {
        Time.timeScale = 1f;
    }
    public void Pause()
    {
        Time.timeScale = 0f;
    }
    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
    public void Reload()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextLevel()
    {

        PlayerPrefs.SetInt("unlock", PlayerPrefs.GetInt("unlock", 1) + 1);
        Time.timeScale = 1f;
        UIScript.UpdateStage(1);
        PlayerPrefs.SetInt("stage", UIScript.getStage());
        SceneManager.LoadScene("Game");
    }
}
