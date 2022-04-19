using UnityEngine;

public class DestroyOnAnimationEnd : MonoBehaviour
{
    // Method to  destroy current animation object when it has reached the end of its animation length
    private void Start()
    {
        Destroy(gameObject,GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        
    }
}
