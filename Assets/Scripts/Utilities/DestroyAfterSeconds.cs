using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    [SerializeField] float seconds;

    // Simple method to destroy an animation object after a duration of being alive
    private void Start()
    {
        Destroy(gameObject, seconds);
    }
}
