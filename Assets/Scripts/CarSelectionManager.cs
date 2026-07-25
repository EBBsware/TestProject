using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CarSelectionManager : MonoBehaviour
{
    [Header("Araba Görselleri / Objeleri")]
    [Tooltip("Ana menüde sergilenecek araba görselleri veya modelleri")]
    public GameObject[] carPreviews;

    [Header("UI Elemanları (İsteğe Bağlı)")]
    [Tooltip("Araba adını gösteren metin (Text)")]
    public Text carNameText;

    [Header("Sahne Ayarları")]
    [Tooltip("Başlatılacak oyun sahnesinin adı")]
    public string gameSceneName = "SampleScene";

    private int currentCarIndex = 0;

    void Start()
    {
        // Daha önce seçilmiş araba varsa onu hatırla (Varsayılan: 0)
        currentCarIndex = PlayerPrefs.GetInt("SelectedCarIndex", 0);
        UpdateSelectionUI();
    }

    /// <summary>
    /// Bir sonraki arabayı gösterir.
    /// </summary>
    public void NextCar()
    {
        if (carPreviews == null || carPreviews.Length == 0) return;

        currentCarIndex++;
        if (currentCarIndex >= carPreviews.Length)
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
        if (carPreviews == null || carPreviews.Length == 0) return;

        currentCarIndex--;
        if (currentCarIndex < 0)
        {
            currentCarIndex = carPreviews.Length - 1;
        }

        UpdateSelectionUI();
    }

    /// <summary>
    /// Seçimi kaydeder ve oyunu başlatır.
    /// </summary>
    public void SelectAndPlay()
    {
        PlayerPrefs.SetInt("SelectedCarIndex", currentCarIndex);
        PlayerPrefs.Save();

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

    private void UpdateSelectionUI()
    {
        // Tüm araba önizlemelerini gizle, sadece seçili olanı aç
        if (carPreviews != null && carPreviews.Length > 0)
        {
            for (int i = 0; i < carPreviews.Length; i++)
            {
                if (carPreviews[i] != null)
                {
                    carPreviews[i].SetActive(i == currentCarIndex);
                }
            }

            // Metin varsa araba adını güncelle
            if (carNameText != null && carPreviews[currentCarIndex] != null)
            {
                carNameText.text = carPreviews[currentCarIndex].name;
            }
        }
    }
}
