using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SS_UIController : MonoBehaviour
{
    [SerializeField] private SS_GameController gameController;                    // ����������
    [SerializeField] private Slider playerHPBar;                                    // ��� �� ��������� ������
    [SerializeField] private Slider destroyerHPBar;                                 // ��� �� ��������� Destroyer
    [SerializeField] private Canvas gameOverScreen;                                 // ����� ���������
    [SerializeField] private Canvas pauseScreen;                                    // ����� �����
    [SerializeField] private Canvas victoryScreen;                                  // ����� ������

    private void Awake()
    {
        // ������� ������ �������� ����������:
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

    // ������ �����
    public void PauseButtonClick()
    {
        gameController.Pause();
    }

    // �������� ����� �������� ����
    public void MenuButtonClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    // ������������ �����
    public void RestartButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
