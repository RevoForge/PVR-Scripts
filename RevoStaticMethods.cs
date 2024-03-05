using UnityEngine;
using PVR.PSharp;
using UnityEngine.AI;

public class RevoStaticMethodHolder : PSharpBehaviour
{ 
    public static class RevoStaticMethods
    {
        public static Vector3 CalculateRandomPosition(Transform transform, float dist, float minDist)
        {
            Vector3 randDir;
            NavMeshHit hit;
            do
            {
                randDir = transform.position + Random.insideUnitSphere * dist;
            }
            while (!NavMesh.SamplePosition(randDir, out hit, dist, NavMesh.AllAreas) || Vector3.Distance(transform.position, hit.position) < minDist);
            return hit.position;
        }

        public static Vector3 CalculateRandomForwardPosition(Transform transform, float dist, float minDist)
        {
            Vector3 randDir;
            // Get the forward direction of the character
            Vector3 forwardDir = transform.forward;
            float forwardAngle = Random.Range(-Mathf.PI / 2f, Mathf.PI / 2f);

            Vector3 randomForwardDirection = Quaternion.Euler(0f, forwardAngle * Mathf.Rad2Deg, 0f) * forwardDir;
            randDir = transform.position + randomForwardDirection * dist;

            if (NavMesh.SamplePosition(randDir, out NavMeshHit hit, dist, NavMesh.AllAreas) && Vector3.Distance(transform.position, hit.position) >= minDist)
            {
                //Debug.Log($"Forward Position Found for {transform.name}");
                return hit.position;
            }
            // If no valid forward position is found, attempt to find a valid backward position
            float backwardAngle = Random.Range(Mathf.PI / 2f, Mathf.PI * 1.5f);

            Vector3 randomBackwardDirection = Quaternion.Euler(0f, backwardAngle * Mathf.Rad2Deg, 0f) * forwardDir;
            randDir = transform.position + randomBackwardDirection * dist;

            if (NavMesh.SamplePosition(randDir, out hit, dist, NavMesh.AllAreas) && Vector3.Distance(transform.position, hit.position) >= minDist)
            {
                //Debug.Log($"Backward Position Found for {transform.name}");
                return hit.position;
            }
            // If no valid position is found in the forward or backward direction, return the character's current position as a fallback
            Debug.Log($"No Valid Nav Position Found for {transform.name}");
            return transform.position;
        }


    }

}