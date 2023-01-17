using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rgbody;
    public float movespeed;
    public float t;
    private float hMove;
    private float vmove;
    public float health;
    void Start()
    {
        rgbody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void move()
    {
        hMove=Input.GetAxisRaw("Horizontal");
        vmove=Input.GetAxisRaw("Vertical");
        rgbody.velocity=new Vector2(hMove*movespeed,vmove*movespeed);
        //rgbody.velocity = new Vector2(rgbody.velocity.x - ((float)0.1 * rgbody.velocity.x), rgbody.velocity.y - ((float) 0.01 * rgbody.velocity.y));
    }
    void FixedUpdate()
    {
        move();
        t = Time.fixedDeltaTime;
        
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
    }


}
