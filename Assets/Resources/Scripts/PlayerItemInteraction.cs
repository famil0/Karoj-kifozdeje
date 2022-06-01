using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemInteraction : MonoBehaviour
{
    public bool spaceDown = false;
    public bool ctrlDown = false;
    public bool canInteract = false;
    public GameObject handItem;
    public GameObject handItemSlot;

    private void Start()
    {
        handItemSlot = transform.parent.Find("handitem").gameObject;
    }

    private void Update()
    {
        if (handItemSlot.transform.childCount > 0)
        {
            handItem = handItemSlot.transform.GetChild(0).gameObject;
        }
        else
        {
            handItem = null;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("joy A button"))
        {
            spaceDown = true;
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetButtonDown("joy X button"))
        {
            ctrlDown = true;
        }

        if (canInteract is false)
        {
            ctrlDown = false;
            spaceDown = false;
        }
    }


    void OnTriggerStay2D(Collider2D col)
    {
        if (col.name is "Karoj")
        {
            return;
        }
        if (spaceDown)
        {
            spaceDown = false;
            //pick up items
            if (handItemSlot.transform.childCount is 0 && FindChildByName(col.gameObject, "Item").transform.childCount is not 0)
            {
                //if (FindChildByName(col.gameObject, "Item").transform.childCount > 0)
                //{
                    GameObject item = FindChildByName(col.gameObject, "Item").transform.GetChild(0).gameObject;
                    if (item.tag == "Slicing" || item.tag == "Washing") return;
                    item.transform.parent = FindChildByName(transform.parent.gameObject, "handitem").transform;
                    item.transform.localPosition = new Vector3(0, 0, item.transform.position.z);
                //}
            }
            //place down items
            else if (handItemSlot.transform.childCount is 1 && FindChildByName(col.gameObject, "Item").transform.childCount is 0)
            {
                GameObject item = FindChildByName(transform.parent.gameObject, "handitem").transform.GetChild(0).gameObject;
                item.transform.parent = FindChildByName(col.gameObject, "Item").transform;
                item.transform.localPosition = Vector3.zero;
            }
            //put items into cooking pot
            if (handItemSlot.transform.childCount > 0 && handItemSlot.transform.GetChild(0).tag == "Sliced" && FindChildByName(FindChildByName(col.gameObject, "Item"), "fazek") is not null && FindChildByName(FindChildByName(col.gameObject, "Item"), "fazek").transform.GetComponent<OvenCook>().isFull is false)
            {
                float offset = 0.3f;
                GameObject cookingPot = FindChildByName(FindChildByName(col.gameObject, "Item"), "fazek").gameObject;
                for (int i = 0; i < cookingPot.GetComponent<OvenCook>().allowedItems.Count; i++)
                {
                    if (cookingPot.GetComponent<OvenCook>().allowedItems[i].name == handItem.name) break;
                    if (i == cookingPot.GetComponent<OvenCook>().allowedItems.Count - 1) return;
                }
                GameObject item = FindChildByName(transform.parent.gameObject, "handitem").transform.GetChild(0).gameObject;
                cookingPot.GetComponent<OvenCook>().items.Add(item.gameObject);
                item.transform.parent = FindChildByName(cookingPot.gameObject, "Items").transform;
                item.transform.localPosition = new Vector3(-2 * offset + cookingPot.GetComponent<OvenCook>().items.Count * offset, 0, -2.1f);
            }
            //put items into cooking pan
            if (handItemSlot.transform.childCount > 0 && FindChildByName(FindChildByName(col.gameObject, "Item"), "pan") is not null && FindChildByName(FindChildByName(col.gameObject, "Item"), "pan").transform.GetComponent<PanBake>().isFull is false)
            {
                GameObject cookingPan = FindChildByName(FindChildByName(col.gameObject, "Item"), "pan").gameObject;
                for (int i = 0; i < cookingPan.GetComponent<PanBake>().allowedItems.Count; i++)
                {
                    if (cookingPan.GetComponent<PanBake>().allowedItems[i].name == handItem.name) break;
                    if (i == cookingPan.GetComponent<PanBake>().allowedItems.Count - 1) return;
                }
                GameObject item = FindChildByName(transform.parent.gameObject, "handitem").transform.GetChild(0).gameObject;
                cookingPan.GetComponent<PanBake>().items.Add(item.gameObject);
                item.transform.parent = FindChildByName(cookingPan.gameObject, "Items").transform;
                item.transform.localPosition = new Vector3(0, 0, -2.1f);
            }
            //soup to plate            
            else if (FindChildByName(FindChildByName(col.gameObject, "Item"), "fazek") != null && FindChildByName(FindChildByName(col.gameObject, "Item"), "fazek").GetComponent<PanBake>().baked && handItem.tag == "Clean" && FindChildByName(FindChildByName(col.gameObject, "Item"), "fazek").GetComponent<PanBake>().burned is false)
            {
                GameObject fazek = FindChildByName(FindChildByName(col.gameObject, "Item"), "fazek");
                GameObject plate = FindChildByName(handItemSlot, "plate").gameObject;
                int tomatoes = 0, potatoes = 0, onions = 0, carrots = 0;
                foreach (var item in fazek.GetComponent<PanBake>().items)
                {
                    if (item.GetComponent<SpriteRenderer>().sprite.name == "tomato_sliced") tomatoes++;
                    else if (item.GetComponent<SpriteRenderer>().sprite.name == "potato_sliced") potatoes++;
                    else if (item.GetComponent<SpriteRenderer>().sprite.name == "carrot_sliced") carrots++;
                    else if (item.GetComponent<SpriteRenderer>().sprite.name == "onion_sliced") onions++;
                }

                if (tomatoes == 3)
                {
                    handItemSlot.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/tomato_soup");
                    plate.tag = "Soup";
                    ResetOven(fazek);
                }
                else if (onions == 3)
                {
                    handItemSlot.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/onion_soup");
                    plate.tag = "Soup";
                    ResetOven(fazek);
                }
                else if (tomatoes == 1 && onions == 1 && carrots == 1)
                {
                    handItemSlot.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/vegy_soup");
                    plate.tag = "Soup";
                    ResetOven(fazek);
                }



            }
        }
        //slice
        else if (ctrlDown && col.gameObject.tag is "Vagodeszka")
        {
            ctrlDown = false;
            GameObject vagodeszka = col.gameObject;
            vagodeszka.GetComponent<Vagodeszka>().canSlice = true;
        }

        //wash
        else if (ctrlDown && col.gameObject.tag is "Sink")
        {
            ctrlDown = false;
            GameObject sink = col.gameObject;
            sink.GetComponent<Sink>().canWash = true;
        }

        //fire extingusher
        else if (ctrlDown && col.gameObject.transform.Find("Item").GetChild(0).tag == "Burning")
        {
            ctrlDown = false;
            GameObject fazek = col.gameObject.transform.Find("Item").GetChild(0).gameObject;
            handItem.GetComponent<FireExtinguisher>().fazek = fazek;
            handItem.GetComponent<FireExtinguisher>().fire = fazek.transform.Find("Fire").gameObject;
            handItem.GetComponent<FireExtinguisher>().canExtinguish = true;            
        }

        ctrlDown = false;
        spaceDown = false;

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name == "Karoj")
        {
            return;
        }
        //target.transform.gameObject.SetActive(true);
        canInteract = true;
        col.gameObject.GetComponent<SpriteRenderer>().color = new Color32(180, 180, 180, 255);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.name == "Karoj")
        {
            return;
        }
        canInteract = false;
        //target.transform.gameObject.SetActive(false);
        if (handItem is not null && handItem.tag is "FireExtinguisher")
        {
            handItem.GetComponent<FireExtinguisher>().SetVariables();
        }
        col.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    void ResetOven(GameObject fazek)
    {
        fazek.GetComponent<PanBake>().SetVariables();
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
