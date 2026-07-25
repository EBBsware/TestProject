using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("UI Elemanları")]
    [Tooltip("Duraklatma (Pause) Menüsü Paneli")]
    public GameObject pauseMenuUI;

    [Header("Sahne Ayarları")]
    [Tooltip("Ana Menü sahnesinin tam adı (Varsayılan: MainMenuScene)")]
    public string mainMenuSceneName = "MainMenuScene";

    [HideInInspector]
    public bool isPaused = false;

    void Update()
    {
        // Klavyede ESC tuşuna basıldığında oyunu duraklat veya devam ettir
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    /// <summary>
    /// Oyunu devam ettirir ve pause panelini gizler.
    /// </summary>
    public void ResumeGame()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
        
        Time.timeScale = 1f;
        isPaused = false;
    }

    /// <summary>
    /// Oyunu dondurur (durdurur) ve pause panelini gösterir.
    /// </summary>
    public void PauseGame()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
        }

        Time.timeScale = 0f;
        isPaused = true;
    }

    /// <summary>
    /// Mevcut sahneyi yeniden başlatır.
    /// </summary>
    public void RestartGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Ana Menü sahnesine dönüş yapar.
    /// </summary>
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        
        if (!string.IsNullOrEmpty(mainMenuSceneName))
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}
