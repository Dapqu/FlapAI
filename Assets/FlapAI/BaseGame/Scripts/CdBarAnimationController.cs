using UnityEngine;

public class CdBarAnimationController : MonoBehaviour
{
    private Animator animator;
    private float cooldownTime = 1f;
    private float lastTriggerTime;

    void Start() {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Check if enough time has passed since the last trigger
        if (Time.time - lastTriggerTime >= cooldownTime)
        {
            // Check if the space bar is pressed
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TriggerAnimation();

                // Update the last trigger time
                lastTriggerTime = Time.time;
            }
        }
    }

    private void TriggerAnimation()
    {
        // Check if the Animator component is assigned
        if (animator != null)
        {
            animator.SetTrigger("Shoot");
        }
    }
}
