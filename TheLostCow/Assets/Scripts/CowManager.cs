using UnityEngine;

public class CowManager : MonoBehaviour
{
    [Header("Cow Settings")]
    [SerializeField] private GameObject cowPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform startPoint;
    [SerializeField] private float followSpeed = 3f;
    [SerializeField] private float followDistance = 2f;
    [SerializeField] private float interactionRadius = 5f;

    private GameObject currentCow;
    private bool isFollowing;
    private Transform playerTransform;
    private GameManager gameManager;

    private void Start()
    {
        Debug.Log("CowManager Start iniciado");
        gameManager = FindObjectOfType<GameManager>();
        playerTransform = FindObjectOfType<PlayerMovement>().transform;
        SpawnCow();
        Debug.Log($"CowManager inicializado - GameManager: {gameManager != null}, PlayerTransform: {playerTransform != null}, Cow: {currentCow != null}");
    }

    private void Update()
    {
        if (currentCow != null && isFollowing)
        {
            Debug.Log("Vaca siguiendo al jugador");
            Vector3 direction = playerTransform.position - currentCow.transform.position;
            direction.y = 0; // Mantener la misma altura
            
            if (direction.magnitude > followDistance)
            {
                direction.Normalize();
                currentCow.transform.position += direction * followSpeed * Time.deltaTime;
                currentCow.transform.rotation = Quaternion.Lerp(
                    currentCow.transform.rotation,
                    Quaternion.LookRotation(direction),
                    Time.deltaTime * 5f
                );
            }
        }
    }

    public void SpawnCow()
    {
        Debug.Log("Spawning new cow");
        if (currentCow != null)
        {
            Destroy(currentCow);
        }
        currentCow = Instantiate(cowPrefab, spawnPoint.position, Quaternion.identity);
        currentCow.tag = "Cow";
        isFollowing = false;
        Debug.Log($"Nueva vaca spawneada: {currentCow != null}");
    }

    public void ToggleFollowing()
    {
        if (currentCow != null)
        {
            float distanceToCow = Vector3.Distance(playerTransform.position, currentCow.transform.position);
            Debug.Log($"Distancia a la vaca: {distanceToCow}");
            
            if (distanceToCow < interactionRadius)
            {
                isFollowing = !isFollowing;
                Debug.Log($"Estado de seguimiento cambiado a: {isFollowing}");
            }
        }
    }

    public void CheckCowAtStart()
    {
        if (currentCow != null && isFollowing)
        {
            float distanceToStart = Vector3.Distance(currentCow.transform.position, startPoint.position);
            if (distanceToStart < 2f)
            {
                Debug.Log("Vaca llegó al punto de inicio");
                gameManager.AddPoint();
                SpawnCow();
            }
        }
    }

    public void OnCowHit()
    {
        Debug.Log("Vaca golpeada");
        SpawnCow();
    }
}