using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SS_GameController : MonoBehaviour
{
    [SerializeField] private SS_PlayerShip playerPrefab;                            // ������ ������
    [SerializeField] private SS_SpikeballSmall spikeballSmallPrefab;                // ������ ����� ����
    [SerializeField] private SS_SpikeballBig spikeballBigPrefab;                    // ������ ������� ����
    [SerializeField] private SS_Trident tridentPrefab;                              // ������ Trident
    [SerializeField] private SS_Destroyer destroyerPrefab;                          // ������ Destroyer

    [SerializeField] private GameObject point1;                                     // ����� ������ 1
    [SerializeField] private GameObject point2;                                     // ����� ������ 2
    [SerializeField] private SS_UIController uiController;                          // ���������� ����������

    [SerializeField] private GameObject backgroundStars;
    [SerializeField] private GameObject foregroundStars;

    private SS_PlayerShip player;                                                   // ������
    private SS_Destroyer destroyer;

    private float spawnRateMin = 1f;                        // ����������� ����� ����� ������� �����������
    private float spawnRateMax = 4f;                        // ������������ ����� ����� ������� �����������
    private int spawnsForRestMin = 8;                       // ���������� ����������� ���������� ����������� ��� ����� � �������
    private int spawnsForRestMax = 12;                      // ����������� ����������� ���������� ����������� ��� ����� � �������
    private float restTimeMin = 4f;                         // ����������� ����� ��������
    private float restTimeMax = 8f;                         // ������������ ����� ��������
    private int phase = 0;                                  // ���� ����

    private float spikeballSmallChance = 0.8f;              // ���� �������� ����� ����
    private float spikeballBigChance = 0.2f;                // ���� �������� ������� ����
    private float tridentChance = 0f;                       // ���� �������� Trident

    private int spawnsCount = 0;                          // ���������� c�������� �����������
    private float timer = 0;                                // ����������-������
    private bool bPause = false;                            // ������� �� �����
    private bool bDestroyerStarted = false;                 // ��� �� ������ Destroyer
    private bool bDestroyerDefeated = false;                // ��� �� ������� Destroyer

    private bool newPhase = true;                           // ����� �� ������� �� ����� ����

    private void Awake()
    {
        timer = 2f;         // ��������� �������
        SetSpawnsCount();   // ��������� ������������ ���������� �����������
        player = Instantiate(playerPrefab, Vector3.back * 60, Quaternion.identity);
    }

    private void Update()
    {
        // �����:
        if (Input.GetButtonDown("Pause"))
        {
            Pause();
        }

        if (!bPause )
        {
            // ���������� �������� ������, ���� �� ����:
            if (player)
            {
                uiController.UpdatePlayerHP(player.Health);
            }

            // ���� ����� ����� �� ����������� Destroyer - ������� ����� ���������:
            if (!player && !bDestroyerDefeated)
            {
                uiController.ShowGameOverScreen();
                Destroy(this);
            }

            // ���������� �������� Destroyer, ���� �� ����:
            if (bDestroyerStarted && destroyer)
            {
                uiController.UpdateDestroyerHP(destroyer.Health);
            }

            // ���� Destroyer ��� �������:
            if (!destroyer && bDestroyerStarted && !bDestroyerDefeated)
            {
                bDestroyerDefeated = true;
                uiController.HideBars();
                player.StateSwitch(player.OutroState);
            }

            // ����� ����:
            if (bDestroyerDefeated && !player)
            {
                uiController.ShowVictoryScreen();
                Destroy(this);
            }

            if (!bDestroyerStarted)
            {

                
                // ���-��� ��� ������� ������ ���
                if (Input.GetKeyDown("p"))
                {
                    Debug.Log("Phase: " + (phase + 1).ToString());
                    spawnsCount = 0;
                }
                

                // �� ��������� ������, ���� �������� ����� ���� � �� ������ ��� ���� �����:
                if (newPhase && FindObjectsOfType<SS_EnemyShip>().Length == 0 || !newPhase)
                {
                    timer -= Time.deltaTime;
                }

                if (timer <= 0f)
                {
                    SpawnUpdate();
                }
            }
        }
    }

    // ������� ��� ��������� ������� ����� ������� �����������
    private void SetTimer()
    {
        timer = Random.Range(spawnRateMin, spawnRateMax);
    }

    // ������� ��� ��������� ������� �� �������
    private void SetTimerForRest()
    {
        timer = Random.Range(restTimeMin, restTimeMax);
    }

    // ������� ��� ��������� ������������ ���������� �����������
    private void SetSpawnsCount()
    {
        spawnsCount = Mathf.FloorToInt(Random.Range(spawnsForRestMin, spawnsForRestMax));
    }

    // ���� ������ ��������:
    // ����� ������ ������� �� ����.
    // ������ ���� ����� ����������� ���������� �����������, ������� ����� ������� � ���������.
    // ��� �� ������ ���� ����� ����������� ���� c������� ������� ���� �����������.
    // ����� �������� ���� ����������� ������� ������ � ����� ��������.
    // ����� �������� �� ���������, ���� �� ����� ���������� ��� ����������,
    // ����� ���� ������ ������������� ������ ����������� �����.
    private void SpawnUpdate()
    {
        // ��������, ��� ����� ���� ��������:
        newPhase = false;

        // �� ����� ���� ������ Destroyer:
        if (phase == 5)
        {
            SpawnDestroyer();
            return;
        }

        // ����� ������:
        Vector3 spawnPoint = Vector3.zero;
        while (true)
        {
            // ��������� ����� ����� ������:
            spawnPoint.x = Random.Range(point1.transform.position.x, point2.transform.position.x);
            spawnPoint.y = Random.Range(point1.transform.position.y, point2.transform.position.y);
            spawnPoint.z = Random.Range(point1.transform.position.z, point2.transform.position.z);

            // ��������, ����� �� ���� ������������:
            if (Physics.OverlapSphere(spawnPoint, 5).Length == 0)
            {
                break;
            }
        }

        // �������� �����:
        SpawnEnemy(spawnPoint);
        spawnsCount--;

        // ���� ��� ���� ���� ��������� - ������ ������
        if (spawnsCount > 0)
        {
            SetTimer();
        }
        // �����:
        else
        {
            // ������ �� �������:
            SetTimerForRest();
            // ���������� ���������� ����������� ��� ����� ����:
            spawnsForRestMin = Mathf.Clamp((int)(spawnsForRestMin * 1.5f), 0, 32);
            spawnsForRestMax = Mathf.Clamp((int)(spawnsForRestMax * 1.5f), 0, 48);
            SetSpawnsCount();
            // ��������� ����� ���� ����:
            newPhase = true;

            // ����� ������ �������� �� ������ �����:
            switch (phase++)
            {
                case 1:
                    spikeballSmallChance = 0.6f;
                    spikeballBigChance = 0.4f;
                    tridentChance = 0f;
                    break;

                case 2:
                    spikeballSmallChance = 0.4f;
                    spikeballBigChance = 0.5f;
                    tridentChance = 0.1f;
                    break;

                case 3:
                    spikeballSmallChance = 0.3f;
                    spikeballBigChance = 0.6f;
                    tridentChance = 0.3f;
                    break;

                case 4:
                    spikeballSmallChance = 0.2f;
                    spikeballBigChance = 0.4f;
                    tridentChance = 0.4f;
                    break;
            }
        }
    }

    // �������� Destroyer:
    private void SpawnDestroyer()
    {
        destroyer = Instantiate(destroyerPrefab, Vector3.forward * 60, new Quaternion(0, 0, 0, 0));
        destroyer.Player = player;
        bDestroyerStarted = true;
        uiController.ShowDestroyerHP();
    }

    // ������� ������ ���������� ��� ��������. ����� ������������ ������ ���� ����������� ������ ��������� �������.
    // ������� ������: �������� ��������������� �����. �� ����� ���� ������������ ��������������� ���������� ����������� ������� ����������,
    // ����� ���� ��� ��������, �������� �� ��������������� ����� ������ ���������� ���������. ���� �� - ��������� ���� ���������.
    // ���� ��� - ����������� ���������.
    private void SpawnEnemy(Vector3 spawnPoint)
    {
        // �������� �������
        Dictionary<SS_EnemyShip, float> weights = new Dictionary<SS_EnemyShip, float>()
        {
            {spikeballSmallPrefab, spikeballSmallChance},
            {spikeballBigPrefab, spikeballBigChance},
            {tridentPrefab, tridentChance},
        };

        float rand = Random.Range(0f, 1f);
        float full = weights.Sum(x => x.Value);

        foreach (KeyValuePair<SS_EnemyShip, float> entry in weights)
        {
            float currentWeight = entry.Value;
            full -= currentWeight;
            if (rand > full)
            {
                // ������ ����� � ��� ��������� �� ������:
                SS_EnemyShip enemy = Instantiate(entry.Key, spawnPoint, Quaternion.identity);
                enemy.Player = player;
                return;
            }
        }
    }

    // �����:
    public void Pause()
    {
        bPause = !bPause;
        uiController.Pause();
        foreach (SS_Entity entity in FindObjectsOfType<SS_Entity>())
        {
            entity.Pause();
        }
        if (bPause)
        {
            // ������������� �����
            backgroundStars.GetComponent<ParticleSystem>().Pause();
            foregroundStars.GetComponent<ParticleSystem>().Pause();
        }
        else
        {
            // ������������� �����
            backgroundStars.GetComponent<ParticleSystem>().Play();
            foregroundStars.GetComponent<ParticleSystem>().Play();
        }
    }
}
