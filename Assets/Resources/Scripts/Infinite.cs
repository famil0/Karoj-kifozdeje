using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infinite : MonoBehaviour
{
    public GameObject item;
    public GameObject infinite;
    void Start()
    {
        item = transform.GetChild(0).gameObject;
        infinite = transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (item.transform.childCount == 0)
        {
            GameObject newItem = Instantiate(infinite);
            newItem.transform.parent = item.transform;
            newItem.transform.position = infinite.transform.position;
            newItem.transform.localScale = infinite.transform.localScale;
        }
    }
}
