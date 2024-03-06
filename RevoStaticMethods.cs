using UnityEngine;
using PVR.PSharp;
using UnityEngine.AI;
using System;

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
                randDir = transform.position + UnityEngine.Random.insideUnitSphere * dist;
            }
            while (!NavMesh.SamplePosition(randDir, out hit, dist, NavMesh.AllAreas) || Vector3.Distance(transform.position, hit.position) < minDist);
            return hit.position;
        }

        public static Vector3 CalculateRandomForwardPosition(Transform transform, float dist, float minDist)
        {
            Vector3 randDir;
            // Get the forward direction of the character
            Vector3 forwardDir = transform.forward;
            float forwardAngle = UnityEngine.Random.Range(-Mathf.PI / 2f, Mathf.PI / 2f);

            Vector3 randomForwardDirection = Quaternion.Euler(0f, forwardAngle * Mathf.Rad2Deg, 0f) * forwardDir;
            randDir = transform.position + randomForwardDirection * dist;

            if (NavMesh.SamplePosition(randDir, out NavMeshHit hit, dist, NavMesh.AllAreas) && Vector3.Distance(transform.position, hit.position) >= minDist)
            {
                //Debug.Log($"Forward Position Found for {transform.name}");
                return hit.position;
            }
            // If no valid forward position is found, attempt to find a valid backward position
            float backwardAngle = UnityEngine.Random.Range(Mathf.PI / 2f, Mathf.PI * 1.5f);

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

        public static GameObject ActivateFirstInactiveChild(Transform objectTransform)
        {
            // Iterate through the children of the old parent
            for (int i = 0; i < objectTransform.childCount; i++)
            {
                GameObject child = objectTransform.GetChild(i).gameObject;

                // Check if the child is inactive
                if (!child.activeSelf)
                {
                    // Set the child active
                    child.SetActive(true);
                    
                    // Break out of the loop after activating the first inactive child
                    return child;
                }
            }
            return null;
        }

    }
    public static class SharedByteArray
    {
        [PSharpSynced]
        private static readonly byte[] _data = new byte[16];
        private static int _head = 0; 

        public static void Add(byte value)
        {
            if (_head == _data.Length)
            {
                throw new IndexOutOfRangeException("Array is full.");
            }
            Debug.Log($"Player {value} added to the holding item array");
            _data[_head++] = value;
        }

        public static bool Contains(byte element)
        {
            for (int i = 0; i < _head; i++)
            {
                if (_data[i] == element)
                {
                    return true;
                }
            }
            return false;
        }

        public static void Remove(byte element)
        {
            int index = Array.IndexOf(_data, element, 0, _head);
            if (index >= 0)
            {
                RemoveAtIndex(index);
                Debug.Log($"Player {element} removed from the holding item array");
            }
        }

        private static void RemoveAtIndex(int index)
        {
            if (index >= 0 && index < _head)
            {
                Array.Copy(_data, index + 1, _data, index, _head - index - 1);
                _head--;
            }
        }
    }


}