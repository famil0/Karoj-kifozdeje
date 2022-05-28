using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject settings;
    public List<GameObject> buttons = new List<GameObject>();

    private void Start()
    {
        settings.SetActive(false);
        settings.GetComponent<RectTransform>().transform.localScale = Vector3.zero;
    }
    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    public void SettingsShow()
    {
        foreach (var button in buttons)
        {
            button.GetComponent<Button>().interactable = false;
        }
        float t = 0.7f;
        GetComponent<RectTransform>().transform.DOLocalMoveX(-800, t);
        settings.GetComponent<RectTransform>().transform.DOScale(Vector3.one, t);
    }

    public void SettingsHide()
    {
        foreach (var button in buttons)
        {
            button.GetComponent<Button>().interactable = true;
        }
        float t = 0.7f;
        GetComponent<RectTransform>().transform.DOLocalMoveX(0, t);
        settings.GetComponent<RectTransform>().transform.DOScale(Vector3.zero, t);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
