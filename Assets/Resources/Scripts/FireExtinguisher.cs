using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExtinguisher : MonoBehaviour
{
    public GameObject statusBar;
    public float reqTimeToFinish;
    public float elapsedTime;
    public bool canExtinguish;
    public bool finished;
    public bool extinguishing;
    public GameObject fazek;
    public GameObject fire;

    void Start()
    {
        statusBar = transform.GetChild(0).gameObject;
        SetVariables();   
    }

    public void SetVariables()
    {
        statusBar.SetActive(false);
        reqTimeToFinish = 5;
        elapsedTime = -0.001f;
        finished = false;
        extinguishing = false;
        canExtinguish = false;
    }

    void Update()
    {
        if (finished is false && canExtinguish)
        {
            Extinguish();
        }

        if (finished)
        {
            Destroy(fire.gameObject);
            fazek.GetComponent<PanBake>().SetVariables();
            fazek.tag = "Untagged";
        }
    }

    public void Extinguish()
    {
        statusBar.SetActive(true);
        GameObject statusBarFg = statusBar.transform.GetChild(0).GetChild(0).gameObject;
        statusBarFg.transform.localScale = new Vector3(elapsedTime / reqTimeToFinish, 1, 1);

        if (statusBarFg.transform.localScale.x >= 1)
        {
            finished = true;
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
}
