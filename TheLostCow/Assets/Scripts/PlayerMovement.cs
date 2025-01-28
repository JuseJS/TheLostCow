using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float jumpForce = 5f;

    [Header("Movement Boundaries")]
    [SerializeField] private float maxX = 75f;
    [SerializeField] private float minX = -75f;
    [SerializeField] private float maxZ = 115f;
    [SerializeField] private float minZ = -75f;

    [Header("Item Settings")]
    [SerializeField] private GameObject itemInHand;

    private Vector3 initialPosition;
    private Rigidbody rb;
    private CowManager cowManager;
    private GameManager gameManager;
    private Camera mainCamera;
    private Animator animator;
    private bool isGrounded = true;

    private void Start()
    {
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();

        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        cowManager = FindObjectOfType<CowManager>();
        gameManager = FindObjectOfType<GameManager>();
        mainCamera = Camera.main;
        itemInHand.SetActive(false);

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the player!");
        }
    }

    private void Update()
    {
        if (!gameManager.IsGameOver)
        {
            HandleMovement();
            ClampPosition();
            HandleItemInteraction();
            CheckCowAtStart();
            HandleJump();
            HandleCrouch();
            HandleHeadMovement();
            HandleBodyMovement();
        }
    }

    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 movement = (cameraForward * verticalInput + cameraRight * horizontalInput);

        if (movement.magnitude > 0)
        {
            movement.Normalize();
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            rb.velocity = movement * moveSpeed;

            // Establecer la velocidad para la animación de caminar/correr
            if (animator != null)
            {
                animator.SetFloat("Speed_f", movement.magnitude); // Controla la velocidad de la animación
                Debug.Log("Speed_f: " + movement.magnitude);
            }
        }
        else
        {
            rb.velocity = Vector3.zero;

            // Detener la animación de caminar/correr
            if (animator != null)
            {
                animator.SetFloat("Speed_f", 0);
                Debug.Log("Speed_f: 0");
            }
        }
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;

            // Activar animación de salto
            if (animator != null)
            {
                animator.SetBool("jump_b", true);
                Debug.Log("Jump animation triggered.");
            }
        }
    }

    private void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Activar/Desactivar animación de agacharse
            if (animator != null)
            {
                bool isCrouching = !animator.GetBool("Crouch_b");
                animator.SetBool("Crouch_b", isCrouching);
                Debug.Log("Crouch_b: " + isCrouching);
            }
        }
    }

    private void HandleHeadMovement()
    {
        // Controlar el movimiento de la cabeza (por ejemplo, con el mouse)
        float headHorizontal = Input.GetAxis("Mouse X");
        float headVertical = Input.GetAxis("Mouse Y");

        if (animator != null)
        {
            animator.SetFloat("Head_Horizontal_f", headHorizontal);
            animator.SetFloat("Head_Vertical_f", headVertical);
            Debug.Log("Head_Horizontal_f: " + headHorizontal + ", Head_Vertical_f: " + headVertical);
        }
    }

    private void HandleBodyMovement()
    {
        // Controlar el movimiento del cuerpo (por ejemplo, con el teclado)
        float bodyHorizontal = Input.GetAxis("Horizontal");
        float bodyVertical = Input.GetAxis("Vertical");

        if (animator != null)
        {
            animator.SetFloat("Body_Horizontal_f", bodyHorizontal);
            animator.SetFloat("Body_Vertical_f", bodyVertical);
            Debug.Log("Body_Horizontal_f: " + bodyHorizontal + ", Body_Vertical_f: " + bodyVertical);
        }
    }

    private void HandleItemInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            itemInHand.SetActive(!itemInHand.activeSelf);
            cowManager.ToggleFollowing();

            // Cambiar el estado de la animación (por ejemplo, sostener un objeto)
            if (animator != null)
            {
                animator.SetInteger("Animation_int", itemInHand.activeSelf ? 1 : 0); // 1 = sostener, 0 = no sostener
                Debug.Log("Animation_int: " + (itemInHand.activeSelf ? 1 : 0));
            }
        }
    }

    private void CheckCowAtStart()
    {
        cowManager.CheckCowAtStart();
    }

    private void ClampPosition()
    {
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, minZ, maxZ);
        clampedPosition.y = initialPosition.y;
        transform.position = clampedPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;

            // Desactivar animación de salto al tocar el suelo
            if (animator != null)
            {
                animator.SetBool("jump_b", false);
                Debug.Log("Jump animation deactivated.");
            }
        }

        if (collision.gameObject.CompareTag("Vehicle"))
        {
            gameManager.GameOver();

            // Activar animación de muerte
            if (animator != null)
            {
                animator.SetBool("death_b", true);
                animator.SetInteger("DeathType_int", Random.Range(1, 3)); // Cambia el tipo de muerte (1 o 2)
                Debug.Log("Death animation triggered.");
            }
        }
    }
}