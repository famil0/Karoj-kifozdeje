using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vagodeszka : MonoBehaviour
{
    public GameObject statusBar;
    public float reqTimeToSlice;
    public float elapsedTime;
    public bool sliced = false;
    public bool slicing = false;
    public GameObject item;
    public bool canSlice = false;

    void Start()
    {
        statusBar = transform.GetChild(3).gameObject;
        statusBar.SetActive(false);
        reqTimeToSlice = 2.5f;
        elapsedTime = -0.001f;
    }

    void Update()
    {
        
        if (transform.GetChild(0).gameObject.transform.childCount == 1)
            item = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        else item = null;

        if (canSlice && sliced is false)
        {
            slicing = true;
            Slice();
            item.tag = "Slicing";
        }
        else
        {
            slicing = false;         
        }

        if (item.tag == "Sliceable") sliced = false;
        else if (item.tag == "Sliced") sliced = true;

        
    }

    public void Slice()
    {
        statusBar.SetActive(true);
        GameObject statusBarFg = statusBar.transform.GetChild(0).GetChild(0).gameObject;
        statusBarFg.transform.localScale = new Vector3(elapsedTime / reqTimeToSlice, 1, 1);

        if (statusBarFg.transform.localScale.x >= 1)
        {
            sliced = true;
            item.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Images/{item.GetComponent<SpriteRenderer>().sprite.name}_sliced");
            item.tag = "Sliced";
            ResetVagodeszka(transform.gameObject);
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

    private void OnTriggerExit(Collider other)
    {
        canSlice = false;
    }

    void ResetVagodeszka(GameObject vago)
    {
        GameObject newGO = Instantiate(vago);
        newGO.transform.position = vago.transform.position;
        newGO.name = $"{newGO.name.Split("(")[0]}";
        Destroy(vago);
    }
}
