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
        spawnedCar.SetActive(true);

        // Doblo veya altındaki parçalar kapalıysa hepsini otomatik aktif (Enable) et
        Transform[] allTransforms = spawnedCar.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in allTransforms)
        {
            t.gameObject.SetActive(true);
        }

        Debug.Log("CarSpawner: Başarıyla Doğurulan Araba İndeksi: " + selectedIndex + " (" + carPrefabs[selectedIndex].name + ")");

        // Sahnedeki diğer sistemlere yeni arabayı otomatik bağla
        AutoConnectComponents(spawnedCar);
    }

    private void AutoConnectComponents(GameObject carObj)
    {
        // CarController veya Rigidbody2D gövde objesinde (Car_Body) olabileceği için alt objeleri de tara
        CarController carController = carObj.GetComponentInChildren<CarController>();
        Rigidbody2D carRb = (carController != null) ? carController.GetComponent<Rigidbody2D>() : carObj.GetComponentInChildren<Rigidbody2D>();
        Transform targetTransform = (carController != null) ? carController.transform : carObj.transform;

        // Arabanın yolun ve arka planın önünde (Order: 10) kalmasını sağla
        SpriteRenderer[] srs = carObj.GetComponentsInChildren<SpriteRenderer>(true);
        foreach (var sr in srs)
        {
            sr.sortingOrder += 10;
        }

        // 1. Kamera Takibi (CameraFollow)
        CameraFollow cameraFollow = FindAnyObjectByType<CameraFollow>();
        if (cameraFollow != null)
        {
            cameraFollow.target = targetTransform;
        }

        // 2. Hız Göstergesi (Speedometer)
        Speedometer speedometer = FindAnyObjectByType<Speedometer>();
        if (speedometer != null)
        {
            speedometer.carRigidbody = carRb;
        }

        // 3. UI Gaz & Fren Butonları (UIButtonController)
        UIButtonController[] uiButtons = FindObjectsByType<UIButtonController>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (UIButtonController button in uiButtons)
        {
            button.carController = carController;
        }
    }
}
