using PVR.PSharp;
using UnityEngine;

public class MobDamage : PSharpBehaviour
{
    public int attackDamage = 10;
    private MobAITest parentMob;

    private void Start()
    {
        parentMob = FindMobAITestInParents(transform);
        if (parentMob == null)
        {
            Debug.Log("Finding the parent broke");
        }
    }
    public void PlayerKilled(PSharpPlayer player)
    {
        if (parentMob != null)
        {
            parentMob.PlayerKilled(player);
        }
        else
        {
            Debug.Log("Finding the parent broke");
        }
    }
    private MobAITest FindMobAITestInParents(Transform currentTransform)
    {
        // Check if the current transform is null
        if (currentTransform == null)
            return null;

        // Try to get the MobAITest component from the current transform
        MobAITest mobAI = (MobAITest)currentTransform.GetComponent(typeof(MobAITest));

        // If MobAITest component is found, return it
        if (mobAI != null)
            return mobAI;

        // If MobAITest component is not found, recursively search through parents
        return FindMobAITestInParents(currentTransform.parent);
    }
}