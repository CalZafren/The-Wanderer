using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GoblinState{
    idle, 
    walk,
    engaged,
    attack,
    stagger,
    dead
}

public class Goblin : Enemy
{

    public GameObject alertBox;
    private GoblinState currentState;
    private Vector3 change;
    private Rigidbody2D myRb;
    public Transform patrolStart;
    public Transform patrolEnd;
    private Transform currentTargetPoint;
    public bool isFacingRight;
    private float currentAttackPause;
    public float maxAttackPause;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        currentState = GoblinState.idle;
        myRb = GetComponent<Rigidbody2D>();
        currentTargetPoint = patrolStart;
        change = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        MakeChecks();
        ChangeState();
        UpdatePoint();
    }

      private void MakeChecks(){
        if(isFacingRight && change.x < 0){
            Flip();
        }else if(!isFacingRight && change.x > 0){
            Flip();
        }
    }

    private void Flip(){
        transform.Rotate(0, 180, 0);
        isFacingRight = !isFacingRight;
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            engaged = true;
            idle = false; 
            alertBox.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("Player")){
            engaged = false;
            idle = true;
            alertBox.SetActive(false);
        }
    }


    private void ChangeState(){
        if(idle){
            currentState = GoblinState.idle;
            anim.SetBool("move", false);
            anim.SetBool("attack1", false);
        }else if(moving){
            currentState = GoblinState.walk;
            anim.SetBool("move", true);
        }else if(staggered){
            currentState = GoblinState.stagger;
            anim.SetBool("move", false);
        }else if(attacking){
            currentState = GoblinState.attack;
            Debug.Log("Attacking");
            anim.SetBool("attack1", true);
        }else if(engaged){
            currentState = GoblinState.engaged;
        }else if(dead){
            currentState = GoblinState.dead;
            anim.SetBool("move", false);
        }
    }

    private void UpdatePoint(){
        if(Vector2.Distance(transform.position, patrolStart.position) < .5f){
            currentTargetPoint = patrolEnd;
        }else if(Vector2.Distance(transform.position, patrolEnd.position) < .5f){
            currentTargetPoint = patrolStart;
        }

        if(engaged){
            MoveToPoint(target);
        }else{
            MoveToPoint(currentTargetPoint);
        }

        

    }

    private void MoveToPoint(Transform point){


        //Stops the goblin once it gets close to the player
        if(Vector3.Distance(transform.position, target.transform.position) < 2.5f && engaged){
            myRb.velocity = Vector3.zero;
            moving = false;
            idle = true;
            StartCoroutine(Attack());
            return;
        }else{
            currentAttackPause = 0;
        }

        //Sets the velocity of change to point towards the new target
        if((point.position.x - transform.position.x) < 0){
            change.x = -1;
        }else if((point.position.x - transform.position.x) > 0){
            change.x = 1;
        }

        //Only run this code if the goblin is not engaged with the player
        if(engaged == false && dead == false){
            moving = true;
            idle = false;
            myRb.velocity = new Vector2((speed * speedMultiplier * change.x)/2, 0);
        }else if(engaged){
            moving = true;
            idle = false;
            myRb.velocity = new Vector2((speed * speedMultiplier * change.x)/1.5f, 0);
        }

    }

    IEnumerator Attack(){
        if(currentAttackPause <= 0){
            currentAttackPause = maxAttackPause;
            attacking = true;
            idle = false;
        }

        currentAttackPause -= Time.deltaTime;
        yield return new WaitForSeconds(1f);
        attacking = false;
    }
}
