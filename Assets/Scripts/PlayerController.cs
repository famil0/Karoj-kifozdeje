using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int speed = 5;
    public Animator animator;
    public SpriteRenderer sr;
    public Vector2 moveDir;
    public Animator anim;
    public GameObject cam;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        moveDir = new Vector2(1, 0);
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float interpolation = speed * 1.5f * Time.deltaTime;
        cam.transform.position = new Vector3(Mathf.Lerp(cam.transform.position.x, transform.position.x, interpolation),
                                                Mathf.Lerp(cam.transform.position.y, transform.position.y, interpolation),
                                                -10);
        animator.StartPlayback();
        var bottomRightDetectionPoint = transform.position - new Vector3(-0.35f, transform.localScale.x / 6.2f, 1);
        var bottomLeftDetectionPoint = transform.position - new Vector3(0.35f, transform.localScale.x / 6.2f, 1);
        RaycastHit2D bottomRightRay = Physics2D.Raycast(bottomRightDetectionPoint, moveDir, speed * Time.deltaTime);
        RaycastHit2D bottomLeftRay = Physics2D.Raycast(bottomLeftDetectionPoint, moveDir, speed * Time.deltaTime);
        RaycastHit2D middleRay = Physics2D.Raycast(transform.position - new Vector3(0, 0.5f, 0), moveDir, speed * Time.deltaTime);
        if (bottomRightRay.collider == null && bottomLeftRay.collider == null && middleRay.collider == null)
        {
            if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S))
            {
                moveDir = new Vector2(-0.7f, -0.7f);
                sr.flipX = true;
                animator.StopPlayback();
                Move();
            }
            else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W))
            {
                moveDir = new Vector2(-0.7f, 0.7f);
                sr.flipX = true;
                animator.StopPlayback();
                Move();
            }
            else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S))
            {
                moveDir = new Vector2(0.7f, -0.7f);
                sr.flipX = false;
                animator.StopPlayback();
                Move();
            }
            else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W))
            {
                moveDir = new Vector2(0.7f, 0.7f);
                sr.flipX = false;
                animator.StopPlayback();
                Move();
            }
            else if (Input.GetKey(KeyCode.W))
            {
                moveDir = new Vector2(0, 1);
                Move();
            }
            else if (Input.GetKey(KeyCode.S))
            {
                moveDir = new Vector2(0, -1);
                Move();
            }
            else if (Input.GetKey(KeyCode.D))
            {
                moveDir = new Vector2(1, 0);
                sr.flipX = false;
                animator.StopPlayback();
                Move();
            }
            else if (Input.GetKey(KeyCode.A))
            {
                moveDir = new Vector2(-1, 0);
                sr.flipX = true;
                animator.StopPlayback();
                Move();
            }
            
            
        }
        else
        {
            transform.position -= new Vector3(moveDir.x, moveDir.y, 0) / 1000;
        }
    }


    void Move()
    {
        transform.position += new Vector3(moveDir.x, moveDir.y, 0) * speed * Time.deltaTime;
    }
}
