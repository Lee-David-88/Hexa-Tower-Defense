using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flame : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D bbody;
    public float bullet_speed;
    public float lifeTime;
    public float damage;
    private float bulletDamage;
    private float scale = 0.2f;
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
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        Enemy p = other.GetComponent<Enemy>();
        if (p != null)
        {
            p.TakeDamage(bulletDamage/scale);
        }
        else if (other.CompareTag("Wall")) 
        {
            Destroy(gameObject);
        }
    }
    void move()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        this.transform.localScale = new Vector3(scale, scale, scale);
        scale = scale + (float)0.003;
    }

}
