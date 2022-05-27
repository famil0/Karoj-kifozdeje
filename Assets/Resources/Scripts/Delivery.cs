using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Delivery : MonoBehaviour
{
    public GameObject item;
    public List<GameObject> orders;
    public GameController gameController;
    public GameObject backItem;
    public Digits digits;

    private void Start()
    {
        digits = transform.parent.Find("back_slot").Find("Digits").GetComponent<Digits>();
        backItem = transform.parent.Find("back_slot").GetChild(0).gameObject;
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        orders = gameController.orders;
    }

    public void Update()
    {
        digits.SetDigits(backItem.transform.childCount);
    }


    void OnTriggerStay2D(Collider2D col)
    {
        if (transform.parent.Find("delivery_slot").GetChild(0).childCount == 1)
        {
            item = transform.parent.Find("delivery_slot").GetChild(0).GetChild(0).gameObject;
        }
        else
        {
            item = null;
        }


        if (item is not null && item.tag is "Soup")
        {
            string itemName = item.GetComponent<SpriteRenderer>().sprite.name;
            foreach (var order in orders)
            {
                string orderFoodName = order.gameObject.transform.Find("Animation").gameObject.transform.Find("Food").gameObject.transform.GetChild(0).name;
                if (orderFoodName == itemName)
                {
                    order.GetComponent<Order>().Done();
                    Destroy(item.gameObject);
                    Sequence seq = DOTween.Sequence();
                    seq.SetDelay(3);
                    seq.OnComplete(() => GetBackPlate());
                    break;
                }
            }
        }

    }

    public void GetBackPlate()
    {
        GameObject newPlate = Instantiate(Resources.Load<GameObject>("Prefabs/Objects/plate"), backItem.transform.position, backItem.transform.localRotation);
        newPlate.name = newPlate.name.Split("(")[0];
        newPlate.transform.DOScale(Vector3.one, 0);
        newPlate.transform.parent = backItem.transform;
        newPlate.tag = "Dirty";
        newPlate.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/plate_dirty");
    }
}
