using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leado : MonoBehaviour
{
    public GameObject item;
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerStay2D(Collider2D col)
    {
        if (transform.GetChild(0).transform.childCount is 1)
        {
            item = transform.GetChild(0).transform.GetChild(0).gameObject;
        }
        else item = null;


        if (item is not null && item.tag is "Soup")
        {
            Debug.Log("leado");
            item.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/plate_dirty");
            item.tag = "Dirty";
        }
    }

    
}
