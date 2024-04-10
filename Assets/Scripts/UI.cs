using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private int stage, targetNum, targetColor, turnNum;
    [SerializeField] private bool gameOver;
    [SerializeField] private TextMeshProUGUI targetText, turnText, stageText;
    [SerializeField] private GameObject win, lose;
    

    void Start()
    {
        int savedStage = PlayerPrefs.GetInt("stage", 1);
        setStage(savedStage);
        stageText.text = "STAGE " + stage;

    }
    private void Update()
    {

    }
    public void UpdateStage(int num)
    {
        stage += num;
    }
    public void setStage(int num)
    {
        stage = num;
    }
    public int getStage()
    {
        return stage;
    }
    public void UpdateTarget(int target)
    {
        targetNum -= target;
        if(targetNum<0) targetNum = 0;
    }
    public void setTarget(int target)
    {
        targetNum = target;
    }
    public int getTarget()
    {
        return targetNum;
    }
    public void targetToText()
    {
        targetText.text = targetNum.ToString();
    }
    public void UpdateTurn(int turn)
    {
        turnNum -= turn;
    }
    public void setTurn(int turn)
    {
        turnNum = turn;
    }
    public int getTurn()
    {
        return turnNum;
    }
    public void turnToText()
    {
        turnText.text = "TURN " + turnNum.ToString();
    }
    public void setTargetColor(int color)
    {
        targetColor = color;
    }
    public int getTargetColor()
    {
        return targetColor;
    }
    public bool isGameOver()
    {
        return gameOver;
    }
    public void Lose()
    {
        StartCoroutine(SetGameOver());
    }
    IEnumerator SetGameOver()
    {
        yield return new WaitForSeconds(1f);
        lose.SetActive(true);
    }
    public void Win()
    {
        StartCoroutine(SetWin());
    }
    IEnumerator SetWin()
    {
        yield return new WaitForSeconds(1f);
        win.SetActive(true);
    }
}
