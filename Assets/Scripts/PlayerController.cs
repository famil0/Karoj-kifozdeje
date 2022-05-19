using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int speed = 3;
    public SpriteRenderer sr;
    public Vector2 moveDir;
    public Animator anim;
    public GameObject cam;
    Rigidbody rb;
    
    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        moveDir = new Vector2(1, 0);
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float interpolation = speed * 1.5f * Time.deltaTime;
        cam.transform.position = new Vector3(Mathf.Lerp(cam.transform.position.x, transform.position.x, interpolation),
                                                Mathf.Lerp(cam.transform.position.y, transform.position.y, interpolation),
                                                -10);
       
        moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        rb.velocity = moveDir * speed;

        if (moveDir.x > 0)
        {
            sr.flipX = false;
            anim.SetTrigger("StartRunning");
        }
        else if (moveDir.x < 0)
        {
            sr.flipX = true;
            anim.SetTrigger("StartRunning");
        }
        else
        {
            anim.SetTrigger("StopRunning");
        }
    }
}