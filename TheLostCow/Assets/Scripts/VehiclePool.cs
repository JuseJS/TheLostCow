using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehiclePool : MonoBehaviour
{
    private Dictionary<string, Queue<GameObject>> vehiclePool = new Dictionary<string, Queue<GameObject>>();

    public GameObject GetVehicle(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        string prefabId = $"{prefab.name}_{Time.time}"; // ID único para cada instancia
        if (!vehiclePool.ContainsKey(prefab.name))
            vehiclePool[prefab.name] = new Queue<GameObject>();

        GameObject vehicle;
        if (vehiclePool[prefab.name].Count > 0)
        {
            vehicle = vehiclePool[prefab.name].Dequeue();
            vehicle.transform.position = position;
            vehicle.transform.rotation = rotation;
            vehicle.SetActive(true);
        }
        else
        {
            vehicle = Instantiate(prefab, position, rotation);
            vehicle.name = prefabId;
        }
        
        return vehicle;
    }

    public void ReturnVehicle(GameObject vehicle)
    {
        string basePrefabName = vehicle.name.Split('_')[0];
        vehicle.SetActive(false);
        vehiclePool[basePrefabName].Enqueue(vehicle);
    }
}