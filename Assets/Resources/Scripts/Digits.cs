using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Digits : MonoBehaviour
{
    
    public List<GameObject> numbers = new List<GameObject>();
    public float offset;

    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            numbers.Add(Resources.Load<GameObject>($"Prefabs/Numbers/{i}"));
        }
        offset = 0.3f;
    }

    public void SetDigits(int value)
    {
        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }

        ;
        for (int i = 0; i < value.ToString().Length; i++)
        {
            var n = Instantiate(numbers[int.Parse(value.ToString()[i] + "")], transform.position - new Vector3((value.ToString().Length - 1) / 2 * offset - i * offset + (value.ToString().Length - 1) % 2 * (offset / 2), 0, 0.1f), transform.localRotation);
            n.transform.parent = transform;
            n.transform.localScale = transform.localScale;
        }
    }
}
