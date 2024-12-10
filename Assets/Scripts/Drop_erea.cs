using UnityEngine;

public class Drop_erea : MonoBehaviour
{
    public bool isInsideArea = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        isInsideArea = true;
        Debug.Log("Player entered the area");
       
    }

    void OnTriggerExit2D(Collider2D other)
    {
        isInsideArea = false;
        Debug.Log("Player exited the area");
    }
}
