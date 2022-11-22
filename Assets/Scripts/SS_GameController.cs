using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SS_GameController : MonoBehaviour
{
    [SerializeField] private SS_PlayerShip playerPrefab;                            // Префаб игрока
    [SerializeField] private SS_SpikeballSmall spikeballSmallPrefab;                // Префаб малой мины
    [SerializeField] private SS_SpikeballBig spikeballBigPrefab;                    // Префаб большой мины
    [SerializeField] private SS_Trident tridentPrefab;                              // Префаб Trident
    [SerializeField] private SS_Destroyer destroyerPrefab;                          // Префаб Destroyer

    [SerializeField] private GameObject point1;                                     // Точка спауна 1
    [SerializeField] private GameObject point2;                                     // Точка спауна 2
    [SerializeField] private SS_UIController uiController;                          // Контроллер интерфейса

    [SerializeField] private GameObject backgroundStars;
    [SerializeField] private GameObject foregroundStars;

    private SS_PlayerShip player;                                                   // Игрока
    private SS_Destroyer destroyer;

    private float spawnRateMin = 1f;                        // Минимальное время между спауном противников
    private float spawnRateMax = 4f;                        // Максимальное время между спауном противников
    private int spawnsForRestMin = 8;                       // Минимально необходимое количество противников для ухода в перерыв
    private int spawnsForRestMax = 12;                      // Максимально необходимое количество противников для ухода в перерыв
    private float restTimeMin = 4f;                         // Минимальное время перерыва
    private float restTimeMax = 8f;                         // Максимальное время перерыва
    private int phase = 0;                                  // Фаза игры

    private float spikeballSmallChance = 0.8f;              // Шанс создания малой мины
    private float spikeballBigChance = 0.2f;                // Шанс создания большой мины
    private float tridentChance = 0f;                       // Шанс создания Trident

    private int spawnsCount = 0;                          // Количество cозданных противников
    private float timer = 0;                                // Переменная-таймер
    private bool bPause = false;                            // Активна ли пауза
    private bool bDestroyerStarted = false;                 // Был ли создан Destroyer
    private bool bDestroyerDefeated = false;                // Был ли побеждён Destroyer

    private bool newPhase = true;                           // Нужно ли перейти на новую фазу

    private void Awake()
    {
        timer = 2f;         // Установка таймера
        SetSpawnsCount();   // Установка необходимого количества противников
        player = Instantiate(playerPrefab, Vector3.back * 60, Quaternion.identity);
    }

    private void Update()
    {
        // Пауза:
        if (Input.GetButtonDown("Pause"))
        {
            Pause();
        }

        if (!bPause )
        {
            // Отображаем здоровье игрока, если он есть:
            if (player)
            {
                uiController.UpdatePlayerHP(player.Health);
            }

            // Если игрок погиб до уничтожения Destroyer - выводим экран поражения:
            if (!player && !bDestroyerDefeated)
            {
                uiController.ShowGameOverScreen();
                Destroy(this);
            }

            // Отображаем здоровье Destroyer, если он есть:
            if (bDestroyerStarted && destroyer)
            {
                uiController.UpdateDestroyerHP(destroyer.Health);
            }

            // Если Destroyer был побеждён:
            if (!destroyer && bDestroyerStarted && !bDestroyerDefeated)
            {
                bDestroyerDefeated = true;
                uiController.HideBars();
                player.StateSwitch(player.OutroState);
            }

            // Конец игры:
            if (bDestroyerDefeated && !player)
            {
                uiController.ShowVictoryScreen();
                Destroy(this);
            }

            if (!bDestroyerStarted)
            {

                
                // Чит-код для отладки работы фаз
                if (Input.GetKeyDown("p"))
                {
                    Debug.Log("Phase: " + (phase + 1).ToString());
                    spawnsCount = 0;
                }
                

                // Не уменьшаем таймер, если началась новая фаза и на экране ещё есть враги:
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

    // Функция для установки таймера между спауном противников
    private void SetTimer()
    {
        timer = Random.Range(spawnRateMin, spawnRateMax);
    }

    // Функция для установки таймера на перерыв
    private void SetTimerForRest()
    {
        timer = Random.Range(restTimeMin, restTimeMax);
    }

    // Функция для установки необходимого количества противников
    private void SetSpawnsCount()
    {
        spawnsCount = Mathf.FloorToInt(Random.Range(spawnsForRestMin, spawnsForRestMax));
    }

    // Суть работы спаунера:
    // Атаки врагов делятса на фазы.
    // Каждая фаза имеет определённое количество противников, которых нужно создать с перерывом.
    // Так же каждая фаза имеет определённый шанс cоздания каждого вида противников.
    // После создания всех противников спаунер уходит в режим ожидания.
    // Режим ожидания не кончается, пока не будут уничтожены все противники,
    // после чего должно дополнительно пройти определённое время.
    private void SpawnUpdate()
    {
        // Отмечаем, что новая фаза началась:
        newPhase = false;

        // На пятой фазе создаём Destroyer:
        if (phase == 5)
        {
            SpawnDestroyer();
            return;
        }

        // Точка спауна:
        Vector3 spawnPoint = Vector3.zero;
        while (true)
        {
            // Случайный выбор точки спауна:
            spawnPoint.x = Random.Range(point1.transform.position.x, point2.transform.position.x);
            spawnPoint.y = Random.Range(point1.transform.position.y, point2.transform.position.y);
            spawnPoint.z = Random.Range(point1.transform.position.z, point2.transform.position.z);

            // Проверка, чтобы не было столкновений:
            if (Physics.OverlapSphere(spawnPoint, 5).Length == 0)
            {
                break;
            }
        }

        // Создание врага:
        SpawnEnemy(spawnPoint);
        spawnsCount--;

        // Если ещё есть кого создавать - ставим таймер
        if (spawnsCount > 0)
        {
            SetTimer();
        }
        // Иначе:
        else
        {
            // Уходим на перерыв:
            SetTimerForRest();
            // Расчитывем количество противников для новой фазы:
            spawnsForRestMin = Mathf.Clamp((int)(spawnsForRestMin * 1.5f), 0, 32);
            spawnsForRestMax = Mathf.Clamp((int)(spawnsForRestMax * 1.5f), 0, 48);
            SetSpawnsCount();
            // Объявляем новую фазу игры:
            newPhase = true;

            // Смена шансов создания на разных фазах:
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

    // Создание Destroyer:
    private void SpawnDestroyer()
    {
        destroyer = Instantiate(destroyerPrefab, Vector3.forward * 60, new Quaternion(0, 0, 0, 0));
        destroyer.Player = player;
        bDestroyerStarted = true;
        uiController.ShowDestroyerHP();
    }

    // Функция выбора противника для создания. Сумма вероятностей спауна всех противников должна равняться единице.
    // Принцип работы: создаётся псевдорандомное число. Из суммы всех вероятностей последовательно вычитаются вероятности каждого противника,
    // после чего идёт проверка, является ли псевдорандомное число меньше результата вычетания. Если да - спаунится этот противник.
    // Если нет - проверяется следующий.
    private void SpawnEnemy(Vector3 spawnPoint)
    {
        // Создание словаря
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
                // Создаём врага и даём указатель на игрока:
                SS_EnemyShip enemy = Instantiate(entry.Key, spawnPoint, Quaternion.identity);
                enemy.Player = player;
                return;
            }
        }
    }

    // Пауза:
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
            // Останавливаем звёзды
            backgroundStars.GetComponent<ParticleSystem>().Pause();
            foregroundStars.GetComponent<ParticleSystem>().Pause();
        }
        else
        {
            // Воспроизводим звёзды
            backgroundStars.GetComponent<ParticleSystem>().Play();
            foregroundStars.GetComponent<ParticleSystem>().Play();
        }
    }
}
