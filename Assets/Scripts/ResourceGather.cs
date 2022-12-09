using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ResourceGather : NetworkBehaviour
{
    public GameObject oreDrop;
    public GameObject woodDrop;
    public GameObject runePillarDrop;
    public int health = 5;
    public int dropAmount = 1;
    Vector3 spawnVariation;

    public AudioSource miningSound;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "P1Weapon" || collider.gameObject.tag == "P2Weapon")
        {

            health -= 1;
            miningSound.Play();
            if (health == 0)
            {
                for (int i = 0; i < dropAmount; i++)
                {
                    spawnVariation = new Vector3(Random.Range(-.25f, .25f), Random.Range(-.25f, .25f), Random.Range(-.25f, .25f)); //helps avoids spawning entities ontop of eachother
                    DropSpawnServerRPC();
                    if (gameObject.tag == "RunePillar")
                    {
                        Instantiate(runePillarDrop, gameObject.transform.position + spawnVariation, gameObject.transform.rotation);
                    }
                }
                gameObject.GetComponent<NetworkObject>().Despawn();
            }
        }
    }

    [ServerRpc]
    public void DropSpawnServerRPC(ServerRpcParams rpcParams = default)
    {
        GameObject newDrop;
        if (gameObject.tag == "Ore")
        {
            newDrop = Instantiate(oreDrop, gameObject.transform.position + spawnVariation, gameObject.transform.rotation);
            newDrop.gameObject.GetComponent<NetworkObject>().SpawnWithOwnership(rpcParams.Receive.SenderClientId);
        }
        if (gameObject.tag == "Tree")
        {
            newDrop = Instantiate(woodDrop, gameObject.transform.position + spawnVariation, gameObject.transform.rotation);
            newDrop.gameObject.GetComponent<NetworkObject>().SpawnWithOwnership(rpcParams.Receive.SenderClientId);
        }
    }
}
