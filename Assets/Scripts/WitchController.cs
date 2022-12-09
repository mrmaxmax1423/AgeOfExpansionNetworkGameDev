using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using Unity.Netcode;
using UnityEngine;

public class WitchController : NetworkBehaviour
{
    private Animator animator;
    public GameObject Witch;
    public GameObject Anchor;

    public GameObject potionAttack;
    public GameObject WitchProjectileSource;

    public int health = 10;

    private GameObject enemyPoint;
    private GameObject playerPoint;
    private Vector3 playerPointPos;
    private Vector3 witchPointPos;

    public float playerDistance;
    public float playerAnchorDistance;
    public float witchAnchorDistance;
    private float speed = 3.0f;

    public AudioSource recieveDamageSound;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerPoint = GameObject.Find("PlayerCopy(Clone)");
        enemyPoint = GameObject.Find("Enemy");
    }
    public float attackCooldown = 1.5f;
    float attackXVel;
    float attackYVel;
    void FixedUpdate()
    {

        //Controls Golem Following, Turning, and Attacking

        playerPointPos = new Vector3(playerPoint.transform.position.x, playerPoint.transform.position.y, playerPoint.transform.position.z);
        playerDistance = Vector3.Distance(playerPoint.transform.position, Witch.transform.position);
        playerAnchorDistance = Vector3.Distance(playerPoint.transform.position, Anchor.transform.position);
        witchAnchorDistance = Vector3.Distance(Witch.transform.position, Anchor.transform.position);

        if (health > 0)
        {
            if (playerDistance < 6 && attackCooldown == 1.5)//witch attacks if player is near
            {
                attackXVel = 0f;
                attackYVel = 0f;
                animator.SetBool("Attacking", true);
                if (playerPoint.transform.position.x - Witch.transform.position.x > 1f)
                {
                    attackXVel = 3f;
                }
                if (Witch.transform.position.x - playerPoint.transform.position.x > 1f)
                {
                    attackXVel = -3f;
                }
                if (playerPoint.transform.position.y - Witch.transform.position.y > 1f)
                {
                    attackYVel = 3f;
                }
                if (Witch.transform.position.y - playerPoint.transform.position.y > 1f)
                {
                    attackYVel = -3f;
                }
                GameObject newPotionAttack = Instantiate(potionAttack, Witch.transform.position, Witch.transform.rotation);
                newPotionAttack.GetComponent<Rigidbody2D>().velocity = new Vector2(attackXVel, attackYVel);
                if ((attackXVel == 0 && attackYVel == 0))//ensure false fires don't occur
                    Destroy(newPotionAttack);
            }

            if (playerDistance > 5 && playerDistance < 10)
            {
                transform.position = Vector3.MoveTowards(transform.position, playerPointPos, speed * Time.deltaTime);
                animator.SetBool("Moving", true);
            }
            else
                animator.SetBool("Moving", false);

            if (playerPoint.transform.position.x > Witch.transform.position.x)
            {
                Witch.transform.localScale = new Vector3(1, 1, 1);
            }

            if (playerPoint.transform.position.x < Witch.transform.position.x)
            {
                Witch.transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        if (animator.GetBool("Attacking") == true)
        {
                attackCooldown -= Time.deltaTime;
                if (attackCooldown <= 0)
                {
                    animator.SetBool("Attacking", false);
                    attackCooldown = 1.5f;
            }      
        }

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Weapon")
        {
            health -= 1;
        }
    }
}
