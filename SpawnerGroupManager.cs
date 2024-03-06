using UnityEngine;
using PVR.PSharp;
using static RevoStaticMethodHolder.RevoStaticMethods;

public class SpawnerGroupManager : PSharpBehaviour
{
    private GameObject[] spawnGroups = new GameObject[16]; // Array to hold spawn groups
    private bool[] spawnGroupActive = new bool[16]; // Array to track if each spawn group is active
    private byte[] playerID = new byte[16]; // Array to hold player IDs

    public override void OnPlayerJoined(PSharpPlayer player)
    {
        byte playerId = player.PlayerID;
        Debug.Log($"{playerId} Joined Activate Mob Group");

        // Activate the first inactive spawn group
        GameObject firstInactiveSpawnGroup = ActivateFirstInactiveChild(transform);
        if (firstInactiveSpawnGroup != null)
        {
            int index = System.Array.IndexOf(spawnGroups, null); // Find the index to store the spawn group
            if (index != -1)
            {
                spawnGroups[index] = firstInactiveSpawnGroup;
                spawnGroupActive[index] = true;
                playerID[index] = playerId; // Store player ID for this spawn group
            }
        }
    }

    public override void OnPlayerLeft(PSharpPlayer player)
    {
        byte playerId = player.PlayerID;
        Debug.Log($"{playerId} Left remove Mob Group");

        // Find the spawn group associated with the leaving player and deactivate it
        for (int i = 0; i < spawnGroups.Length; i++)
        {
            if (playerID[i] == playerId && spawnGroupActive[i])
            {
                spawnGroups[i].SetActive(false);
                spawnGroups[i] = null;
                spawnGroupActive[i] = false;
                break; // Exit loop after deactivating the spawn group
            }
        }
    }


}