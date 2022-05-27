using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int points;
    public DateTime d;
    public int usableTime;
    public float time;
    public GameObject PointDigits;
    public GameObject TimeDigits;
    public Dictionary<GameObject, List<GameObject>> recipes = new Dictionary<GameObject, List<GameObject>>();
    public List<GameObject> orders = new List<GameObject>();

    void Start()
    {
        usableTime = 300;
        points = 0;
        d = DateTime.Now;
        time = 0;

        GameObject tomato = Resources.Load<GameObject>("Prefabs/Ingredients/tomato").gameObject;
        GameObject carrot = Resources.Load<GameObject>("Prefabs/Ingredients/carrot").gameObject;
        GameObject onion = Resources.Load<GameObject>("Prefabs/Ingredients/onion").gameObject;

        recipes.Add(Resources.Load<GameObject>("Prefabs/Foods/tomato_soup").gameObject, new List<GameObject>() { tomato, tomato, tomato });
        recipes.Add(Resources.Load<GameObject>("Prefabs/Foods/onion_soup").gameObject, new List<GameObject>() { onion, onion, onion });
        recipes.Add(Resources.Load<GameObject>("Prefabs/Foods/vegy_soup").gameObject, new List<GameObject>() { tomato, onion, carrot });
                
        orders.Add(NewOrder());
        orders.Add(NewOrder());
    }

    void Update()
    {
        if (time <= usableTime)
        {
            time += Time.deltaTime;
        }

        PointDigits.GetComponent<Digits>().SetDigits(points);
        TimeDigits.GetComponent<Digits>().SetDigits(int.Parse(Math.Floor(usableTime - time).ToString()));


        
        for (int i = 0; i < orders.Count; i++)
        {
            orders[i].transform.localPosition = Vector3.Lerp(orders[i].transform.localPosition, new Vector3(orders[i].transform.localPosition.x, 0.7f + (i + 1) * -0.55f, orders[i].transform.localPosition.z), 5 * Time.deltaTime);
                
        }
            

    }

    public GameObject NewOrder()
    {
        System.Random r = new System.Random();
        GameObject order = Instantiate(Resources.Load<GameObject>("Prefabs/Order"));
        order.transform.parent = Camera.main.transform.Find("Orders");
        order.transform.localPosition = new Vector3(0, 0.7f + Camera.main.transform.Find("Orders").childCount * -0.55f, 0);
        GameObject food = recipes.ElementAt(r.Next(0, recipes.Count)).Key;
        GameObject ingredients = order.transform.Find("Animation").Find("Ingredients").gameObject;
        float size = 2.8f;
        float offset = 0.3f;
        for (int i = 0; i < recipes[food].Count; i++)
        {
            GameObject ingredient = Instantiate(recipes[food][i], ingredients.transform.position - new Vector3(i * (offset * 1f), 0, 0.1f), ingredients.transform.localRotation);
            ingredient.transform.parent = ingredients.transform;
            ingredient.transform.localScale = new Vector3(size, size, 1);
        }


        food = Instantiate(food).gameObject;
        food.transform.parent = order.transform.Find("Animation").Find("Food").transform;
        food.transform.localPosition = new Vector3(0, 0, -0.1f);
        food.transform.localScale = new Vector3(size, size, 1);
        food.name = food.name.Split("(")[0];        

        return order;
    }
}
