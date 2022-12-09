using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class RespawnStatue : NetworkBehaviour
{
    public GameObject selfRef;
    public int health = 5;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "P1Weapon" || collider.gameObject.tag == "P2Weapon")
        {
            health -= 1;
            if (health == 0)
            {
                selfRef.GetComponent<NetworkObject>().Despawn();
            }
        }
    }
}
