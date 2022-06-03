using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Order : MonoBehaviour
{
    public GameObject statusBar;
    public float timeToFinish;
    public float elapsedTime;
    public Animator anim;
    public GameController gameController;

    private void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        statusBar = transform.Find("Animation").Find("StatusBar").gameObject;
        timeToFinish = 100;
    }

    void Update()
    {
        if (elapsedTime < timeToFinish) elapsedTime += Time.deltaTime;
        if (elapsedTime >= timeToFinish)
        {
            Delete();
            GameController.lostOrders--;
            GameController.points -= 7;
        }
        else if (elapsedTime >= timeToFinish - 7) anim.SetTrigger("TimeIsOver");

        GameObject statusBarFg = statusBar.transform.GetChild(0).GetChild(0).gameObject;
        statusBarFg.transform.localScale = new Vector3(1 - elapsedTime / timeToFinish, 1, 1);

    }
    public void Done()
    {
        GameController.finishedOrders++;
        Sequence seq = DOTween.Sequence();
        transform.Find("Animation").GetComponent<Animator>().enabled = false;
        transform.Find("Animation").Find("background").GetComponent<SpriteRenderer>().color = Color.green;
        seq.Append(transform.DOLocalMoveX(-1.7f, 0.5f));
        seq.OnComplete( () => Delete() );

        if (elapsedTime <= timeToFinish * 0.66666f) GameController.points += 10;
        else GameController.points += 5;


    }

    public void Delete()
    {
        List<GameObject> orders = gameController.GetComponent<GameController>().orders;
        orders.Remove(transform.gameObject);
        Destroy(transform.gameObject);
        gameController.MoveOrders();
    }
}
