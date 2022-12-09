using Unity.Netcode;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : NetworkBehaviour
{
    [SerializeField]
    public Text deathCounter, oreCounter, woodCounter;

    public NetworkVariable<Vector2> PositionChange = new NetworkVariable<Vector2>();

    public NetworkVariable<int> Health = new NetworkVariable<int>(5);
    public NetworkVariable<int> Deaths = new NetworkVariable<int>(0);
    public NetworkVariable<int> OreCount = new NetworkVariable<int>(0);
    public NetworkVariable<int> WoodCount = new NetworkVariable<int>(0);
    public TMPro.TMP_Text txtOreCount, txtWoodCount, txtDeathCount;

    private GameManager _gameMgr;
    private Camera _camera;
    private float movementSpeed = .175f;

    public static int oreCount;
    public static int woodCount;

    private Animator animator;
    public Rigidbody2D rb;
    private Vector2 moveDirection;

    public GameObject PlayerRef;
    public GameObject PlayerCanvas;
    public bool facingRight;

    public float attackCooldown = .5f;

    public Sprite lostHeart;
    public Sprite fullHeart;

    public GameObject respawnStatue1;
    public GameObject respawnStatue2;

    public GameObject swordHit;
    public GameObject heartContainer1;
    public GameObject heartContainer2;
    public GameObject heartContainer3;
    public GameObject heartContainer4;
    public GameObject heartContainer5;

    public AudioSource hitSound;

    // -----------------------
    // Behaviour
    // -----------------------
    public void Start()
    {

        txtOreCount.text = "Ore:";
        txtWoodCount.text = "Wood:";
        respawnStatue1 = GameObject.FindGameObjectWithTag("RespawnGuardian1");
        respawnStatue2 = GameObject.FindGameObjectWithTag("RespawnGuardian2");

    }

    void FixedUpdate() //physics calcs due to fixed rate
    {
        animator = GetComponent<Animator>();
        animatePlayer();

        if (attackCooldown <= 0)
        {
            animator.SetBool("Attack", false);
        }
        else
            attackCooldown -= Time.deltaTime;
    }

    public override void OnNetworkSpawn()
    {
        _camera = transform.Find("Camera").GetComponent<Camera>();
        _camera.enabled = IsOwner;
        if (!IsOwner)
        { return;
        }
        if (PlayerRef.gameObject.tag == "Player")
        {
            PlayerCanvas.SetActive(true);
        }
        if (PlayerRef.gameObject.tag == "Player2")
        {
            PlayerCanvas.SetActive(true);
        }

    }

    void Update()
    {
        if (IsOwner)
        {
            Vector2 results = CalcMovement();
            RequestPositionForMovementServerRpc(results);
            if (Input.GetKeyDown(KeyCode.Space))// && animator.GetBool("Attack") == false && attackCooldown <= 0)
            {
                RegisterHitServerRpc();
            }
            if (Input.GetButtonDown("Fire1"))
            {
            }
        }
        /*
        if (IsOwner)
        {
            float movement = Input.GetAxis("Horizontal");
            GetComponent<Rigidbody2D>().velocity = new Vector2(movement * movementSpeed, 0.0f);
        */
        if (!IsOwner || IsHost)
        {
            transform.Translate(PositionChange.Value * movementSpeed);
        }

    }
    float y_move;
    public float x_move;
    // -----------------------
    // Private
    // -----------------------
    public float hold;
    public Vector2 CalcMovement()
    {
        x_move = Input.GetAxis("Horizontal");
        y_move = Input.GetAxis("Vertical");
        hold = x_move;

        Vector2 moveVect = new Vector2(x_move, y_move);
        return moveVect;
    }

    public void animatePlayer()
    {
        UpdateUIValue();
        if (x_move != 0) //if player inputs a move, make sure they face the direction of movement
        {
            animator.SetBool("Moving", true);
            x_move = Input.GetAxis("Horizontal");
            if (hold == -1 && facingRight)
            {
                //swordHit.SetActive(false);
                facingRight = false;
            }
            if (hold == 1 && !facingRight)
            {
                facingRight = true;
            }
        }
        else
        {
            animator.SetBool("Moving", false);
        }
        if(facingRight)
        {
            PlayerRef.transform.localScale = new Vector3(6, 6, 1);
        }
        if (!facingRight)
        {
            PlayerRef.transform.localScale = new Vector3(-6, 6, 1);
        }

    }

    private void HostHandleBulletCollision(GameObject PlayerRef)
    {
        ulong ownerClientId = PlayerRef.GetComponent<NetworkObject>().OwnerClientId;
        Player otherPlayer = NetworkManager.Singleton.ConnectedClients[ownerClientId].PlayerObject.GetComponent<Player>();

        //Destroy(PlayerRef);
    }

    private void HostHandleDamageBoostPickup(Collider other)
    {

    }

    // -----------------------
    // Events
    // -----------------------

    public void OnCollisionEnter2D(Collision2D collision)
    {

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (IsOwner)
        {
            if (other.gameObject.tag == "OreDrop")
            {
                //oreCount += 1;
                //txtOreCount.text = "Ore: " + oreCount.ToString();
            }
            if (other.gameObject.tag == "WoodDrop")
            {
                //woodCount += 1;
                //txtWoodCount.text = "Wood: " + woodCount.ToString();
            }
            if (other.gameObject.tag == "EnemyWeapon" || other.gameObject.tag == "P1Weapon" || other.gameObject.tag == "P2Weapon")
            {
                HPUpdate();
            }
        }
        if (IsHost)
        {
            if(PlayerRef.gameObject.tag == "Player" && other.gameObject.tag == "P2Weapon")
            {
                Health.Value -= 1;
                HPUpdate();
            }
            if (PlayerRef.gameObject.tag == "Player2" && other.gameObject.tag == "P1Weapon")
            {
                Health.Value -= 1;
                HPUpdate();
            }
            if (other.gameObject.tag == "EnemyWeapon")
            {
                Health.Value -= 1;
                HPUpdate();
            }
            if (other.gameObject.tag == "OreDrop")
            {
                OreCount.Value += 1;
                Destroy(other.gameObject);
            }
            if (other.gameObject.tag == "WoodDrop")
            {

                WoodCount.Value += 1;
                Destroy(other.gameObject);
            }
        }



    }
    // -----------------------
    // RPC
    // -----------------------
    [ServerRpc]
    public void RegisterHitServerRpc(ServerRpcParams rpcParams = default)
    {
        animator.SetBool("Attack", true);
        attackCooldown = .5f;
    }

    [ServerRpc]
    void RequestPositionForMovementServerRpc(Vector2 posChange)
    {
        if (!IsServer && !IsHost) return;

        PositionChange.Value = posChange;
    }



    // -----------------------
    // Public
    // -----------------------
    void UpdateUIValue()
    {
        txtDeathCount.text = "Deaths: " + Deaths.Value.ToString();
        txtOreCount.text = "Ore: " + OreCount.Value.ToString();
        txtWoodCount.text = "Wood: " + WoodCount.Value.ToString();
    }

    void HPUpdate()
    {
        Image heartCont1;
        heartCont1 = heartContainer1.GetComponent<Image>();
        Image heartCont2;
        heartCont2 = heartContainer2.GetComponent<Image>();
        Image heartCont3;
        heartCont3 = heartContainer3.GetComponent<Image>();
        Image heartCont4;
        heartCont4 = heartContainer4.GetComponent<Image>();
        Image heartCont5;
        heartCont5 = heartContainer5.GetComponent<Image>();

        if (Health.Value == 5)
        {
            heartCont1.sprite = fullHeart;
            heartCont2.sprite = fullHeart;
            heartCont3.sprite = fullHeart;
            heartCont4.sprite = fullHeart;
            heartCont5.sprite = fullHeart;
        }
        if (Health.Value == 4)
        {
            heartCont1.sprite = fullHeart;
            heartCont2.sprite = fullHeart;
            heartCont3.sprite = fullHeart;
            heartCont4.sprite = fullHeart;
            heartCont5.sprite = lostHeart;
        }
        if (Health.Value == 3)
        {
            heartCont1.sprite = fullHeart;
            heartCont2.sprite = fullHeart;
            heartCont3.sprite = fullHeart;
            heartCont4.sprite = lostHeart;
            heartCont5.sprite = lostHeart;
        }
        if (Health.Value == 2)
        {
            hitSound.Play();
            heartCont1.sprite = fullHeart;
            heartCont2.sprite = fullHeart;
            heartCont3.sprite = lostHeart;
            heartCont4.sprite = lostHeart;
            heartCont5.sprite = lostHeart;
        }
        if (Health.Value == 1)
        {
            hitSound.Play();
            heartCont1.sprite = fullHeart;
            heartCont2.sprite = lostHeart;
            heartCont3.sprite = lostHeart;
            heartCont4.sprite = lostHeart;
            heartCont5.sprite = lostHeart;
        }
        if (Health.Value <= 0)
        {
            hitSound.Play();
            Health.Value = 5;
            HPUpdate();
            if (PlayerRef.gameObject.tag == "Player" && respawnStatue1 != null)
            {
                PlayerRef.transform.position = new Vector3(9.5f, -30.0f, 0.0f);
            }
            else if(PlayerRef.gameObject.tag == "Player" && respawnStatue1 == null)
            {
                GameObject otherPlayer = GameObject.FindGameObjectWithTag("Player2");
                Destroy(PlayerRef);
                Destroy(otherPlayer);
                var scene = NetworkManager.SceneManager.LoadScene("Game Over Player 2 Win", UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
            if (PlayerRef.gameObject.tag == "Player2" && respawnStatue2 != null)
            {
                PlayerRef.transform.position = new Vector3(9.5f, 12.6f, 0.0f);
            }
            else if (PlayerRef.gameObject.tag == "Player2" && respawnStatue2 == null)
            {
                GameObject otherPlayer = GameObject.FindGameObjectWithTag("Player");
                Destroy(otherPlayer);
                Destroy(PlayerRef);
                var scene = NetworkManager.SceneManager.LoadScene("Game Over Player 1 Win", UnityEngine.SceneManagement.LoadSceneMode.Single);              
            }
            Deaths.Value += 1;
        }

    }
}