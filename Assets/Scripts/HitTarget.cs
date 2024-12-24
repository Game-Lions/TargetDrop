using UnityEngine;

public class HitTarget : MonoBehaviour
{
    private GameObject target;
    // This method is called when another object enters the trigger collider
    private void OnCollisionEnter(Collision other)
    {
        if (this.target == null)
        {
            Debug.LogWarning("Target is null. Cannot calculate distance.");
            return;
        }
        float distanceFromTarget = Vector3.Distance(other.transform.position, target.transform.position);
        Debug.Log("Distance from target: " + distanceFromTarget + " km");
        GameManager.instance.TargetHit(distanceFromTarget);
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
