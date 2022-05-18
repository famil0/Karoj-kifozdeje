using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int speed = 5;
    public Animator animator;
    SpriteRenderer sr;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        animator.StartPlayback();
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0, speed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(0, -speed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            sr.flipX = false;
            animator.StopPlayback();
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            sr.flipX = true;
            animator.StopPlayback();
            transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
        }
    }
}
