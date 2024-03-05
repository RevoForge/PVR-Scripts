using UnityEngine;
using PVR.PSharp;

public class SpawnMob : PSharpBehaviour
{
	public float respawnTime = 30;
	private float respawnTimer = 0;
	public bool startRespawn = false;
	public bool testSpawn = false;

    
    public Color gizmoColor = Color.yellow;
    public float gizmoSize = 0.5f;

    
    void OnDrawGizmos()
    {
        // Set the color of the Gizmo
        Gizmos.color = gizmoColor;

        // Draw a wire sphere at the position of the GameObject
        Gizmos.DrawWireSphere(transform.position, gizmoSize);
    }
    
    private void Start()
    {
        SpawnFromPool();
    }
    public void SpawnFromPool()
    {
        int childCount = transform.childCount;
        if (childCount == 0)
            return; // No children to move

        // Create an array to store the indices of inactive children
        int[] inactiveIndices = new int[childCount];
        int inactiveCount = 0;

        // Iterate through the children and store the indices of inactive ones
        for (int i = 0; i < childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (!child.activeSelf)
            {
                inactiveIndices[inactiveCount++] = i;
            }
        }

        // Check if there are inactive children
        if (inactiveCount > 0)
        {
            // Choose a random index from the array of inactive indices
            int randomIndex = Random.Range(0, inactiveCount);
            int chosenIndex = inactiveIndices[randomIndex];

            // Move the chosen inactive child to the Parent location
            GameObject chosenChild = transform.GetChild(chosenIndex).gameObject;
            chosenChild.transform.localPosition = Vector3.zero;
            chosenChild.SetActive(true);
        }
    }
    private void Update()
    {
        if (startRespawn)
		{
			respawnTimer += Time.deltaTime;
			if (respawnTimer >= respawnTime)
			{
				startRespawn = false;
                SpawnFromPool();
                respawnTimer = 0;
			}
		}
		if (testSpawn)
		{
            testSpawn = false;
            SpawnFromPool();
        }
    }
	public void MobDied()
	{
		startRespawn = true;
	}
}