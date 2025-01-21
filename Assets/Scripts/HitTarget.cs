using System;
using UnityEngine;

public class HitTarget : MonoBehaviour
{
    // Keep track of the current target
    private GameObject target;
    [SerializeField] private MovePlayer movePlayerScript;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Spawner"))
        {
            if (this.target == null)
            {
                Debug.LogWarning("Target is null. Cannot calculate distance.");
                return;
            }
            float distanceFromTarget = Vector3.Distance(other.transform.position, target.transform.position);
            Debug.Log("Distance from target: " + distanceFromTarget + " km");
            GameManager.instance.TargetHit(distanceFromTarget);     // Send information to gameManager
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.PlaneCrash();
            //Debug.Log("Player crash!");
            //movePlayerScript.respawn();
        }
    }
    public void SetTarget(string targetName)
    {
        this.target = GameObject.Find(targetName);

        if (this.target == null)
        {
            Debug.LogWarning($"GameObject with name '{targetName}' not found.");
        }
        else
        {
            Debug.Log($"Target set to: {targetName}");
        }
    }
}
