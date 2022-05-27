using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delivery : MonoBehaviour
{
    public GameObject item;
    public List<GameObject> orders;

    private void Start()
    {
        orders = GameObject.Find("GameController").GetComponent<GameController>().orders;
    }

    IEnumerator MoveObject(Vector3 source, Vector3 target, float overTime)
    {
        float startTime = Time.time;
        while (Time.time < startTime + overTime)
        {
            transform.position = Vector3.Lerp(source, target, (Time.time - startTime) / overTime);
            yield return null;
        }
        transform.position = target;
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
                    order.transform.Find("Animation").GetComponent<Animator>().enabled = false;
                    order.transform.Find("Animation").Find("background").GetComponent<SpriteRenderer>().color = Color.green;
                    StartCoroutine(DestroyAndNewAfterSeconds(order, 0.3f));
                    orders.Remove(order);
                    item.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/plate_dirty");
                    item.tag = "Dirty";
                    break;
                }
            }
        }

    }


    public IEnumerator DestroyAndNewAfterSeconds(GameObject g, float t)
    {
        yield return new WaitForSeconds(t);
        Destroy(g);
        orders.Add(GameObject.Find("GameController").GetComponent<GameController>().NewOrder());
    }

}
