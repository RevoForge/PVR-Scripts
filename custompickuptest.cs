using UnityEngine;
using PVR.PSharp;

public class Custompickuptest : PSharpBehaviour
{
	private bool pickup = false;
	public Vector3 rotationAdjustment;

	public override void OnInteract()
	{
		pickup = !pickup;
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