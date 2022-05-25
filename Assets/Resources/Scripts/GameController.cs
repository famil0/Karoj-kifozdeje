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

    void Start()
    {
        usableTime = 300;
        points = 0;
        d = DateTime.Now;
        time = 0;

        recipes.Add(Resources.Load<GameObject>("Prefabs/Foods/tomato_soup").gameObject, new List<GameObject>() { Resources.Load<GameObject>("Prefabs/Ingredients/tomato").gameObject, Resources.Load<GameObject>("Prefabs/Ingredients/tomato").gameObject, Resources.Load<GameObject>("Prefabs/Ingredients/tomato").gameObject });
        recipes.Add(Resources.Load<GameObject>("Prefabs/Foods/onion_soup").gameObject, new List<GameObject>() { Resources.Load<GameObject>("Prefabs/Ingredients/onion").gameObject, Resources.Load<GameObject>("Prefabs/Ingredients/onion").gameObject, Resources.Load<GameObject>("Prefabs/Ingredients/onion").gameObject });
        recipes.Add(Resources.Load<GameObject>("Prefabs/Foods/vegy_soup").gameObject, new List<GameObject>() { Resources.Load<GameObject>("Prefabs/Ingredients/tomato").gameObject, Resources.Load<GameObject>("Prefabs/Ingredients/onion").gameObject, Resources.Load<GameObject>("Prefabs/Ingredients/carrot").gameObject });

        NewOrder();
    }

    void Update()
    {
        if (time <= usableTime)
        {
            time += Time.deltaTime;
        }

        PointDigits.GetComponent<Digits>().SetDigits(points);
        TimeDigits.GetComponent<Digits>().SetDigits(int.Parse((usableTime - time).ToString().Split(",")[0]));
    }

    public void NewOrder()
    {
        System.Random r = new System.Random();
        GameObject order = Instantiate(Resources.Load<GameObject>("Prefabs/Order"), Camera.main.transform.position - new Vector3(6, -3.4f, -1), Camera.main.transform.localRotation);
        order.transform.parent = Camera.main.transform;
        GameObject food = recipes.ElementAt(r.Next(0, recipes.Count)).Key;
        GameObject ingredients = order.transform.Find("Ingredients").gameObject;
        float size = 2.8f;
        float offset = 0.3f;
        for (int i = 0; i < recipes[food].Count; i++)
        {
            GameObject ingredient = Instantiate(recipes[food][i], ingredients.transform.position - new Vector3(i * (offset * 1.2f), 0, 0.1f), ingredients.transform.localRotation);
            ingredient.transform.parent = ingredients.transform;
            ingredient.transform.localScale = new Vector3(size, size, 1);
        }


        food = Instantiate(food).gameObject;
        food.transform.parent = order.transform.Find("Food").transform;
        food.transform.localPosition = new Vector3(0, 0, -0.1f);
        food.transform.localScale = new Vector3(size, size, 1);
        food.name = food.name.Split("(")[0];
    }
}
