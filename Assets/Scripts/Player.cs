using UnityEngine;

public class Player : MonoBehaviour
{
    // Setting player speed
    public float speed = 5.0f;

    private void Update()
    {
        // Transform position to left/right if a left/right input is receieved from the user
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            this.transform.position += Vector3.left * this.speed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            this.transform.position += Vector3.right * this.speed * Time.deltaTime;
        }
    }
   
}
