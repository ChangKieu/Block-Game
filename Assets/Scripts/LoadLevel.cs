using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    [SerializeField] Button[] buttons;
    private void Awake()
    {

        int unlock = PlayerPrefs.GetInt("unlock", 1);
        if (buttons.Length < unlock)
        {
            Debug.Log(unlock);
        }
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
        for (int i = 0; i < unlock; i++)
        {
            buttons[i].interactable = true;
        }

    }
    public void LoadStage(int lv)
    {
        PlayerPrefs.SetInt("stage", lv);
        SceneManager.LoadScene("Game");
    }
    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}