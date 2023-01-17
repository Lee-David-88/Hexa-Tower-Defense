using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D bbody;
    private float bullet_speed;
    public float lifeTime;
    private float bulletDamage;
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
    private void OnTriggerEnter2D(Collider2D other) {
        
        Enemy p = other.GetComponent<Enemy>();
        if(p != null)
        {
            p.TakeDamage(bulletDamage);
        }
        else if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }

    } 
    // Update is called once per frame
    void Update()
    {
        
    }

}
