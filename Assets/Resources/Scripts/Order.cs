using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order : MonoBehaviour
{
    public GameObject statusBar;
    public float timeToFinish;
    public float elapsedTime;
    public Animator anim;

    private void Start()
    {
        statusBar = transform.Find("StatusBar").gameObject;
        timeToFinish = 90;
    }

    void Update()
    {
        if (elapsedTime < timeToFinish) elapsedTime += Time.deltaTime;
        if (elapsedTime >= timeToFinish) Destroy(transform.gameObject);
        else if (elapsedTime >= timeToFinish - 7) anim.SetTrigger("TimeIsOver");

        GameObject statusBarFg = statusBar.transform.GetChild(0).GetChild(0).gameObject;
        statusBarFg.transform.localScale = new Vector3(1 - elapsedTime / timeToFinish, 1, 1);

        
    }
}
