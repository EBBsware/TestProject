using System.Collections.Generic;
using UnityEngine;

public class EndlessRoadGenerator : MonoBehaviour
{
    [Header("Yol Ayarları")]
    [Tooltip("Sonsuz eklenecek yol Prefab'ı (Road_1)")]
    public GameObject roadPrefab;

    [Tooltip("İlk başta ekrana yerleştirilecek yol sayısı")]
    public int initialRoadCount = 5;

    [Tooltip("Bir yol parçasının genişliği (0 bırakılırsa Prefab Scale'i dahil genişlik otomatik hesaplanır)")]
    public float customRoadWidth = 0f;

    [Tooltip("Yolun Y yüksekliğini ince ayarlamak için (Örn: -0.5 veya -1.0 yaparak yolu aşağı kaydırabilirsiniz)")]
    public float yOffset = 0f;

    [Header("Takip Edilecek Obje")]
    [Tooltip("Takip edilecek hedef (Boş bırakılırsa Ana Kamera kullanılır)")]
    public Transform targetToFollow;

    private float roadWidth = 20f;
    private float nextSpawnX = 0f;
    private List<GameObject> activeRoads = new List<GameObject>();
    private Transform cameraTransform;

    void Start()
    {
        if (targetToFollow != null)
        {
            cameraTransform = targetToFollow;
        }
        else if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        if (roadPrefab == null)
        {
            Debug.LogError("EndlessRoadGenerator: Lütfen Road_1 Prefab'ını inspector'da atayın!");
            return;
        }

        // Yol genişliğini Prefab'ın Scale X (Genişlik) çarpanını da hesaba katarak tam ölç
        CalculateRoadWidth();

        // Başlangıçta yolları uca ekle
        nextSpawnX = transform.position.x;
        for (int i = 0; i < initialRoadCount; i++)
        {
            SpawnRoadTile();
        }
    }

    void Update()
    {
        if (cameraTransform == null || roadPrefab == null) return;

        // Kamera yola yaklaştıkça ön tarafa yeni yol ekle
        if (cameraTransform.position.x + (initialRoadCount * roadWidth * 0.4f) > nextSpawnX)
        {
            SpawnRoadTile();
            RemoveOldRoadTile();
        }
    }

    private void CalculateRoadWidth()
    {
        if (customRoadWidth > 0f)
        {
            roadWidth = customRoadWidth;
            return;
        }

        // Prefab Scale çarpanını al (X: 2 ise 2 ile çarpar)
        float prefabScaleX = roadPrefab.transform.localScale.x;
        if (prefabScaleX <= 0f) prefabScaleX = 1f;

        SpriteRenderer sr = roadPrefab.GetComponentInChildren<SpriteRenderer>();
        if (sr != null && sr.sprite != null)
        {
            float rawWidth = sr.sprite.rect.width / sr.sprite.pixelsPerUnit;
            roadWidth = rawWidth * prefabScaleX;
        }
        else
        {
            BoxCollider2D col = roadPrefab.GetComponentInChildren<BoxCollider2D>();
            if (col != null)
            {
                roadWidth = col.size.x * prefabScaleX;
            }
        }

        if (roadWidth <= 0f)
        {
            roadWidth = 20f;
        }

        Debug.Log("EndlessRoadGenerator: Prefab Scale (" + prefabScaleX + ") Dahil Yol Genişliği: " + roadWidth);
    }

    private void SpawnRoadTile()
    {
        Vector3 spawnPos = new Vector3(nextSpawnX, transform.position.y + yOffset, transform.position.z);
        GameObject newRoad = Instantiate(roadPrefab, spawnPos, roadPrefab.transform.rotation, transform);
        
        // Prefab'ın Scale değerlerini (X: 2, Y: 0.5) tam olarak koru
        newRoad.transform.localScale = roadPrefab.transform.localScale;

        // Yolun arka planın önünde (Order: 0) kalmasını garanti et
        SpriteRenderer[] srs = newRoad.GetComponentsInChildren<SpriteRenderer>();
        foreach (var sr in srs)
        {
            sr.sortingOrder = 0;
        }

        activeRoads.Add(newRoad);
        nextSpawnX += roadWidth;
    }

    private void RemoveOldRoadTile()
    {
        if (activeRoads.Count > initialRoadCount + 2)
        {
            GameObject oldRoad = activeRoads[0];
            activeRoads.RemoveAt(0);
            Destroy(oldRoad);
        }
    }
}
