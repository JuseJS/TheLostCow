using UnityEngine;

public class VehicleMovement : MonoBehaviour
{
    private float speed;
    private bool goingRight;
    private Camera mainCamera;
    private float destroyDistance = 100f;
    private Vector3 startPosition;
    private Rigidbody rb;

    public void Initialize(float vehicleSpeed, bool isGoingRight, Camera camera, VehiclePool vehiclePool)
    {
        speed = vehicleSpeed;
        goingRight = isGoingRight;
        mainCamera = camera;
        startPosition = transform.position;
        
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
        
        gameObject.tag = "Vehicle";
        Debug.Log($"Vehículo inicializado: {gameObject.name}, Speed: {speed}, RightLane: {goingRight}");
    }

    private void Update()
    {
        float direction = goingRight ? 1f : -1f;
        transform.Translate(Vector3.left * direction * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, startPosition) > destroyDistance)
        {
            ReturnToPool();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Colisión detectada en vehículo con: {collision.gameObject.name} (Tag: {collision.gameObject.tag})");
        
        if (collision.gameObject.CompareTag("Cow"))
        {
            Debug.Log("Procesando colisión con vaca");
            FindObjectOfType<CowManager>().OnCowHit();
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Procesando colisión con jugador");
            FindObjectOfType<GameManager>().GameOver();
        }
    }

    public void ReturnToPool()
    {
        FindObjectOfType<VehiclePool>().ReturnVehicle(gameObject);
    }
}