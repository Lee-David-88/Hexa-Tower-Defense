using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plasma_bullet : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D bbody;
    public float bullet_speed;
    public float lifeTime;
    public float damage;
    private float bulletDamage;
    private bool exploding;
    private int fuse;
    public void SetDamage(float damage)
    {
        bulletDamage = damage;
    }
    public void setbspeed(float bspeed)
    {
        bullet_speed = bspeed;
    }



    void Start()
    {
        bbody = gameObject.GetComponent<Rigidbody2D>();
        bbody.velocity = bbody.transform.right*bullet_speed;
        Destroy(gameObject,lifeTime);
        exploding = false;
        fuse = 20;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy p = other.GetComponent<Enemy>();
        if (p != null)
        {
            p.TakeDamage(bulletDamage*10);
            exploding = true;
        }
        else if (other.CompareTag("Wall"))
        {
            exploding = true;   
        }

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Enemy p = other.GetComponent<Enemy>();
        if (p != null)
        {
            p.TakeDamage(bulletDamage);
        }
    }

    void move()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (exploding)
        {
            bbody.velocity = bbody.transform.right * bullet_speed / 2;
            this.transform.localScale = this.transform.localScale + new Vector3(0.15f,0.15f,0f);
            fuse -= 1;

        }
        if (fuse <= 0)
        {
            Destroy(gameObject);
        }
    }

}
