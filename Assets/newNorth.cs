using UnityEngine;

public class SpinRawImage : MonoBehaviour
{
    public Transform target;
    void Update() => transform.rotation = Quaternion.Euler(0, 0, target.eulerAngles.y);
}
