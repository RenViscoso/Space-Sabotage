using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SS_MenuController : MonoBehaviour
{

    [SerializeField] private Canvas mainMenuCanvas;                       // ����� �������� ����
    [SerializeField] private Canvas controlsCanvas;                       // ����� ����������

    // ������� ������ ���������� ��� �������:
    private void Awake()
    {
        controlsCanvas.enabled = false;
    }

    // �������� ������� �����
    public void StartButtonClick()
    {
        SceneManager.LoadScene("GameRoom");
    }

    // ����� �� ����
    public void ExitButtonClick()
    {
        Application.Quit();
    }

    // ������� � ���� ����������
    public void ControlsButtonClick()
    {
        mainMenuCanvas.enabled = false;
        controlsCanvas.enabled = true;
    }

    // ������� � ������� ����
    public void ReturnButtonClick()
    {
        mainMenuCanvas.enabled = true;
        controlsCanvas.enabled = false;
    }
}
