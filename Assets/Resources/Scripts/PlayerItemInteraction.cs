using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemInteraction : MonoBehaviour
{
    //Vector3 closestPoint = avoidanceObject.GetComponent<Collider>().ClosestPoint(target.transform.position);
    //public GameObject g;
    private bool isSpaceDown = false;
    private bool isCtrlDown = false;
    public GameObject target;
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isSpaceDown = true;
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCtrlDown = true;
        }
        //else
        //{
        //    isSpaceDown = false;
        //    isCtrlDown = false;
        //}
    }
    public List<Sprite> slicedSprites;
    void OnTriggerStay(Collider col)
    {
        
        if (col.name == "Karoj")
        {
            return;
        }
        target.transform.gameObject.SetActive(true);
        if (isSpaceDown)
        {
            isSpaceDown = false;
            //place down items
            if (FindChildByName(transform.parent.gameObject, "handitem").transform.childCount == 0 && FindChildByName(col.gameObject, "Item").transform.childCount != 0)
            {
                GameObject item = FindChildByName(col.gameObject, "Item").transform.GetChild(0).gameObject;
                item.transform.parent = FindChildByName(transform.parent.gameObject, "handitem").transform;
                item.transform.localPosition = new Vector3(0, 0, item.transform.position.z);
            }
            else if (FindChildByName(transform.parent.gameObject, "handitem").transform.childCount == 1 && FindChildByName(col.gameObject, "Item").transform.childCount == 0)
            {
                GameObject item = FindChildByName(transform.parent.gameObject, "handitem").transform.GetChild(0).gameObject;
                item.transform.parent = FindChildByName(col.gameObject, "Item").transform;
                item.transform.localPosition = Vector3.zero;
            }
            //put items into cooking pot
            if (FindChildByName(transform.parent.gameObject, "handitem").transform.GetChild(0).tag == "Sliced" && !FindChildByName(FindChildByName(col.gameObject, "Item"), "fazek").transform.GetComponent<Fazek>().isFull)
            {
                GameObject cookingPot = FindChildByName(FindChildByName(col.gameObject, "Item"), "fazek").gameObject;
                GameObject item = FindChildByName(transform.parent.gameObject, "handitem").transform.GetChild(0).gameObject;
                cookingPot.GetComponent<Fazek>().items.Add(item.gameObject);
                item.transform.parent = FindChildByName(cookingPot.gameObject, "Items").transform;
                item.transform.localPosition = new Vector3(-0.6f + cookingPot.GetComponent<Fazek>().items.Count * 0.3f, 0, -2.1f);
            }
            //soup to plate            
            else if (FindChildByName(FindChildByName(col.gameObject, "Item"), "fazek") != null && FindChildByName(FindChildByName(col.gameObject, "Item"), "fazek").GetComponent<Fazek>().cooked && FindChildByName(transform.parent.gameObject, "handitem").transform.GetChild(0).name == "plate")
            {
                GameObject fazek = FindChildByName(FindChildByName(col.gameObject, "Item"), "fazek");
                int tomatoes = 0, potatoes = 0, onions = 0, carrots = 0;
                foreach (var item in fazek.GetComponent<Fazek>().items)
                {
                    if (item.GetComponent<SpriteRenderer>().sprite.name == "tomato_sliced") tomatoes++;
                    else if (item.GetComponent<SpriteRenderer>().sprite.name == "potato_sliced") potatoes++;
                    else if (item.GetComponent<SpriteRenderer>().sprite.name == "carrot_sliced") carrots++;
                    else if (item.GetComponent<SpriteRenderer>().sprite.name == "onion_sliced") onions++;
                }

                if (tomatoes == 3)
                {
                    FindChildByName(transform.parent.gameObject, "handitem").transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/tomato_soup");
                    ResetOven(fazek);
                }
                else if (onions == 3)
                {
                    FindChildByName(transform.parent.gameObject, "handitem").transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/onion_soup");
                    ResetOven(fazek);
                }
                

                
                
            }
        }
        //slice
        else if (isCtrlDown && FindChildByName(col.gameObject, "vagodeszka") != null && FindChildByName(col.gameObject, "Item").transform.childCount == 1 && FindChildByName(col.gameObject, "Item").transform.GetChild(0).tag == "Sliceable")
        {
            isCtrlDown = false;
            GameObject item = FindChildByName(col.gameObject, "Item").transform.GetChild(0).gameObject;
            SpriteRenderer sr = item.transform.GetComponent<SpriteRenderer>();
            sr.sprite = slicedSprites.Find(x => x.name == sr.sprite.name + "_sliced");
            item.tag = "Sliced";
        }

        
        
    }

    private void OnTriggerExit(Collider other)
    {
        target.transform.gameObject.SetActive(false);
    }

    void ResetOven(GameObject fazek)
    {
        GameObject newGO = Instantiate(Resources.Load("Prefabs/oven")) as GameObject;
        newGO.transform.position = fazek.transform.parent.gameObject.transform.parent.gameObject.transform.position;
        Destroy(fazek.transform.parent.gameObject.transform.parent.gameObject);
    }

    public GameObject FindChildByName(GameObject parentGameObject, string name)
    {
        for (int i = 0; i < parentGameObject.transform.childCount; i++)
        {
            if (parentGameObject.transform.GetChild(i).name == name) return parentGameObject.transform.GetChild(i).gameObject;
        }

        return null;
    }
}
