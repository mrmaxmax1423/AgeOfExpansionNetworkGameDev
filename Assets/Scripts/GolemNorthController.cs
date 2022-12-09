using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;

public class GolemNorthController : NetworkBehaviour
{
    private Animator animator;
    public GameObject Golem;
    public GameObject Anchor;

    public GameObject golemFist;

    public int health = 10;

    private GameObject playerPoint;
    private Vector3 playerPointPos;

    private GameObject enemyPoint;

    public float playerDistance;
    public float anchorDistance;
    private float speed = 3.0f;

    public AudioSource recieveDamageSound;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerPoint = GameObject.Find("Player2Copy(Clone)");
        enemyPoint = GameObject.Find("Enemy");
    }
    public float timeLeft = 3f;
    void FixedUpdate()
    {

        //Controls Golem Following, Turning, and Attacking
        playerPointPos = new Vector3(playerPoint.transform.position.x, playerPoint.transform.position.y, playerPoint.transform.position.z);
        playerDistance = Vector3.Distance(playerPoint.transform.position, Golem.transform.position);
        anchorDistance = Vector3.Distance(playerPoint.transform.position, Anchor.transform.position);
        if(health > 0)
        {
            if (playerDistance > .95 && anchorDistance < 6.5)
            {
                transform.position = Vector3.MoveTowards(transform.position, playerPointPos, speed * Time.deltaTime);
                animator.SetBool("Moving", true);
            }

            if (playerDistance < 1 || anchorDistance > 6.5)
            {
                animator.SetBool("Moving", false);
            }
            if (playerDistance < 1.2 && animator.GetBool("Attacking") == false)
            {
                animator.SetBool("Attacking", true);
            }
            if (playerDistance > 1.2)
            {
                animator.SetBool("Attacking", false);
            }

            if (playerPoint.transform.position.x > Golem.transform.position.x)
            {
                Golem.transform.localScale = new Vector3(1, 1, 1);
            }

            if (playerPoint.transform.position.x < Golem.transform.position.x)
            {
                Golem.transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        
        if(health <= 0)
        {
            animator.SetBool("Alive", false);
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                Instantiate(golemFist, Golem.transform.position, Golem.transform.rotation);
                Golem.GetComponent<NetworkObject>().Despawn();
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "P1Weapon" || collider.gameObject.tag == "P2Weapon")
        {
            recieveDamageSound.Play();
            health -= 1;
        }
    }


}