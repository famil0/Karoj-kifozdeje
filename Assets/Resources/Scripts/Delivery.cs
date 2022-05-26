using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delivery : MonoBehaviour
{
    public GameObject item;    

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
            item.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/plate_dirty");
            item.tag = "Dirty";
            List<GameObject> orders = GameObject.Find("GameController").GetComponent<GameController>().orders;
            string itemName = item.GetComponent<SpriteRenderer>().sprite.name;
            foreach (var order in orders)
            {
                Debug.Log(order.gameObject.transform.Find("Animation").gameObject.transform.Find("Food").gameObject.transform.GetChild(0).name + " " + itemName);
                string orderFoodName = order.gameObject.transform.Find("Animation").gameObject.transform.Find("Food").gameObject.transform.GetChild(0).name; /*.Split("(")[0]*/;
                if (orderFoodName == itemName)
                {
                    order.GetComponent<SpriteRenderer>().color = Color.green;
                }
            }
        }
    }

    
}
