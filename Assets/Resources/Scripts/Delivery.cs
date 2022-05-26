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
            List<GameObject> orders = GameObject.Find("GameController").GetComponent<GameController>().orders;
            string itemName = item.GetComponent<SpriteRenderer>().sprite.name;
            foreach (var order in orders)
            {
                string orderFoodName = order.gameObject.transform.Find("Animation").gameObject.transform.Find("Food").gameObject.transform.GetChild(0).name; /*.Split("(")[0]*/;
                if (orderFoodName == itemName)
                {
                    order.transform.Find("Animation").GetComponent<Animator>().enabled = false;
                    order.transform.Find("Animation").Find("background").GetComponent<SpriteRenderer>().color = Color.green;
                    StartCoroutine(DestroyAfterSeconds(order, 0.3f));
                    orders.Remove(order);
                    break;
                }
            }
            item.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/plate_dirty");
            item.tag = "Dirty";
        }
    }


    public IEnumerator DestroyAfterSeconds(GameObject g, float t)
    {
        yield return new WaitForSeconds(t);
        Destroy(g);
    }
    
}
