using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemInteraction : MonoBehaviour
{

    public GameObject handItem;

    void OnCollisionEnter(Collision col)
    {
        if (handItem == null && col.gameObject.tag == "Box" && col.gameObject.transform.GetChild(0).childCount == 1 && Input.GetKey(KeyCode.Space))
        {
            handItem = col.gameObject.transform.GetChild(0).GetChild(0).gameObject;
            handItem.transform.parent = transform.GetChild(1).transform;
            handItem.transform.localPosition = new Vector3(0, 0, handItem.transform.position.z);
        }
        else if (handItem != null && col.gameObject.tag == "Box" && col.gameObject.transform.GetChild(0).childCount == 0 && Input.GetKey(KeyCode.Space))
        {
            handItem.transform.parent = col.gameObject.transform.GetChild(0).transform;
            col.gameObject.transform.GetChild(0).GetChild(0).transform.localPosition = Vector3.zero;
            handItem = null;
        }
    }
}
