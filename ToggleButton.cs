using UnityEngine;
using PVR.PSharp;

public class ToggleButton : PSharpBehaviour
{
	public GameObject objectToToggle;

    public override void OnInteract()
    {
        if (objectToToggle.activeSelf)
        {
            objectToToggle.SetActive(false);
        }
        else
        {
            objectToToggle.SetActive(true);
        }
    }


}