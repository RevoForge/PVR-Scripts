using UnityEngine;
using PVR.PSharp;
using static RevoStaticMethodHolder.SharedByteArray;
using PVR.CCK.Worlds.Components;

public class Custompickuptest : PSharpBehaviour
{
	private bool pickup = false;
	public Vector3 rotationAdjustment;
    private PSharpPlayer player;

   
    public override void OnInteract()
	{
        player = PSharpPlayer.LocalPlayer;
        if (!Contains(player.PlayerID))
        {
            Add(player.PlayerID);
            pickup = true;
        }
        else
        {
            pickup = false;
            Remove(player.PlayerID);
            player = null;
        }
    }
    private void Update()
    {
        if (pickup)
        {
            Vector3 controllerEulerAngles = PSharpPlayer.GetRightControllerRotation().eulerAngles;
            Vector3 adjustedRotation = controllerEulerAngles + rotationAdjustment;

            transform.SetPositionAndRotation(PSharpPlayer.GetRightControllerPosition(), Quaternion.Euler(adjustedRotation));
        }

    }
}