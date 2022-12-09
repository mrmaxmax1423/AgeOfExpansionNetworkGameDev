using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public Sprite lostHeart;
    public Sprite fullHeart;
    public void Start()
    {
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    private float moveSpeed = 10f;
    private Animator animator;
    public Rigidbody2D rb;
    private Vector2 moveDirection;

    public GameObject Player;
    public bool facingRight;

    public int health = 3;
    private int deaths = 0;

    public static int oreCount;
    public static int woodCount;

    public bool hasGolemDrop;
    public bool hasRuneStone;
    
    [SerializeField]
    public Text oreCounter, woodCounter, oreWallet, woodWallet, deathCounter;


    public float moveY;
    public float moveX;

    public Transform golemFistIndicator;
    public Transform runeStoneIndicator;

    public Transform newGameTipsUI;
    public Transform craftingUI;

    public GameObject heartContainer1;
    public GameObject heartContainer2;
    public GameObject heartContainer3;

    public bool axeOwned = false;

    public AudioSource hitSound;

    public float attackCooldown = .5f;
    // Update is called once per frame

    void Update()
    {
        ProcessInputs();
        oreCounter.text = "Ore: " + oreCount;
        woodCounter.text = "Wood: " + woodCount;
        
    }

    void FixedUpdate() //physics calcs due to fixed rate
    {
        Move();

        if (moveX != 0) //if player inputs a move, make sure they face the direction of movement
        {
            animator.SetBool("Moving", true);
            if(moveX == -1 && facingRight)
            {
                Player.transform.localScale = new Vector3(-6, 6, 1);
                facingRight = false;
            }
            if(moveX == 1 && !facingRight)
            {
                Player.transform.localScale = new Vector3(6, 6, 1);
                facingRight = true;
            }
        }
        else
        {
            animator.SetBool("Moving", false);
        }

        if(attackCooldown <= 0)
        {
            animator.SetBool("Attack", false);
        }
        else
            attackCooldown -= Time.deltaTime;

    }


    void ProcessInputs()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");


        moveDirection = new Vector2(moveX, moveY).normalized;

        if (Input.GetKeyDown(KeyCode.Space) && animator.GetBool("Attack") == false && attackCooldown <= 0)
        {
            animator.SetBool("Attack", true);
            attackCooldown = .5f;
        }

        if (moveX != 0 && animator.GetBool("Attack") == false)
        {
            animator.SetFloat("XDirection", moveX);
        }

        //display crafting menu
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (craftingUI.gameObject.activeSelf)
            {
                craftingUI.gameObject.SetActive(false);
            }
            else
            {
                craftingUI.gameObject.SetActive(true);
            }
        }
        //toggle tips
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (newGameTipsUI.gameObject.activeSelf)
            {
                newGameTipsUI.gameObject.SetActive(false);
            }
            else
            {
                newGameTipsUI.gameObject.SetActive(true);
            }
        }
    }
    public String currentWoodBal;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "EnemyWeapon")//Enemy Hit
        {
            health -= 1;
            HPUpdate();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //item Pickup
        if (collision.gameObject.tag == "OreDrop")
        {
            Destroy(collision.gameObject);
            oreCount += 1;
        }
        if (collision.gameObject.tag == "WoodDrop")
        {
            Destroy(collision.gameObject);
            woodCount += 1;
        }
        if (collision.gameObject.tag == "GolemDrop")
        {
            Destroy(collision.gameObject);
            golemFistIndicator.gameObject.SetActive(true);
            hasGolemDrop = true;
        }
        if (collision.gameObject.tag == "RuneStone")
        {
            Destroy(collision.gameObject);
            runeStoneIndicator.gameObject.SetActive(true);
            hasRuneStone = true;
        }
    }

    void Move()
    {
        if (animator.GetBool("Attack") == true)
        {
            rb.velocity = new Vector2(0, 0);
        }
        else
        {
            rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        }
    }

    void StopAttack()
    {
        if (animator.GetBool("Attack"))
        {
            animator.SetBool("Attack", false);
        }
    }

    void HPUpdate()
    {
        Image heartCont1;
        heartCont1 = heartContainer1.GetComponent<Image>();
        Image heartCont2;
        heartCont2 = heartContainer2.GetComponent<Image>();
        Image heartCont3;
        heartCont3 = heartContainer3.GetComponent<Image>();

        if (health == 3)
        {
            heartCont1.sprite = fullHeart;
            heartCont2.sprite = fullHeart; 
            heartCont3.sprite = fullHeart;
        }
        if(health == 2)
        {
            hitSound.Play();
            heartCont1.sprite = lostHeart;
            heartCont2.sprite = fullHeart;
            heartCont3.sprite = fullHeart;
        }
        if (health == 1)
        {
            hitSound.Play();
            heartCont1.sprite = lostHeart;
            heartCont2.sprite = lostHeart;
            heartCont3.sprite = fullHeart;
        }
        if(health == 0)
        {
            hitSound.Play();
            health = 3;
            HPUpdate();
            Player.transform.position = new Vector3(9.5f,-30.0f,0.0f);
            deaths++;
            deathCounter.text = "Deaths: " + deaths;
        }

    }

}