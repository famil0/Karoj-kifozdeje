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
    public GameObject settingsMenu;
    public GameObject screenResolution;
    public List<GameObject> buttons = new List<GameObject>();
    public float t = 0.7f;
    public Dictionary<string, int> resolutions = new Dictionary<string, int>()
    {
        { "1920x1080", 0 }, { "1280x720", 1 }, { "800x600", 2 }
    };



    private void Start()
    {
        settingsMenu.SetActive(false);
        settingsMenu.GetComponent<RectTransform>().transform.localScale = Vector3.zero;

        foreach (var res in resolutions)
        {
            screenResolution.GetComponent<TMP_Dropdown>().options.Add(new TMP_Dropdown.OptionData(text: res.Key));
        }
        screenResolution.transform.Find("Label").GetComponent<TMP_Text>().text = resolutions.ElementAt(0).Key;
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
        GetComponent<RectTransform>().transform.DOLocalMoveX(-800, t);
        settingsMenu.GetComponent<RectTransform>().transform.DOScale(Vector3.one, t);
    }

    public void SettingsHide()
    {
        foreach (var button in buttons)
        {
            button.GetComponent<Button>().interactable = true;
        }
        GetComponent<RectTransform>().transform.DOLocalMoveX(0, t);
        settingsMenu.GetComponent<RectTransform>().transform.DOScale(Vector3.zero, t);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void SetScreen()
    {
        int width = int.Parse(resolutions.ElementAt(screenResolution.GetComponent<TMP_Dropdown>().value).Key.Split("x")[0]);
        int height = int.Parse(resolutions.ElementAt(screenResolution.GetComponent<TMP_Dropdown>().value).Key.Split("x")[1]);
        Screen.SetResolution(width, height, true);
    }
}
