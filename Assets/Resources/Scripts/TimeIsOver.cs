using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class TimeIsOver : MonoBehaviour
{

    public TMP_Text currentPointsText;
    public TMP_Text bestPointsText;
    public TMP_Text finishedOrdersText;
    public TMP_Text lostOrdersText;

    void Start()
    {
        currentPointsText.text += GameController.points;
        if (GameController.points > GameController.bestPoints)
        {
            GameController.bestPoints = GameController.points;
            PlayerPrefs.SetInt($"{Menu.difficulty}bestPoints", GameController.bestPoints);
        }
        bestPointsText.text += PlayerPrefs.GetInt($"{Menu.difficulty}bestPoints");
        if (Menu.difficulty is 0)
        {
            bestPointsText.text += " (easy)";
            currentPointsText.text += " (easy)";
        }
        else if (Menu.difficulty is 1)
        {
            bestPointsText.text += " (normal)";
            currentPointsText.text += " (normal)";
        }
        else if (Menu.difficulty is 2)
        {
            bestPointsText.text += " (hard)";
            currentPointsText.text += " (hard)";
        }
        finishedOrdersText.text += GameController.finishedOrders;
        lostOrdersText.text += GameController.lostOrders;
    }

    public void Restart()
    {
        DOTween.Clear(true);
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
        DOTween.Clear(true);
        SceneManager.LoadScene("Menu");
    }
}
