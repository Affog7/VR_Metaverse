using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class AnimatorPlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    private Vector3 velocity;
    private CharacterController controller;
    private Animator animator;

    private Vector3 externalDirection = Vector3.zero;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        bool isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // Combine les inputs classiques et les commandes réseau
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 inputMove = transform.right * x + transform.forward * z;

        Vector3 move = inputMove + externalDirection;
        controller.Move(move * speed * Time.deltaTime);

        animator.SetFloat("Speed", move.magnitude);

        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Reset du mouvement externe à chaque frame
        externalDirection = Vector3.zero;
    }

     
}
