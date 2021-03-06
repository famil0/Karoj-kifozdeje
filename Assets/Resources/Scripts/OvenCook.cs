using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenCook : MonoBehaviour
{
    public List<GameObject> items = new List<GameObject>();
    public bool isFull;
    public bool showItems = false;
    public GameObject statusBar;
    public float reqTimeToCook;
    public float elapsedTime;
    public bool cooked;
    public bool cooking;
    public bool canCook;
    public Animator anim;
    public bool burned;
    public float warningTime;

    private void Start()
    {
        statusBar = transform.GetChild(1).gameObject;
        SetVariables();
    }

    public void SetVariables()
    {
        items.Clear();
        isFull = false;
        foreach (Transform item in transform.GetChild(0))
        {
            Destroy(item.gameObject);
        }
        statusBar.SetActive(false);
        reqTimeToCook = 0;
        elapsedTime = -0.001f;
        cooked = false;
        cooking = false;
        canCook = false;
        burned = false;
        warningTime = 0;
    }

    void Update()
    {
        if (PauseMenu.gameIsPaused) return;
        if (items.Count == 3) isFull = true;
        transform.GetChild(0).gameObject.SetActive(showItems);
        if (items.Count > 0)
        {
            reqTimeToCook = items.Count * 10;
            if (canCook && cooked is false && burned is false)
            {
                Cook();
            }
        }

        if (transform.parent.gameObject.transform.parent.gameObject.tag == "Oven")
        {
            canCook = true;
            if (items.Count > 0)
            {
                cooking = true;
            }
            else
            {
                cooking = false;
            }
        }
        else
        {
            canCook = false;
        }

        if ((cooked && cooking) || (elapsedTime >= reqTimeToCook && cooking) && burned is false)
        {
            anim.SetTrigger("Warning");
            warningTime += Time.deltaTime;
        }
        else
        {
            anim.SetTrigger("NoWarning");
            warningTime = 0;
        }

        if (warningTime >= 5 && burned is false)
        {
            tag = "Burning";
            statusBar.SetActive(false);
            burned = true;
            GameObject fire = Instantiate(Resources.Load<GameObject>("Prefabs/Fire"));
            fire.name = fire.name.Split("(")[0];
            fire.transform.parent = transform;
            fire.transform.localScale = Vector3.one;
            fire.transform.localPosition = new Vector3(0, 0, fire.transform.localPosition.z);
        }

        if (canCook is false)
        {
            cooking = false;
        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float offset = 0.3f;
        if (transform.position.x - offset <= mousePos.x && transform.position.x + offset >= mousePos.x &&
            transform.position.y - offset <= mousePos.y && transform.position.y + offset >= mousePos.y)
        {
            showItems = true;
        }
        else showItems = false;
    }

    public void Cook()
    {
        statusBar.SetActive(true);
        GameObject statusBarFg = statusBar.transform.GetChild(0).GetChild(0).gameObject;
        statusBarFg.transform.localScale = new Vector3(elapsedTime / reqTimeToCook, 1, 1);
        
        if (statusBarFg.transform.localScale.x >= 1 && items.Count == 3)
        {
            cooked = true;
        }
        else if (statusBarFg.transform.localScale.x >= 1)
        {
            statusBarFg.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            elapsedTime += Time.deltaTime;
        }
    }

    private void OnMouseOver()
    {
        showItems = true;
    }

    private void OnMouseExit()
    {
        showItems = false;
    }
}
