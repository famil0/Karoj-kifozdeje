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
            PlayerPrefs.SetInt("bestPoints", GameController.bestPoints);    
        }
        bestPointsText.text += PlayerPrefs.GetInt("bestPoints");
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
