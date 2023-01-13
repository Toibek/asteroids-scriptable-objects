using UnityEngine;

public class DestroyInvisible : MonoBehaviour
{
    private void OnBecameInvisible()
    {
        Debug.Log("Destroying cause invisible: " + gameObject.name);
        Destroy(gameObject);
    }
}
