using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemInteraction : MonoBehaviour
{

    public List<Sprite> slicedSprites;
    void OnCollisionEnter(Collision col)
    {
        if (FindChildByName(transform.gameObject, "handitem").transform.childCount == 0 && FindChildByName(col.gameObject, "Item").transform.childCount != 0 && Input.GetKey(KeyCode.Space))
        {
            GameObject item = FindChildByName(col.gameObject, "Item").transform.GetChild(0).gameObject;
            item.transform.parent = FindChildByName(transform.gameObject, "handitem").transform;
            item.transform.localPosition = new Vector3(0, 0, item.transform.position.z);
        }
        else if (FindChildByName(transform.gameObject, "handitem").transform.childCount == 1 && FindChildByName(col.gameObject, "Item").transform.childCount == 0 && Input.GetKey(KeyCode.Space))
        {
            GameObject item = FindChildByName(transform.gameObject, "handitem").transform.GetChild(0).gameObject;
            item.transform.parent = FindChildByName(col.gameObject, "Item").transform;
            item.transform.localPosition = Vector3.zero;
        }
        else if (Input.GetKey(KeyCode.LeftControl) && FindChildByName(col.gameObject, "vagodeszka") != null && FindChildByName(col.gameObject, "Item").transform.childCount == 1 && FindChildByName(col.gameObject, "Item").transform.GetChild(0).tag == "Sliceable")
        {
            GameObject item = FindChildByName(col.gameObject, "Item").transform.GetChild(0).gameObject;
            SpriteRenderer sr = item.transform.GetComponent<SpriteRenderer>();
            sr.sprite = slicedSprites.Find(x => x.name == sr.sprite.name + "_sliced");
            item.tag = "Sliced";
        }
    }

    private GameObject FindChildByName(GameObject parentGameObject, string name)
    {
        for (int i = 0; i < parentGameObject.transform.childCount; i++)
        {
            if (parentGameObject.transform.GetChild(i).name == name) return parentGameObject.transform.GetChild(i).gameObject;
        }

        return null;
    }
}
