using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Delivery : MonoBehaviour
{
    public GameObject item;
    public List<GameObject> orders;
    public GameObject gameController;

    private void Start()
    {
        gameController = GameObject.Find("GameController");
        orders = GameObject.Find("GameController").GetComponent<GameController>().orders;
    }


    void OnTriggerStay2D(Collider2D col)
    {
        if (transform.GetChild(0).transform.childCount is 1)
        {
            item = transform.GetChild(0).transform.GetChild(0).gameObject;
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
                    
                    //gameController.GetComponent<GameController>().OrdersMove();






                    item.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/plate_dirty");
                    item.tag = "Dirty";
                    break;
                }
            }
        }

    }
}
