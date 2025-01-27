using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0f, 5f, -8f);
    
    [Header("Follow Settings")]
    [SerializeField] private float smoothSpeed = 5f;
    
    [Header("Camera Control Settings")]
    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private float minVerticalAngle = -30f;
    [SerializeField] private float maxVerticalAngle = 60f;
    [SerializeField] private float minDistance = 5f;
    [SerializeField] private float maxDistance = 15f;
    
    private float currentRotationX = 0f;
    private float currentRotationY = 0f;
    private float currentDistance;
    
    private void Start()
    {
        currentDistance = offset.magnitude;
        currentRotationY = transform.eulerAngles.y;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    private void LateUpdate()
    {
        if (target == null) return;

        // Control de rotación con el ratón
        if (Input.GetMouseButton(1)) // Botón derecho del ratón
        {
            currentRotationY += Input.GetAxis("Mouse X") * rotationSpeed;
            currentRotationX -= Input.GetAxis("Mouse Y") * rotationSpeed;
            currentRotationX = Mathf.Clamp(currentRotationX, minVerticalAngle, maxVerticalAngle);
        }

        // Control de zoom con la rueda del ratón
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        currentDistance = Mathf.Clamp(currentDistance - scrollWheel * 5f, minDistance, maxDistance);

        // Calcular la posición deseada de la cámara
        Quaternion rotation = Quaternion.Euler(currentRotationX, currentRotationY, 0);
        Vector3 direction = rotation * Vector3.forward;
        Vector3 desiredPosition = target.position - direction * currentDistance;
        desiredPosition.y += offset.y; // Mantener altura relativa

        // Suavizar el movimiento de la cámara
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        
        // Hacer que la cámara mire hacia el objetivo
        transform.LookAt(target.position + Vector3.up * offset.y * 0.5f);
    }
}