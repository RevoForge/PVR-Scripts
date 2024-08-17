using UnityEngine;
using PVR.PSharp;
using System.Collections.Generic;
using UnityEngine.UI;

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
    public Slider healthSlider;
    private bool localJoined = false;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        healthPool = startingHealthPool;
        healthSlider.maxValue = startingHealthPool;
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
    public override void OnPlayerJoined(PSharpPlayer player)
    {
        if (player == PSharpPlayer.LocalPlayer)
        {
            localPlayer = player;
            localJoined = true;
        }
    }
    public override void OnPlayerLeft(PSharpPlayer player)
    {
        if (player == PSharpPlayer.LocalPlayer)
        {
            localJoined = false;
        }
    }

    private void Update()
    {
        if (localJoined)
        {
            if (healthPool != healthSlider.value)
            {
                healthSlider.value = healthPool;
            }
            Quaternion headRotation = localPlayer.GetBoneRotation(HumanBodyBones.Head);
            Quaternion zeroedRotation = Quaternion.Euler(0, headRotation.eulerAngles.y, 0);
            Vector3 playerPosition = localPlayer.GetPosition();
            Vector3 adjustedPosition = new(playerPosition.x, playerPosition.y + 1, playerPosition.z);
            transform.SetPositionAndRotation(adjustedPosition, zeroedRotation);
        }

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