using UnityEngine;
using PVR.PSharp;
using System.Collections;

public class PlayerMeleeWeapon : PSharpBehaviour
{
	public int damage = 10;
    private MobTakeDamage currentTarget;
    private AudioSource audioSource;
    // Velocity variables
    private Transform swordTip;
    private Vector3[] previousPositions = new Vector3[15]; // Array to store previous positions
    private int frameCount = 0; 
    public float velocityThreshold = 0.05f; 
    private bool isOverThreshold = false; 
    private void Start()
	{
        audioSource = GetComponent<AudioSource>();
        swordTip = transform.GetChild(0);
	}
    private void OnTriggerEnter(Collider other)
    {
        if (isOverThreshold)
        {
            currentTarget = (MobTakeDamage)other.GetComponent(typeof(MobTakeDamage));
            if (currentTarget != null)
            {
                currentTarget.IncomingDamage(damage);
                audioSource.Play();
            }
            currentTarget = null;
        }

    }
    // in fixed update due to max fps setting
    private void FixedUpdate()
    {
        // Update previous positions array
        previousPositions[frameCount] = swordTip.position;

        // Check if current position is the same as the previous position
        if (frameCount > 0 && previousPositions[frameCount] == previousPositions[(frameCount - 1 + 15) % 15])
        {
            // Skip velocity calculation if positions are the same
            return;
        }

        // Calculate velocity
        Vector3 velocity = CalculateVelocity();

        // Check if velocity is over the threshold
        if (velocity.magnitude > velocityThreshold)
        {
            isOverThreshold = true;
        }
        else
        {
            isOverThreshold = false;
        }

        // Increment frame counter and wrap around if necessary
        frameCount = (frameCount + 1) % 15;
    }
    // Calculate velocity based on the distance traveled in the last 15 frames
    private Vector3 CalculateVelocity()
    {
        Vector3 totalDistance = Vector3.zero;
        for (int i = 1; i < previousPositions.Length; i++)
        {
            totalDistance += previousPositions[i] - previousPositions[i - 1];
        }
        // Average the distances over the last 15 frames
        Vector3 averageDistance = totalDistance / (previousPositions.Length - 1);

        // Calculate velocity (distance / time)
        // Assuming the time interval between frames is constant
        float timeInterval = Time.deltaTime * 15; // Time interval for 15 frames
        Vector3 velocity = averageDistance / timeInterval;
        return velocity;
    }
}