using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CarSelectionManager : MonoBehaviour
{
    [Header("Araba Prefab Listesi")]
    [Tooltip("100 tane araba da olsa tüm Prefab'ları sadece buraya ekleyin! Otomatik sergilenecektir.")]
    public GameObject[] carPrefabs;

    [Header("Garaj Sergileme Konumu")]
    [Tooltip("Arabanın garajda sergileneceği konum (Boş bırakılırsa bu objenin konumu kullanılır)")]
    public Transform previewSpawnPoint;

    [Header("UI Elemanları (İsteğe Bağlı)")]
    [Tooltip("Araba adını gösteren metin (Text)")]
    public Text carNameText;

    [Header("Sahne Ayarları")]
    [Tooltip("Yarışın başlayacağı oyun sahnesinin adı")]
    public string gameSceneName = "SampleScene";

    [Tooltip("Ana Menü sahnesinin adı")]
    public string mainMenuSceneName = "MainMenuScene";

    private int currentCarIndex = 0;
    private GameObject currentPreviewObject;

    void Start()
    {
        // Daha önce seçilmiş araba varsa onu hatırla
        currentCarIndex = PlayerPrefs.GetInt("SelectedCarIndex", 0);
        UpdateSelectionUI();
    }

    /// <summary>
    /// Bir sonraki arabayı gösterir.
    /// </summary>
    public void NextCar()
    {
        if (carPrefabs == null || carPrefabs.Length == 0) return;

        currentCarIndex++;
        if (currentCarIndex >= carPrefabs.Length)
        {
            currentCarIndex = 0;
        }

        UpdateSelectionUI();
    }

    /// <summary>
    /// Bir önceki arabayı gösterir.
    /// </summary>
    public void PreviousCar()
    {
        if (carPrefabs == null || carPrefabs.Length == 0) return;

        currentCarIndex--;
        if (currentCarIndex < 0)
        {
            currentCarIndex = carPrefabs.Length - 1;
        }

        UpdateSelectionUI();
    }

    /// <summary>
    /// Seçilen arabayı kaydeder ve doğrudan oyunu başlatır.
    /// </summary>
    public void SelectAndPlay()
    {
        SaveSelection();

        Time.timeScale = 1f;
        if (!string.IsNullOrEmpty(gameSceneName))
        {
            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }

    /// <summary>
    /// Seçilen arabayı kaydeder ve Ana Menüye geri döner.
    /// </summary>
    public void SelectAndGoToMainMenu()
    {
        SaveSelection();
        GoToMainMenu();
    }

    /// <summary>
    /// Ana Menüye geri döner.
    /// </summary>
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        if (!string.IsNullOrEmpty(mainMenuSceneName))
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    private void SaveSelection()
    {
        PlayerPrefs.SetInt("SelectedCarIndex", currentCarIndex);
        PlayerPrefs.Save();
        Debug.Log("Garaj: Araba Seçimi Kaydedildi! Seçilen İndeks: " + currentCarIndex);
    }

    private void UpdateSelectionUI()
    {
        if (carPrefabs == null || carPrefabs.Length == 0) return;

        // Eski önizleme objesini garajdan kaldır
        if (currentPreviewObject != null)
        {
            Destroy(currentPreviewObject);
        }

        if (currentCarIndex < 0 || currentCarIndex >= carPrefabs.Length)
        {
            currentCarIndex = 0;
        }

        if (carPrefabs[currentCarIndex] != null)
        {
            Vector3 pos = (previewSpawnPoint != null) ? previewSpawnPoint.position : transform.position;
            Quaternion rot = (previewSpawnPoint != null) ? previewSpawnPoint.rotation : transform.rotation;

            // Seçilen arabanın Prefab'ını garajda canlı oluştur
            currentPreviewObject = Instantiate(carPrefabs[currentCarIndex], pos, rot);
            currentPreviewObject.name = "GaragePreview_" + carPrefabs[currentCarIndex].name;
            currentPreviewObject.SetActive(true);

            // Doblo veya altındaki Car_Body objeleri kapalıysa hepsini otomatik aktif (Enable) et
            Transform[] allTransforms = currentPreviewObject.GetComponentsInChildren<Transform>(true);
            foreach (Transform t in allTransforms)
            {
                t.gameObject.SetActive(true);
            }

            // Garajda arabanın yere düşmemesi veya hareket etmemesi için fizikleri geçici dondur
            Rigidbody2D[] rbs = currentPreviewObject.GetComponentsInChildren<Rigidbody2D>();
            foreach (var rb in rbs)
            {
                rb.simulated = false;
            }

            // Metin varsa araba adını ekranda güncelle
            if (carNameText != null)
            {
                carNameText.text = carPrefabs[currentCarIndex].name;
            }
        }
    }
}
