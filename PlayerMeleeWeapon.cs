using UnityEngine;
using PVR.PSharp;

public class PlayerMeleeWeapon : PSharpBehaviour
{
	public int damage = 10;
    private MobTakeDamage currentTarget;
    private AudioSource audioSource;
	private void Start()
	{
        audioSource = GetComponent<AudioSource>();
	}
    private void OnTriggerEnter(Collider other)
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