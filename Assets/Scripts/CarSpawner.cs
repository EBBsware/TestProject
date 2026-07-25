using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [Header("Araba Prefab Listesi")]
    [Tooltip("Seçilebilir araba Prefab'larını buraya ekleyin (0: Doblo, 1: İkinci Araba vb.)")]
    public GameObject[] carPrefabs;

    [Header("Doğma Konumu")]
    [Tooltip("Arabanın doğacağı konum (Boş bırakılırsa bu objenin konumunu alır)")]
    public Transform spawnPoint;

    private GameObject spawnedCar;

    void Awake()
    {
        SpawnCar();
    }

    /// <summary>
    /// PlayerPrefs'ten seçili araba indeksini okur ve sahneye doğurur.
    /// Kamera, Hız Göstergesi ve Gaz/Fren butonlarını yeni arabaya otomatik bağlar.
    /// </summary>
    public void SpawnCar()
    {
        if (carPrefabs == null || carPrefabs.Length == 0)
        {
            Debug.LogWarning("CarSpawner: Atanmış hiç araba Prefab'ı yok!");
            return;
        }

        // Kaydedilmiş araba indeksini al (Varsayılan: 0)
        int selectedIndex = PlayerPrefs.GetInt("SelectedCarIndex", 0);

        // İndeks sınır dışıysa 0'a sıfırla
        if (selectedIndex < 0 || selectedIndex >= carPrefabs.Length)
        {
            selectedIndex = 0;
        }

        Vector3 pos = (spawnPoint != null) ? spawnPoint.position : transform.position;
        Quaternion rot = (spawnPoint != null) ? spawnPoint.rotation : transform.rotation;

        // Arabayı sahneye oluştur (Instantiate)
        spawnedCar = Instantiate(carPrefabs[selectedIndex], pos, rot);
        spawnedCar.name = "PlayerCar";

        // Sahnedeki diğer sistemlere yeni arabayı otomatik bağla
        AutoConnectComponents(spawnedCar);
    }

    private void AutoConnectComponents(GameObject carObj)
    {
        // 1. Kamera Takibi (CameraFollow)
        CameraFollow cameraFollow = FindFirstObjectByType<CameraFollow>();
        if (cameraFollow != null)
        {
            cameraFollow.target = carObj.transform;
        }

        // 2. Hız Göstergesi (Speedometer)
        Speedometer speedometer = FindFirstObjectByType<Speedometer>();
        if (speedometer != null)
        {
            speedometer.carRigidbody = carObj.GetComponent<Rigidbody2D>();
        }

        // 3. UI Gaz & Fren Butonları (UIButtonController)
        CarController carController = carObj.GetComponent<CarController>();
        UIButtonController[] uiButtons = FindObjectsByType<UIButtonController>(FindObjectsSortMode.None);
        foreach (UIButtonController button in uiButtons)
        {
            button.carController = carController;
        }
    }
}
