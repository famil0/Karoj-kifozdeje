using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sink : MonoBehaviour
{
    public GameObject statusBar;
    public float reqTimeToWash;
    public float elapsedTime;
    public bool washed;
    public bool washing;
    public GameObject item;
    public bool canWash;

    void Start()
    {
        statusBar = transform.GetChild(1).gameObject;

        SetVariables();
    }

    void SetVariables()
    {
        statusBar.SetActive(false);
        reqTimeToWash = 1.8f;
        elapsedTime = -0.001f;
        washed = false;
        washing = false;
        canWash = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.GetChild(0).gameObject.transform.childCount == 1)
            item = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        else item = null;

        if (canWash && washed is false)
        {
            washing = true;
            item.tag = "Washing";
            Wash();
        }
        else
        {
            washing = false;
        }

        if (item is not null)
        {
            if (item.tag == "Dirty") washed = false;
            else if (item.tag == "Clean") washed = true;
        }
    }

    public void Wash()
    {
        statusBar.SetActive(true);
        GameObject statusBarFg = statusBar.transform.GetChild(0).GetChild(0).gameObject;
        statusBarFg.transform.localScale = new Vector3(elapsedTime / reqTimeToWash, 1, 1);

        if (statusBarFg.transform.localScale.x >= 1)
        {
            washed = true;
            item.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Images/{item.GetComponent<SpriteRenderer>().sprite.name.Split("_")[0]}");
            item.tag = "Clean";
            SetVariables();
        }
        else if (statusBarFg.transform.localScale.x >= 1)
        {
            statusBarFg.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            elapsedTime += Time.deltaTime;
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        canWash = false;
    }
}
