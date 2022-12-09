using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

public class GameManager : NetworkBehaviour {
    public Player playerPrefab;
    public Player player2Prefab;
    public GameObject spawnPoints;

    private int spawnIndex = 0;
    private List<Vector3> availableSpawnPositions = new List<Vector3>();

    public void Awake()
    {
        refreshSpawnPoints();
    }

    public override void OnNetworkSpawn()
    {
        if (IsHost)
        {
            SpawnPlayers();
        }
    }

    private void refreshSpawnPoints()
    {
        Transform[] allPoints = spawnPoints.GetComponentsInChildren<Transform>();
        availableSpawnPositions.Clear();
        foreach(Transform point in allPoints)
        {
            if(point != spawnPoints.transform)
            {
                availableSpawnPositions.Add(point.localPosition);
            }
        }
    }

    public Vector3 GetNextSpawnLocation()
    {
        var newPosition = availableSpawnPositions[spawnIndex];
        spawnIndex += 1;

        if(spawnIndex > availableSpawnPositions.Count - 1)
        {
            spawnIndex = 0;
        }

        return newPosition;
    }
    int playerCount = 0;
    private void SpawnPlayers()
    {
        Player playerSpawn;
        foreach (PlayerInfo pi in GameData.Instance.allPlayers)
        {
            if(playerCount == 0)
            {
            playerSpawn = Instantiate(playerPrefab, GetNextSpawnLocation(), Quaternion.identity);
            playerSpawn.GetComponent<NetworkObject>().SpawnAsPlayerObject(pi.clientId);
            }
            else
            {
                playerSpawn = Instantiate(player2Prefab, GetNextSpawnLocation(), Quaternion.identity);
                playerSpawn.GetComponent<NetworkObject>().SpawnAsPlayerObject(pi.clientId);
            }
            playerCount += 1;

        }
    }
}