using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Sahne Ayarları")]
    [Tooltip("Yüklenecek oyun sahnesinin adı (Varsayılan: SampleScene)")]
    public string gameSceneName = "SampleScene";

    [Header("UI Panelleri (İsteğe Bağlı)")]
    [Tooltip("Ayarlar paneli varsa buraya sürükleyin")]
    public GameObject settingsPanel;

    /// <summary>
    /// Oyunu başlatır ve belirtilen oyun sahnesine geçiş yapar.
    /// Button OnClick() olayına bağlanır.
    /// </summary>
    public void PlayGame()
    {
        // Zamandan emin olmak için (Pause'da kalmış olabilir) timeScale'i 1 yapalım
        Time.timeScale = 1f;

        if (!string.IsNullOrEmpty(gameSceneName))
        {
            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            // Eğer sahne ismi boşsa listedeki 1. indexli sahneyi yükle
            SceneManager.LoadScene(1);
        }
    }

    /// <summary>
    /// Oyundan çıkış yapar. Unity Editor'de çalışırken konsola da bilgi yazar.
    /// Button OnClick() olayına bağlanır.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Oyundan Çıkış Yapılıyor...");
        Application.Quit();
    }

    /// <summary>
    /// Ayarlar panelini açar (Panel atanmışsa aktif eder).
    /// </summary>
    public void OpenSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
    }

    /// <summary>
    /// Ayarlar panelini kapatır.
    /// </summary>
    public void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }
}
