using UnityEngine;
using UnityEngine.SceneManagement;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

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
        bool escapePressed = false;

#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            escapePressed = true;
        }
#else
        try
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                escapePressed = true;
            }
        }
        catch
        {
            // Input System aktifse eski Input API hata vermesin
        }
#endif

        if (escapePressed)
        {
            TogglePause();
        }
    }

    /// <summary>
    /// Ekrandaki Pause butonuna dokunulduğunda veya ESC'ye basıldığında duraklatma durumunu değiştirir.
    /// </summary>
    public void TogglePause()
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

        try
        {
            if (!string.IsNullOrEmpty(mainMenuSceneName))
            {
                SceneManager.LoadScene(mainMenuSceneName);
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Ana Menüye yüklenirken hata oluştu! Lütfen MainMenuScene sahnesini File -> Build Settings alanına eklediğinizden emin olun. Hata: " + ex.Message);
        }
    }
}
