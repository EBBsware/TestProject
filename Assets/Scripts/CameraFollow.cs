using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Tooltip("Takip edilecek araç gövdesi (CarBody objesini buraya sürükleyin)")]
    public Transform target;
    
    [Tooltip("Kameranın araca göre konumu (Z değeri -10 kalmalı)")]
    public Vector3 offset = new Vector3(0f, 2f, -10f);
    
    [Tooltip("Kameranın takip etme yumuşaklığı (Değer düştükçe daha hızlı tepki verir)")]
    [Range(0.01f, 1f)]
    public float smoothTime = 0.15f;
    
    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        // Eğer takip edilecek bir hedef yoksa hiçbir şey yapma
        if (target == null) return;

        // Kameranın gitmek istediği asıl hedef pozisyon
        Vector3 targetPosition = target.position + offset;

        // Kamerayı mevcut pozisyonundan hedef pozisyonuna yumuşakça (SmoothDamp) kaydır
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
