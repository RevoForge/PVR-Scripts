using UnityEngine;
using PVR.PSharp;

public class MobTakeDamage : PSharpBehaviour
{
	private MobAITest mobAI;

    private void Start()
    {
        mobAI = (MobAITest)transform.parent.GetComponent(typeof(MobAITest));
        if ( mobAI == null)
        {
            Debug.Log($"{transform.parent.name} MobAI Not Found");

        }
    }
    // Send DMG to Mob AI
    public void IncomingDamage(int damage)
	{
		mobAI.DealtDamage(damage);
	}
    // Send Dodge to Mob AI
    public override void OnPlayerTriggerStay(PSharpPlayer player)
    {
        //Debug.Log($"{mobAI.transform.name} Trying To Dodge Player");
        mobAI.DodgePlayer(player.GetPosition());
    }
    // test method
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MobAttack"))
        {
            Debug.Log($"{mobAI.transform.name} Trying To Dodge Tagged Object");
            mobAI.DodgePlayer(other.transform.position);
        }
    }

}