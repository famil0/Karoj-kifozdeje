using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemInteraction : MonoBehaviour
{
    public bool isSpaceDown = false;
    public bool isCtrlDown = false;
    public bool canInteract = false;
    public GameObject target;
    private void Update()
    {
        if (canInteract)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("joy A button"))
            {
                isSpaceDown = true;
            }
            else if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetButtonDown("joy X button"))
            {
                isCtrlDown = true;
            }
        }

    }

    

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
            //pick up items
            if (FindChildByName(transform.parent.gameObject, "handitem").transform.childCount == 0 && FindChildByName(col.gameObject, "Item").transform.childCount != 0)
            {
                GameObject item = FindChildByName(col.gameObject, "Item").transform.GetChild(0).gameObject;
                if (item.tag == "Slicing") return;
                item.transform.parent = FindChildByName(transform.parent.gameObject, "handitem").transform;
                item.transform.localPosition = new Vector3(0, 0, item.transform.position.z);
            }
            //place down items
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
                else if (tomatoes == 1 && onions == 1 && carrots == 1)
                {
                    FindChildByName(transform.parent.gameObject, "handitem").transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/vegy_soup");
                    ResetOven(fazek);
                }




            }
        }
        //slice
        else if (isCtrlDown && col.gameObject.tag is "Vagodeszka" /*&& FindChildByName(col.gameObject, "Item").transform.childCount == 1*/ /*&& FindChildByName(col.gameObject, "Item").transform.GetChild(0).tag == "Sliceable"*/)
        {
            isCtrlDown = false;
            GameObject vagodeszka = col.gameObject;
            vagodeszka.GetComponent<Vagodeszka>().canSlice = true;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        canInteract = true;
    }

    private void OnTriggerExit(Collider other)
    {
        target.transform.gameObject.SetActive(false);
        canInteract = false;
    }

    void ResetOven(GameObject fazek)
    {
        GameObject newGO = Instantiate(Resources.Load("Prefabs/fazek")) as GameObject;
        newGO.transform.position = fazek.transform.position;
        newGO.transform.parent = fazek.transform.parent;
        newGO.transform.localScale = fazek.transform.localScale;
        newGO.name = $"{newGO.name.Split("(")[0]}";
        Destroy(fazek);
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
