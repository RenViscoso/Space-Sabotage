using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SS_UIController : MonoBehaviour
{
    [SerializeField] private SS_GameController gameController;                    // Контроллер
    [SerializeField] private Slider playerHPBar;                                    // Бар со здоровьем игрока
    [SerializeField] private Slider destroyerHPBar;                                 // Бар со здоровьем Destroyer
    [SerializeField] private Canvas gameOverScreen;                                 // Экран проигрыша
    [SerializeField] private Canvas pauseScreen;                                    // Экран паузы
    [SerializeField] private Canvas victoryScreen;                                  // Экран победы

    private void Awake()
    {
        // Убираем лишние элементы интерфейса:
        destroyerHPBar.enabled = false;
        gameOverScreen.enabled = false;
        pauseScreen.enabled = false;
        victoryScreen.enabled = false;
    }

    public void Pause()
    {
        pauseScreen.enabled = !pauseScreen.enabled;
    }

    public void UpdatePlayerHP(float hp)
    {
        playerHPBar.value = hp;
    }

    public void ShowDestroyerHP()
    {
        destroyerHPBar.enabled = true;
    }

    public void UpdateDestroyerHP(float hp)
    {
        destroyerHPBar.value = hp;
    }

    public void HideBars()
    {
        destroyerHPBar.enabled = false;
        playerHPBar.enabled = false;
    }

    public void ShowVictoryScreen()
    {
        victoryScreen.enabled = true;
    }

    public void ShowGameOverScreen()
    {
        playerHPBar.enabled = false;
        gameOverScreen.enabled = true;
    }

    // Снятие паузы
    public void PauseButtonClick()
    {
        gameController.Pause();
    }

    // Загрузка сцены главного меню
    public void MenuButtonClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    // Перезагрузка сцены
    public void RestartButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
