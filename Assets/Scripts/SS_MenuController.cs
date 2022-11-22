using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SS_MenuController : MonoBehaviour
{

    [SerializeField] private Canvas mainMenuCanvas;                       // Экран главного меню
    [SerializeField] private Canvas controlsCanvas;                       // Экран управления

    // Скрытие экрана управления при запуске:
    private void Awake()
    {
        controlsCanvas.enabled = false;
    }

    // Загрузка игровой сцены
    public void StartButtonClick()
    {
        SceneManager.LoadScene("GameRoom");
    }

    // Выход из игры
    public void ExitButtonClick()
    {
        Application.Quit();
    }

    // Переход в меню управления
    public void ControlsButtonClick()
    {
        mainMenuCanvas.enabled = false;
        controlsCanvas.enabled = true;
    }

    // Возврат в главное меню
    public void ReturnButtonClick()
    {
        mainMenuCanvas.enabled = true;
        controlsCanvas.enabled = false;
    }
}
