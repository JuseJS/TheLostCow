using UnityEngine;
using System.Collections.Generic;

public class TrafficManager : MonoBehaviour
{
    [System.Serializable]
    public class VehicleType
    {
        public GameObject prefab;
        public float speed;
        public string vehicleName;
    }

    [System.Serializable]
    public class SpawnLane
    {
        public Transform spawnPoint;
        public bool isRightLane; // true si el carril va hacia la derecha
        [Header("Tiempo de Spawn")]
        public float minSpawnTime = 2f;
        public float maxSpawnTime = 5f;
        [HideInInspector]
        public float nextSpawnTime;
    }

    [Header("Configuración de Vehículos")]
    [SerializeField] private List<VehicleType> vehicleTypes = new List<VehicleType>();

    [Header("Configuración de Carriles")]
    [SerializeField] private List<SpawnLane> lanes = new List<SpawnLane>();

    private Camera mainCamera;
    [SerializeField] private VehiclePool vehiclePool;

    private void Start()
    {
        mainCamera = Camera.main;
        vehiclePool = gameObject.AddComponent<VehiclePool>();

        // Inicializar los tiempos de spawn para cada carril
        foreach (var lane in lanes)
        {
            lane.nextSpawnTime = Time.time + Random.Range(lane.minSpawnTime, lane.maxSpawnTime);
        }
    }

    private void Update()
    {
        // Revisar cada carril individualmente
        foreach (var lane in lanes)
        {
            if (Time.time >= lane.nextSpawnTime)
            {
                SpawnVehicleInLane(lane);
                // Establecer el siguiente tiempo de spawn para este carril
                lane.nextSpawnTime = Time.time + Random.Range(lane.minSpawnTime, lane.maxSpawnTime);
            }
        }
    }

    private void SpawnVehicleInLane(SpawnLane lane)
    {
        VehicleType selectedVehicle = vehicleTypes[Random.Range(0, vehicleTypes.Count)];
        Quaternion rotation = lane.isRightLane ? Quaternion.Euler(0f, -90f, 0f) : Quaternion.Euler(0f, 90f, 0f);
        
        GameObject vehicle = vehiclePool.GetVehicle(selectedVehicle.prefab, lane.spawnPoint.position, rotation);
        VehicleMovement movement = vehicle.GetComponent<VehicleMovement>() ?? vehicle.AddComponent<VehicleMovement>();
        movement.Initialize(selectedVehicle.speed, lane.isRightLane, mainCamera, vehiclePool);
    }

    // Método para visualizar los puntos de spawn en el editor
    private void OnDrawGizmos()
    {
        foreach (var lane in lanes)
        {
            if (lane.spawnPoint != null)
            {
                Gizmos.color = lane.isRightLane ? Color.blue : Color.red;
                Gizmos.DrawWireSphere(lane.spawnPoint.position, 1f);
                // Dibujar una flecha para indicar la dirección
                Vector3 direction = lane.isRightLane ? Vector3.right : Vector3.left;
                Gizmos.DrawRay(lane.spawnPoint.position, direction * 2f);
            }
        }
    }
}