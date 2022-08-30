using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{


    [HideInInspector]
    public Transform target;
    [HideInInspector]
    public Animator anim;
    private Collider2D myCol;
    public int health;
    public int speed;
    [HideInInspector]
    public float speedMultiplier = .1f;
    public bool staggered = false;
    public bool moving = false;
    public bool idle = true;
    public bool dead = false;
    public bool engaged = false;
    public bool attacking = false;


    // Start is called before the first frame update
    public virtual void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        myCol = GetComponent<Collider2D>();
        idle = true;
    }

    public void TakeDamage(int damage){
        health -= damage;
        Debug.Log(health);
        StartCoroutine(TakeHit());
    }

    IEnumerator TakeHit(){
        idle = false;
        staggered = true;
        //Waits until the sword makes physical contact
        yield return new WaitForSeconds(.25f);
        //Starts the animation
        anim.SetBool("takeHit", true);
        //Waits until the animation is done
        yield return new WaitForSeconds(.25f);
        anim.SetBool("takeHit", false);
        staggered = false;

         //Kills the enemy
        if(health <= 0){
            StartCoroutine(Die());
        }
    }

    IEnumerator Die(){
        Destroy(this.transform.GetChild(0).gameObject);
        idle = false;
        dead = true;
        anim.SetBool("die", true);
        myCol.enabled = false;
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

}
