using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class railgun : MonoBehaviour
{
    public GameObject bulletPrefab;
    private Rigidbody2D tower;

    public float atkspeed;
    public float range;
    public float damage;
    public float bspeed;
    public int targetmode = 0; //0 is near, 1 is first, 2 is last, 3 is strong

    private float t;

    private float T;

    private float rot;
    private List<Enemy> enemylist = new List<Enemy>();
    private Enemy target;
    private void Awake()
    {
        tower = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        t = atkspeed;
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        FindTarget();
        AimShot();
    }
    void FindTarget()
    {
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(this.transform.position, range);
        if (enemylist != null)
        {
            enemylist.Clear();
            target = null;
        }


        for (int collider2D = 0; collider2D < colliderArray.Length; collider2D++)
        {
            if (colliderArray[collider2D].TryGetComponent(out Enemy enemy))
            {
                enemylist.Add(enemy);
            }
        }
        if (targetmode == 0 && enemylist != null)
        {
            float MinDist = 10000;
            foreach (Enemy targets in enemylist)
            {
                float distance = (targets.transform.position - tower.transform.position).magnitude;
                if (distance < MinDist)
                {
                    target = targets;
                    MinDist = distance;
                }
            }
        }
        if (targetmode == 1 && enemylist != null)
        {
            float MinDist = 10000;
            foreach (Enemy targets in enemylist)
            {
                float distance = targets.transform.localScale.z;
                if (distance < MinDist)
                {
                    target = targets;
                    MinDist = distance;
                }
            }
        }
        if (targetmode == 2 && enemylist != null)
        {
            float MinDist = 0;
            foreach (Enemy targets in enemylist)
            {
                float distance = targets.transform.localScale.z;
                if (distance > MinDist)
                {
                    target = targets;
                    MinDist = distance;
                }
            }
        }


    }
    void AimShot()
    {
        if (target != null)
        {
            T = (target.transform.position - tower.transform.position).magnitude / bspeed;
            Vector2 ghost = T * target.velocity + target.transform.position;
            for (int i = 0; i < 4; i++)
            {
                T = (ghost - tower.position).magnitude / bspeed;
                ghost = T * target.velocity + target.transform.position;
            }
            rot = Mathf.Atan2((ghost.y - tower.transform.position.y), (ghost.x - tower.transform.position.x)) - Mathf.PI / 2;

            tower.transform.rotation = Quaternion.Euler(Vector3.forward * rot * Mathf.Rad2Deg);
        }
        if (t >= atkspeed && target != null)
        {
         
            GameObject bullet = Instantiate(bulletPrefab, gameObject.transform.position, Quaternion.Euler(Vector3.forward * (rot + Mathf.PI / 2) * Mathf.Rad2Deg));
            bullet.GetComponent<bullet>().SetDamage(damage);
            bullet.GetComponent<bullet>().setbspeed(bspeed);
            t = 0.0f;
        }

        t += Time.deltaTime;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
