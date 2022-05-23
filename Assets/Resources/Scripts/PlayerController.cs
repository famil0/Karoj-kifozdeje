using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int speed;
    public SpriteRenderer sr;
    public Vector2 moveDir;
    public Vector2 facingDir;
    public Animator anim;
    public GameObject cam;
    Rigidbody2D rb;
    public GameObject itemDetection;
    public bool isMoving = false;
    
    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        moveDir = new Vector2(0, -1);
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isMoving)
        {
            facingDir = moveDir;
        }
        float interpolation = speed * 1.5f * Time.deltaTime;
        cam.transform.position = new Vector3(Mathf.Lerp(cam.transform.position.x, transform.position.x, interpolation),
                                                Mathf.Lerp(cam.transform.position.y, transform.position.y, interpolation),
                                                -10);
        itemDetection.transform.position = new Vector3(Mathf.Floor(transform.position.x + facingDir.x), Mathf.Round(transform.position.y + facingDir.y) - 1, itemDetection.transform.position.z);
        moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        rb.velocity = moveDir * speed;

        
            if (moveDir.x > 0)
            {
                sr.flipX = false;
                anim.SetTrigger("RunLeftRight");
            }
            else if (moveDir.x < 0)
            {
                sr.flipX = true;
                anim.SetTrigger("RunLeftRight");
            }
            else if (moveDir.y < 0)
            {
                anim.SetTrigger("Down");
            }
            else if (moveDir.y > 0)
            {
              anim.SetTrigger("Up");
            }
            else if (facingDir.x != 0)
            {
                anim.SetTrigger("StandLeftRight");
            }
            else if (facingDir.y < 0)
            {
                anim.SetTrigger("StandDown");
            }
            else if (facingDir.y > 0)
            {
                anim.SetTrigger("StandUp");
            }

        if (moveDir.x != 0 || moveDir.y != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }
}