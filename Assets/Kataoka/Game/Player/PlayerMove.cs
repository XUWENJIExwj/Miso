using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed;
    float x;

    public Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        if(horizontal> 0.1)
        {
            x = speed;
        }else if(horizontal < -0.1)
        {
            x = -speed;
        }
        else
        {
            x = 0;
        }

        rb.velocity = new Vector2(x, rb.velocity.y);
    }
}
