using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energizer : MonoBehaviour
{
    // Start is called before the first frame update
    public float atkspeed;

    public GameObject bullets;
    GameObject target;


    public GameObject ghostimage;
    private Rigidbody2D enemy;
    private Rigidbody2D pbody;

    public float spread;
    public float range;
    public float damage;
    public float bspeed;

    private Vector2 playerV;
    private float thea;
    private float theta;
    private float t;

    public LineRenderer Predict;

    public float T;
    private Vector2 rejectv;

    public Vector2 acc;

    public float speedtest;
    private Vector2 s1,s2,s3,s4,s5,s6,s7,s8,s9,s0;

    public float rot;


    void aimshoot2(){
        T = (pbody.position-enemy.position).magnitude/bspeed;
        Vector2 ghost = T * pbody.velocity + pbody.position; //+acc*T*T/2 ;
        for(int i = 0; i<4; i++){
            T = (ghost-enemy.position).magnitude/bspeed;
            ghost = T * pbody.velocity + pbody.position;//+acc*T*T/2;
        }
        rot =Mathf.Atan2((ghost.y - enemy.transform.position.y), (ghost.x - enemy.transform.position.x))-Mathf.PI/2;
        
        enemy.transform.rotation = Quaternion.Euler(Vector3.forward * rot* Mathf.Rad2Deg);
        if(t>=atkspeed && (enemy.position-pbody.position).magnitude<=range)
        {
            GameObject bullet = Instantiate(bullets, gameObject.transform.position, Quaternion.Euler(Vector3.forward * (rot+Mathf.PI/2) * Mathf.Rad2Deg));
            Instantiate(ghostimage, ghost, Quaternion.identity);
            bullet.GetComponent<flame>().SetDamage(damage);
            bullet.GetComponent<flame>().setbspeed(bspeed);
            t =0.0f;
        }
        t += Time.deltaTime;
        Predict.SetPosition(0,gameObject.transform.position);
        Predict.SetPosition(1,ghost);
        
    }



    // Update is called once per frame
    void Start()
    {
        target = GameObject.FindWithTag("Enemy");
        enemy = gameObject.GetComponent<Rigidbody2D>();
        pbody = target.GetComponent<Rigidbody2D>();
    }
    void LateUpdate() {
        aimshoot2();
    }


}
