using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ZombieMove : MonoBehaviour
{
    // Start is called before the first frame update
    private bool hunt;
    private bool zombiemode = true;
    public int str_lvl;

    private float speed;
    private float base_speed;
    private float attack;
    public float health;

    public GameHandler gameHandler;
    private Transform target;
    private int framecount = 0;
    private Vector3 attack_location;
    private float angle;
    private float reinfect = 0.0f;
    //private float rate = 0.99f;
    private float daze = 0;

    public Sprite Human;
    public Sprite Zombie;
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rb2D;

    void Start()
    {
        zombify();
        target = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform> ();
        attack_location = transform.position;
        angle = Mathf.Atan2((transform.position.y - target.transform.position.y) *-1, (transform.position.x - target.transform.position.x)*-1) * Mathf.Rad2Deg -90f;
    }

    void zombify()
    {
        health = str_lvl*str_lvl * 2;
        base_speed = 1 / ((float)(str_lvl + 1));
    }

    // Update is called once per frame
    void Update()
    {
        //health = target.eulerAngles;
        //Random rnd = new Random();
        float dist1 = Vector3.Distance(transform.position, target.transform.position);

        spriteRenderer.sprite = Zombie;
        framecount = (framecount+1)%25;

         double DistToPlayer = Vector3.Distance(transform.position, attack_location);
            
            //if(dist1 > 15) attack_location = seek_victim();

        if(dist1 < 2)
        {
            attack_location = target.position;
            hunt = true;
        }

        if(dist1 < 10 && Input.GetMouseButtonDown(0))
        {
            attack_location = target.position;
            hunt = true;
        }

        if(DistToPlayer > 1.5f)
        {
            hunt = true;
        }

        if(hunt)
        {
            angle = Mathf.Atan2((transform.position.y - attack_location.y) *-1, (transform.position.x - attack_location.x)*-1) * Mathf.Rad2Deg -90f;
            speed = 6.0f * base_speed;
            if(DistToPlayer < 1)
            {
                hunt = false;
            }
        }
        else
        {
            speed = base_speed;
            if(framecount == 0) angle += (Math.Abs(angle)%11 - 5)*5;
        }

        if(daze == 0)
        {
            Vector3 hvMove = new Vector3((float)Math.Cos((angle + 90) / Mathf.Rad2Deg), (float)Math.Sin((angle + 90)/ Mathf.Rad2Deg), 0.0f);
            transform.position = transform.position + hvMove * speed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            daze --;
        }
    }
    
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        //float bounce = 6f; //amount of force to apply
        //rb2D.AddForce(collision.contacts[0].normal * bounce);
        //isBouncing = true;

        //angle += (Math.Abs(angle)%3 - 2)*45;
        //Vector3 hvMove = new Vector3((float)Math.Cos((angle + 90) / Mathf.Rad2Deg), (float)Math.Sin((angle + 90)/ Mathf.Rad2Deg), 0.0f);
        //transform.position = transform.position + hvMove * 2 * Time.deltaTime;


        if (collision.gameObject.tag == "Vaccine") 
        {
            health--;
            if(health <= 0)
            {
                Destroy(gameObject);
            }
        }
        if (collision.gameObject.tag == "Player" && zombiemode)
        {
            gameHandler.playerGetHit(str_lvl);
            daze = 100;
            Vector3 hvMove = new Vector3((float)Math.Cos((angle + 270) / Mathf.Rad2Deg), (float)Math.Sin((angle + 270)/ Mathf.Rad2Deg), 0.0f);
            transform.position = transform.position + hvMove * 5.0f * Time.deltaTime;
        }
    }

    private Vector3 seek_victim()
    {

        Transform respawn = GameObject.FindGameObjectWithTag ("Civilian").GetComponent<Transform> ();
        double dist_temp = Vector3.Distance(transform.position, respawn.transform.position);

        if(dist_temp <= 10) return respawn.transform.position;
        return attack_location;
    }
}