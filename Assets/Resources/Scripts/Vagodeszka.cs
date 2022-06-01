using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vagodeszka : MonoBehaviour
{
    public GameObject statusBar;
    public float reqTimeToSlice;
    public float elapsedTime;
    public bool sliced;
    public bool slicing;
    public GameObject item;
    public bool canSlice;

    void Start()
    {
        statusBar = transform.GetChild(3).gameObject;
        
        SetVariables();
    }

    void SetVariables()
    {
        statusBar.SetActive(false);
        reqTimeToSlice = 1.8f;
        elapsedTime = -0.001f;
        sliced = false;
        slicing = false;
        canSlice = false;
    }

    void Update()
    {
        
        if (transform.GetChild(0).gameObject.transform.childCount == 1)
            item = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        else item = null;


        if (canSlice && sliced is false)
        {
            Slice();
            
        }
        else
        {
            slicing = false;         
        }

        if (item is not null)
        {
            if (item.tag == "Sliceable") sliced = false;
            else if (item.tag == "Sliced") sliced = true;
        }
        else sliced = false;

        
    }

    public void Slice()
    {        
        if (item.tag != "Sliceable" && item.tag != "Slicing") return;
        slicing = true;
        item.tag = "Slicing";
        statusBar.SetActive(true);
        GameObject statusBarFg = statusBar.transform.GetChild(0).GetChild(0).gameObject;
        statusBarFg.transform.localScale = new Vector3(elapsedTime / reqTimeToSlice, 1, 1);

        if (statusBarFg.transform.localScale.x >= 1)
        {
            sliced = true;
            GameObject tmp = Instantiate(Resources.Load<GameObject>($"Prefabs/Ingredients/{item.GetComponent<SpriteRenderer>().sprite.name}_sliced"));
            tmp.transform.parent = transform.Find("Item");
            tmp.transform.localScale = Vector3.one;
            tmp.transform.localPosition = new Vector3(0, 0, item.transform.localPosition.z);
            tmp.tag = "Sliced";
            Destroy(item.gameObject);
            item = tmp;
            item.name = item.name.Split("(")[0];    
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
        canSlice = false;
    }
}
