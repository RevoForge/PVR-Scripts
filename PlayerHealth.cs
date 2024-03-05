using UnityEngine;
using PVR.PSharp;
using System.Collections.Generic;

public class PlayerHealth : PSharpBehaviour
{
    public Transform respawn;
    public int startingHealthPool = 100;
    private int healthPool;
    private PSharpPlayer localPlayer;
    private MobDamage mobDMG;
    private AudioSource audioSource;
    public AudioClip audioClipHit;
    public AudioClip audioClipDeath;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        localPlayer = PSharpPlayer.LocalPlayer;
        healthPool = startingHealthPool;
    }
    private void OnTriggerEnter(Collider other)
    {
        mobDMG = (MobDamage)other.GetComponent(typeof(MobDamage));
        if (mobDMG != null)
        {
            healthPool -= mobDMG.attackDamage;
            audioSource.clip = audioClipHit;
            audioSource.Play();
            if (healthPool <= 0)
            {
                PlayerDeath();
            }
        }
        mobDMG = null;
    }

    private void Update()
    {
        transform.position = localPlayer.GetPosition();
    }

    private void PlayerDeath()
    {
        audioSource.clip = audioClipDeath;
        audioSource.Play();
        mobDMG.PlayerKilled(localPlayer);
        PSharpPlayer.Teleport(respawn.position, 0);
        healthPool = startingHealthPool;
    }
}