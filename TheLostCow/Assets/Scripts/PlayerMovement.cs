using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    
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

    private void Start()
    {
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        cowManager = FindObjectOfType<CowManager>();
        gameManager = FindObjectOfType<GameManager>();
        itemInHand.SetActive(false);
    }

    private void Update()
    {
        if (!gameManager.IsGameOver)
        {
            HandleMovement();
            ClampPosition();
            HandleItemInteraction();
            CheckCowAtStart();
        }
    }

    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);

        if (movement.magnitude > 0)
        {
            movement.Normalize();
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            rb.velocity = movement * moveSpeed;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void HandleItemInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            itemInHand.SetActive(!itemInHand.activeSelf);
            cowManager.ToggleFollowing();
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
        if (collision.gameObject.CompareTag("Vehicle"))
        {
            gameManager.GameOver();
        }
    }
}