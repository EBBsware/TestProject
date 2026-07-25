using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Sahne Ayarları")]
    [Tooltip("Yüklenecek oyun sahnesinin adı (Varsayılan: SampleScene)")]
    public string gameSceneName = "SampleScene";

    [Tooltip("Yüklenecek garaj sahnesinin adı (Varsayılan: GarageScene)")]
    public string garageSceneName = "GarageScene";

    [Header("UI Panelleri (İsteğe Bağlı)")]
    [Tooltip("Ayarlar paneli varsa buraya sürükleyin")]
    public GameObject settingsPanel;

    /// <summary>
    /// Oyunu başlatır ve belirtilen oyun sahnesine geçiş yapar.
    /// </summary>
    public void PlayGame()
    {
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
    /// Garaj sahnesine geçiş yapar.
    /// </summary>
    public void OpenGarage()
    {
        Time.timeScale = 1f;

        if (!string.IsNullOrEmpty(garageSceneName))
        {
            SceneManager.LoadScene(garageSceneName);
        }
    }

    /// <summary>
    /// Oyundan çıkış yapar.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Oyundan Çıkış Yapılıyor...");
        Application.Quit();
    }

    /// <summary>
    /// Ayarlar panelini açar.
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
