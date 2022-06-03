using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour
{
    public Transform playMenu;
    public Transform settingsMenu;
    public GameObject resolutionDropDown;
    public GameObject difficultyDropDown;
    public Toggle fullscreen;
    public List<GameObject> buttons = new List<GameObject>();
    public float t = 0.7f;

    public static int difficulty;



    private void Start()
    {        
        playMenu.transform.localScale = Vector3.zero;
        settingsMenu.transform.localScale = Vector3.zero;
        
        resolutionDropDown.GetComponent<TMP_Dropdown>().value = resolutionDropDown.GetComponent<TMP_Dropdown>().options.FindIndex(option => option.text == $"{Screen.width}x{Screen.height}");

        difficulty = 1;
        difficultyDropDown.GetComponent<TMP_Dropdown>().value = difficulty;
    }

    public void PlayShow()
    {
        foreach (var button in buttons)
        {
            button.GetComponent<Button>().interactable = false;
        }
        MoveLeft();
        Show(playMenu);
    }

    public void PlayHide()
    {
        foreach (var button in buttons)
        {
            button.GetComponent<Button>().interactable = true;
        }
        MoveRight();
        Hide(playMenu);
    }

    public void SettingsShow()
    {
        foreach (var button in buttons)
        {
            button.GetComponent<Button>().interactable = false;
        }
        MoveLeft();
        Show(settingsMenu);
    }

    public void SettingsHide()
    {
        foreach (var button in buttons)
        {
            button.GetComponent<Button>().interactable = true;
        }
        MoveRight();
        Hide(settingsMenu);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void SetScreen()
    {
        int width = int.Parse(resolutionDropDown.transform.Find("Label").GetComponent<TMP_Text>().text.Split("x")[0]);
        int height = int.Parse(resolutionDropDown.transform.Find("Label").GetComponent<TMP_Text>().text.Split("x")[1]);
        Screen.SetResolution(width, height, fullscreen.isOn);
    }

    public void MoveLeft()
    {
        transform.DOLocalMoveX(-800, t);
    }

    public void MoveRight()
    {
        transform.DOLocalMoveX(0, t);
    }

    public void Show(Transform tr)
    {
        tr.DOScale(Vector3.one, t);
    }

    public void Hide(Transform tr)
    {
        tr.DOScale(Vector3.zero, t);
    }

    public void StartGame()
    {
        SceneManager.LoadSceneAsync("Game");
    }

    public void SetDifficulty()
    {
        difficulty = difficultyDropDown.GetComponent<TMP_Dropdown>().value;
    }
}
