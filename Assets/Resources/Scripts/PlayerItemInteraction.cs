using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
                GameObject item = FindChildByName(col.gameObject, "Item").transform.GetChild(0).gameObject;
                if (item.tag == "Slicing" || item.tag == "Washing") return;
                item.transform.parent = FindChildByName(transform.parent.gameObject, "handitem").transform;
                item.transform.localPosition = new Vector3(0, 0, item.transform.position.z);
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
                for (int i = 0; i < cookingPot.GetComponent<AllowedItems>().allowedItems.Count; i++)
                {
                    if (cookingPot.GetComponent<AllowedItems>().allowedItems[i].name == handItem.name) break;
                    if (i == cookingPot.GetComponent<AllowedItems>().allowedItems.Count - 1) return;
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
                for (int i = 0; i < cookingPan.GetComponent<AllowedItems>().allowedItems.Count; i++)
                {
                    if (cookingPan.GetComponent<AllowedItems>().allowedItems[i].name == handItem.name) break;
                    if (i == cookingPan.GetComponent<AllowedItems>().allowedItems.Count - 1) return;
                }
                GameObject item = FindChildByName(transform.parent.gameObject, "handitem").transform.GetChild(0).gameObject;
                cookingPan.GetComponent<PanBake>().items.Add(item.gameObject);
                item.transform.parent = FindChildByName(cookingPan.gameObject, "Item").transform;
                item.transform.localPosition = new Vector3(0, 0, -2.1f);
            }
            //soup to plate            
            else if (col.gameObject.transform.Find("Item").Find("fazek") is not null && col.gameObject.transform.Find("Item").Find("fazek").GetComponent<OvenCook>().cooked && handItem.tag is "Clean" && col.gameObject.transform.Find("Item").Find("fazek").GetComponent<OvenCook>().burned is false)
            {
                GameObject fazek = col.gameObject.transform.Find("Item").Find("fazek").gameObject;
                GameObject plate = handItemSlot.transform.Find("plate").gameObject;
                int tomatoes = 0, potatoes = 0, onions = 0, carrots = 0;
                foreach (var item in fazek.GetComponent<OvenCook>().items)
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
            //bread to plate
            else if (handItem is not null && col.gameObject.transform.Find("Item").Find("plate") is not null && col.gameObject.transform.Find("Item").Find("plate").tag is "Clean" && col.gameObject.transform.Find("Item").Find("plate").Find("Item") is not null && handItem.name is "Bread_sliced")
            {
                GameObject plate = col.gameObject.transform.Find("Item").Find("plate").gameObject;
                handItem.transform.parent = plate.transform.Find("Item");
                handItem.transform.localPosition = new Vector3(0, 0, handItem.transform.localPosition.z);
                plate.tag = "Burger";
            }
            //ingredients to burger
            else if (handItem is not null && handItem.tag is "Sliced" && col.gameObject.transform.Find("Item").Find("plate") is not null && col.gameObject.transform.Find("Item").Find("plate").Find("Item").Find("Bread_sliced") is not null)
            {
                GameObject plate = col.gameObject.transform.Find("Item").Find("plate").gameObject;
                GameObject burger = col.gameObject.transform.Find("Item").Find("plate").Find("Item").Find("Bread_sliced").gameObject;
                float offsetY = 0.015f;
                float offsetZ = 0.01f;
                foreach (var allowed in burger.GetComponent<AllowedItems>().allowedItems)
                {
                    if (handItem.name == allowed.name)
                    {
                        handItem.transform.parent = burger.transform.Find("Items");
                        burger.transform.Find("top").transform.DOLocalMoveY(burger.transform.Find("Items").childCount * offsetY, 0);
                        handItem.transform.DOLocalMove(new Vector3(0, (burger.transform.Find("Items").childCount - 1) * offsetY, 0.04f - burger.transform.Find("Items").childCount * offsetZ), 0);
                        plate.tag += allowed.name.Split("_")[0];
                    }
                }
            }

            //baked meat to burger
            else if (handItem is not null && handItem.name is "pan" && handItem.GetComponent<PanBake>().baked && col.gameObject.transform.Find("Item").Find("plate") is not null && col.gameObject.transform.Find("Item").Find("plate").Find("Item").Find("Bread_sliced") is not null)
            {
                ResetPan(handItem);
                float offsetY = 0.015f;
                float offsetZ = 0.01f;
                GameObject burger = col.gameObject.transform.Find("Item").Find("plate").Find("Item").Find("Bread_sliced").gameObject;
                GameObject meat = Instantiate(Resources.Load<GameObject>("Prefabs/Ingredients/Meat_baked"));
                meat.name = meat.name.Split("(")[0];
                meat.transform.DOLocalMove(Vector3.zero, 0);
                meat.transform.localScale = new Vector3(3.125f, 3.125f, 1);
                meat.transform.parent = burger.transform.Find("MeatSlot");
                burger.transform.Find("top").transform.DOLocalMoveY(burger.transform.Find("Items").childCount * offsetY, 0);
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
        fazek.GetComponent<OvenCook>().SetVariables();
    }

    void ResetPan(GameObject fazek)
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
