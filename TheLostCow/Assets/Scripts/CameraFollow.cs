using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0f, 3f, -12f);
    
    [Header("Follow Settings")]
    [SerializeField] private float smoothSpeed = 5f;
    
    private void LateUpdate()
    {
        if (target == null) return;

        // Calcular la posición deseada de la cámara
        Vector3 desiredPosition = target.position + offset;
        
        // Suavizar el movimiento de la cámara
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        
        // Actualizar posición de la cámara
        transform.position = smoothedPosition;
        
        // Hacer que la cámara mire hacia el jugador
        transform.LookAt(target);
    }
}