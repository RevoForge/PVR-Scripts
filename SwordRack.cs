using UnityEngine;
using PVR.PSharp;
using static RevoStaticMethodHolder.RevoStaticMethods;

public class SwordRack : PSharpBehaviour
{
	private GameObject currentActiveChild;
    public int itemMovementThreshold = 2;

	private void Start()
	{
		currentActiveChild = ActivateFirstInactiveChild(transform);
	}

    private void Update()
    {
		if (currentActiveChild != null)
		{
            // Check if the currentActiveChild has moved further than itemMovementThreshold away from transform.position
            if (Vector3.Distance(transform.position, currentActiveChild.transform.position) >= itemMovementThreshold)
            {
                Debug.Log($"Current active child is further than {itemMovementThreshold} away.");
                currentActiveChild = ActivateFirstInactiveChild(transform);
            }


        }
    }
}