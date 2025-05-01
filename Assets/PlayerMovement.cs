using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    public float mouseSensitivity = 100f;

    private CharacterController controller;
    private Animator animator;
    private Vector3 velocity;
    private float yRotation = 0f;

    // Mouvements via WebSocket
    private Vector3 externalMovement = Vector3.zero;
    private float externalRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // Verrouille la souris pour le jeu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        // 🖱️ Rotation souris (gauche-droite)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        yRotation += mouseX + externalRotation;
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
        externalRotation = 0f; // reset

        // 🧍‍♂️ Déplacement clavier + WebSocket
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 inputMove = transform.right * x + transform.forward * z;
        Vector3 move = inputMove + externalMovement;

        controller.Move(move * moveSpeed * Time.deltaTime);
        externalMovement = Vector3.zero; // reset

        // 🎞️ Animation (Idle / Walk / Run)
        if (animator != null)
            animator.SetFloat("Speed", move.magnitude);

        // 🧷 Gravité et saut
        bool isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // ========== Méthodes publiques appelables via WebSocket ==========

    public void Avancer()
    {
        externalMovement = transform.forward;
    }

    public void Reculer()
    {
        externalMovement = -transform.forward;
    }

    public void TournerDroite()
    {
        externalRotation = 90f;
    }

    public void TournerGauche()
    {
        externalRotation = -90f;
    }

    public void Sauter()
    {
        if (controller.isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }
}
