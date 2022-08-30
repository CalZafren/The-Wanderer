using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState{
    idle, 
    walk,
    jump,
    attack,
    stagger
}

public class Player : MonoBehaviour
{

    public PlayerState currentState;
    private Rigidbody2D myRB;
    private BoxCollider2D boxCol;
    private Vector3 change;
    private Animator anim;
    public LayerMask ground;

    //Attributes
    public int speed;
    private int speedMultiplier = 5;
    public int jumpHeight;
    private int jumpMultiplier = 5;
    public int damage;

    //Check Values
    private bool flip = false;
    private bool jump = false;
    private bool isFacingRight = true;
    private bool firstAttack = false;
    private bool secondAttack = false;
    private bool attacking = false;


    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCol = GetComponent<BoxCollider2D>();
        currentState = PlayerState.idle;
    }

    // Update is called once per frame
    void Update()
    {
        MakeChecks();
        ChangePlayerState();
        MoveAndAnimate();
    }


    

    //This function makes all the checks for our character
    private void MakeChecks(){
        //Resets variables
        change = Vector3.zero;
        flip = false;
        jump = false;

        //Check for movement
        change.x = Input.GetAxisRaw("Horizontal");



        //Checks for Jumps
        if(Input.GetButtonDown("Jump")){
            jump = true;
        }


        //Checks if player needs to be flipped
        if(isFacingRight && change.x < 0){
            flip = true;
        }else if(!isFacingRight && change.x > 0){
            flip = true;;
        }

        //Checks for attacks
        if(Input.GetButtonDown("Attack") && IsGrounded()){
            firstAttack = true;
        }
    }




    
    //Handles functionality for switching the player state
    private void ChangePlayerState(){
        if(change == Vector3.zero && myRB.velocity.y == 0 && attacking == false){
            currentState = PlayerState.idle;
            return;
        }

        if(change != Vector3.zero && Mathf.Abs(myRB.velocity.y) < .001 && attacking == false){
            currentState = PlayerState.walk;
            return;
        }

        if(Mathf.Abs(myRB.velocity.y) > .001){
            currentState = PlayerState.jump;
            return;
        }

        if(attacking == true){
            currentState = PlayerState.attack;
            return;
        }
    }




    //This function actually makes our player do stuff based on the checks
    private void MoveAndAnimate(){

        //Flips player
        if(flip){
            Flip();
        }

        //Has player jump
        if(jump && IsGrounded()){
            Jump();
        }

        //Makes player attack
        if(firstAttack){
            StartCoroutine(Attack());
        }

        //Makes player walk and animate
        if(IsGrounded() && change != Vector3.zero && currentState != PlayerState.attack){
            myRB.velocity = new Vector2(speed * speedMultiplier * change.x, myRB.velocity.y);
            anim.SetBool("walking", true);
        }else if(change != Vector3.zero && currentState != PlayerState.attack){
            //Handles functionality for movement in air
            myRB.velocity = new Vector2((speed * speedMultiplier * change.x)/2, myRB.velocity.y);
        }else{
            //Should stop the players horizontal movement when the keys are released
            myRB.velocity = new Vector2(0, myRB.velocity.y);
            anim.SetBool("walking", false);
        }

        //Makes player animate jump and fall
        if(myRB.velocity.y > .001){
            anim.SetBool("jumping", true);
            anim.SetBool("falling", false);
        }else if(myRB.velocity.y < -.001){
            anim.SetBool("falling", true);
            anim.SetBool("jumping", false);
        }else{
            anim.SetBool("jumping", false);
            anim.SetBool("falling", false);
        }
    }

    private void Flip(){
        transform.Rotate(0, 180, 0);
        isFacingRight = !isFacingRight;
    }

    private void Jump(){
        myRB.velocity = new Vector2(myRB.velocity.x, jumpHeight * jumpMultiplier);
    }

    private IEnumerator Attack(){
        attacking = true;
        firstAttack = false;
        anim.SetBool("firstAttack", true);
        yield return new WaitForSeconds(.5f);
        anim.SetBool("firstAttack", false);
        yield return new WaitForSeconds(.5f);
        attacking = false;
    }

    private bool IsGrounded(){
        //Performs a box raycast and returns the bool value
        float extraHeight = .1f;
        RaycastHit2D rayCastHit = Physics2D.BoxCast(boxCol.bounds.center, boxCol.bounds.size, 0f, Vector2.down, extraHeight, ground);
        Color rayColor;
        if(rayCastHit.collider != null){
            rayColor = Color.green;
        }else{
            rayColor = Color.red;
        }

        //Draws the box cast
        Debug.DrawRay(boxCol.bounds.center - new Vector3(boxCol.bounds.extents.x, 0), Vector2.down * (boxCol.bounds.extents.y + extraHeight), rayColor);
        Debug.DrawRay(boxCol.bounds.center + new Vector3(boxCol.bounds.extents.x, 0), Vector2.down * (boxCol.bounds.extents.y + extraHeight), rayColor);
        Debug.DrawRay(boxCol.bounds.center - new Vector3(boxCol.bounds.extents.x, boxCol.bounds.extents.y + extraHeight), Vector2.right * boxCol.bounds.size.x, rayColor);

        return rayCastHit.collider != null;
    }
}
