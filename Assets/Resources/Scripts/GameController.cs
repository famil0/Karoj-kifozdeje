using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameController : MonoBehaviour
{
    public static int points;
    public static int bestPoints;
    public static int finishedOrders;
    public static int lostOrders;

    public int nextOrder;
    public int usableTime;
    public float time;
    public Digits PointDigits;
    public Digits TimeDigits;
    public Dictionary<GameObject, List<GameObject>> recipes = new Dictionary<GameObject, List<GameObject>>();
    public List<GameObject> orders = new List<GameObject>();
    Sequence seq;
    public int timeOffset;



    void Start()
    {
        timeOffset = 10;
        
        PointDigits = Camera.main.transform.Find("PointDigits").GetChild(0).GetComponent<Digits>();
        TimeDigits = Camera.main.transform.Find("TimeDigits").GetChild(0).GetComponent<Digits>();
        usableTime = 300;
        points = 0;
       
        time = 0;
        
        GameObject tomato = Resources.Load<GameObject>("Prefabs/Ingredients/tomato").gameObject;
        GameObject carrot = Resources.Load<GameObject>("Prefabs/Ingredients/carrot").gameObject;
        GameObject onion = Resources.Load<GameObject>("Prefabs/Ingredients/onion").gameObject;
        GameObject cheese = Resources.Load<GameObject>("Prefabs/Ingredients/cheese").gameObject;
        GameObject meat = Resources.Load<GameObject>("Prefabs/Ingredients/meat_baked").gameObject;

        //soups
        recipes.Add(Resources.Load<GameObject>("Prefabs/Foods/tomato_soup").gameObject, new List<GameObject>() { tomato, tomato, tomato });
        recipes.Add(Resources.Load<GameObject>("Prefabs/Foods/onion_soup").gameObject, new List<GameObject>() { onion, onion, onion });
        recipes.Add(Resources.Load<GameObject>("Prefabs/Foods/vegy_soup").gameObject, new List<GameObject>() { tomato, onion, carrot });

        //burgers
        recipes.Add(Resources.Load<GameObject>("Prefabs/Foods/Burger").gameObject, new List<GameObject>() { meat });
        recipes.Add(Resources.Load<GameObject>("Prefabs/Foods/CheeseBurger").gameObject, new List<GameObject>() { meat, cheese });
        recipes.Add(Resources.Load<GameObject>("Prefabs/Foods/CheeseTomatoBurger").gameObject, new List<GameObject>() { meat, cheese, tomato });



        orders.Add(NewOrder());
    }

    void Update()
    {
        if (time <= usableTime)
        {
            time += Time.deltaTime;
        }
        else
        {
            DOTween.Clear(true);
            SceneManager.LoadScene("TimeIsOver");
        }

        PointDigits.SetDigits(points);
        TimeDigits.SetDigits(int.Parse(Math.Floor(usableTime - time).ToString()));


        

    }

    public void NewOrder(int seconds)
    {
        seq = DOTween.Sequence();
        seq.SetDelay(seconds);
        seq.OnComplete(() => orders.Add(NewOrder()));
        
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

        //nextOrder = new System.Random().Next(30 - Menu.difficulty * timeOffset, 41 - Menu.difficulty * timeOffset);
        nextOrder = 40 - Menu.difficulty * timeOffset;
        NewOrder(nextOrder);

        return order;
    }

    public void MoveOrders()
    {
        for (int i = 0; i < orders.Count; i++)
        {
            orders[i].transform.DOLocalMoveY(0.7f + (i + 1) * -0.55f, 0.8f);
        }
    }    
}
