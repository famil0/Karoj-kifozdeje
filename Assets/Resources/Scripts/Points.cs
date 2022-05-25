using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Points : MonoBehaviour
{
    
    public List<GameObject> numbers = new List<GameObject>();
    public int point;

    private void Start()
    {
        point = 0;
        for (int i = 0; i < 10; i++)
        {
            numbers.Add(Resources.Load<GameObject>($"Prefabs/{i}"));
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) AddPoints(1);
    }

    public void AddPoints(int c)
    {
        point += c;

        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }

        float offset = 0.3f;
        for (int i = 0; i < point.ToString().Length; i++)
        {
            Instantiate(numbers[int.Parse(point.ToString()[i]+"")], transform.position - new Vector3((point.ToString().Length - 1) / 2 * offset - i * offset  + (point.ToString().Length - 1) % 2 * (offset / 2), 0, 0.1f), transform.localRotation).transform.parent = transform;
        }
    }
}
