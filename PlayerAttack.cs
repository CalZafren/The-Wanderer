using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    private PolygonCollider2D myCol;
    private Player playerScript;
    public bool firstAttack = true;

    // Start is called before the first frame update
    void OnEnable()
    {
        myCol = GetComponent<PolygonCollider2D>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    void OnTriggerStay2D(Collider2D other){
        //Triggered if target is within the collider and the player is in the attack state
        if(playerScript.currentState == PlayerState.attack && firstAttack == true && other.CompareTag("Enemy")){
            //Handles only calling this function once per hit
            firstAttack = false;
            Damage(other);  
        }else if(playerScript.currentState != PlayerState.attack){
            firstAttack = true;
        }
    }

    private void Damage(Collider2D other){
            other.GetComponent<Enemy>().TakeDamage(playerScript.damage);
    }
}
