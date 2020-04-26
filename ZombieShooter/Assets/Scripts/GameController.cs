using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Controller;
    public static int Highscore;
    public static float ZombieSpawnInterval;
    public static float SoundVolume;

    [SerializeField] private GameObject _zombie;
    [SerializeField] private int _pistolClipSize = 17;
    [SerializeField] private int _shotgunClipSize = 8;
    [SerializeField] private int _pistolSpareAmmo = 17;
    [SerializeField] private int _shotgunSpareAmmo = 8;
    [SerializeField] private Text _ammoCounter;
    [SerializeField] private Text _clipAmmoCounter;
    [SerializeField] private Text _reloadTimerText;
    [SerializeField] private float _pistolReloadTime = 3;
    [SerializeField] private float _shotgunReloadTime = 5;
    [SerializeField] private Text _staminaCount;
    [SerializeField] private float _staminaDrainPerSecond = 10;
    [SerializeField] private float _staminaRegenPerSecond = 5;
    [SerializeField] private GameObject _gameOverText;
    [SerializeField] private GameObject _newHighscoreText;
    [SerializeField] private Text _scoreCount;
    [SerializeField] private Text _secondCount;
    [SerializeField] private Text _minuteCount;
    [SerializeField] private float _zombieSpawnDistance = 10;

    public GameObject[] PatrolPoints;
    public GameObject Player;
    public PlayerMovement PlayerMovement;
    public bool GameOver;
    public bool Shooting;
    public bool Reloading;
    public bool PistolEquipped;
    public bool ShotgunEquipped;
    public bool FirstPersonMode;
    public int PistolClip;
    public int ShotgunClip;
    public float Stamina = 100;

    private float _reloadTimer;
    private float _zombieTimer;
    private int _score;
    private float _timerSeconds;
    private int _timerMinutes;

    void Awake()
    {
        Controller = this;
        PatrolPoints = GameObject.FindGameObjectsWithTag("PatrolPoint");
    }

    void Start()
    {
        Cursor.visible = false;
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerMovement = Player.GetComponent<PlayerMovement>();
        PistolClip = _pistolClipSize;
        ShotgunClip = _shotgunClipSize;
        UpdateStaminaBar();
        AddScore(0);
    }

    void Update()
    {
        _zombieTimer += Time.deltaTime;

        if (_zombieTimer >= ZombieSpawnInterval && !GameOver)
        {
            SpawnZombie();
            _zombieTimer = 0.0f;
        }

        if (Input.GetButton("Reload") && !Reloading )
        {
            if (PistolEquipped && PistolClip < _pistolClipSize || ShotgunEquipped && ShotgunClip < _shotgunClipSize)
            {
                Reloading = true;
                _reloadTimerText.gameObject.SetActive(true);
            }
        }

        if (Reloading)
        {
            _reloadTimer += Time.deltaTime;
            int time;
            if (ShotgunEquipped)
            {
                if (_reloadTimer >= _shotgunReloadTime)
                {
                    ReloadShotgun();
                    _reloadTimer = 0.0f;
                    _reloadTimerText.gameObject.SetActive(false);
                }
                else
                {
                    time = (int)(_shotgunReloadTime - _reloadTimer + 1);
                    _reloadTimerText.text = time.ToString();
                }
            }
            else
            {
                if (_reloadTimer >= _pistolReloadTime)
                {
                    ReloadPistol();
                    _reloadTimer = 0.0f;
                    _reloadTimerText.gameObject.SetActive(false);
                }
                else
                {
                    time = (int)(_pistolReloadTime - _reloadTimer + 1);
                    _reloadTimerText.text = time.ToString();
                }
            }
        }

        if (Input.GetButton("Sprint") && Stamina >= 0.0f && !Shooting && (Input.GetAxis("Horizontal") != 0.0f || Input.GetAxis("Vertical") != 0.0f))
        {
            Stamina -= _staminaDrainPerSecond * Time.deltaTime;
            UpdateStaminaBar();
        }
        else if (Stamina < 100 && !Input.GetButton("Sprint"))
        {
            Stamina += _staminaRegenPerSecond * Time.deltaTime;
            UpdateStaminaBar();
        }

        if (!GameOver)
        {
            UpdateTimer();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Cursor.visible = true;
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void SpawnZombie()
    {
        Vector3 spawnPoint = PatrolPoints[Random.Range(0, PatrolPoints.Length - 1)].transform.position;

        if (Vector3.Distance(Player.transform.position, spawnPoint) < _zombieSpawnDistance)
        {
            foreach (GameObject point in PatrolPoints)
            {
                if (Vector3.Distance(Player.transform.position, point.transform.position) > _zombieSpawnDistance)
                {
                    spawnPoint = point.transform.position;
                }
            }
        }

        Instantiate(_zombie, spawnPoint, transform.rotation);
    }

    public void UpdateAmmoCountPistol()
    {
        _ammoCounter.text = _pistolSpareAmmo.ToString();

        if (_pistolSpareAmmo == 0)
        {
            _ammoCounter.color = Color.red;
        }
        else
        {
            _ammoCounter.color = Color.white;
        }

        _clipAmmoCounter.text = PistolClip.ToString();

        if (PistolClip == 0)
        {
            _clipAmmoCounter.color = Color.red;
        }
        else
        {
            _clipAmmoCounter.color = Color.white;
        }
    }

    public void UpdateAmmoCountShotgun()
    {
        _ammoCounter.text = _shotgunSpareAmmo.ToString();

        if (_shotgunSpareAmmo == 0)
        {
            _ammoCounter.color = Color.red;
        }
        else
        {
            _ammoCounter.color = Color.white;
        }

        _clipAmmoCounter.text = ShotgunClip.ToString();

        if (ShotgunClip == 0)
        {
            _clipAmmoCounter.color = Color.red;
        }
        else
        {
            _clipAmmoCounter.color = Color.white;
        }
    }

    public void AddPistolAmmo(int ammoAmount)
    {
        _pistolSpareAmmo += ammoAmount;

        if (PistolEquipped)
        {
            UpdateAmmoCountPistol();
        }
    }

    public void AddShotgunAmmo(int ammoAmount)
    {
        _shotgunSpareAmmo += ammoAmount;

        if (ShotgunEquipped)
        {
            UpdateAmmoCountShotgun();
        }
    }

    private void ReloadPistol()
    {
        if (_pistolSpareAmmo > _pistolClipSize - PistolClip)
        {
            _pistolSpareAmmo -= _pistolClipSize - PistolClip;
            PistolClip = _pistolClipSize;
        }
        else
        {
            PistolClip += _pistolSpareAmmo;
            _pistolSpareAmmo = 0;
        }

        Reloading = false;
        UpdateAmmoCountPistol();
    }

    private void ReloadShotgun()
    {
        if (_shotgunSpareAmmo > _shotgunClipSize - ShotgunClip)
        {
            _shotgunSpareAmmo -= _shotgunClipSize - ShotgunClip;
            ShotgunClip = _shotgunClipSize;
        }
        else
        {
            ShotgunClip += _shotgunSpareAmmo;
            _shotgunSpareAmmo = 0;
        }

        Reloading = false;
        UpdateAmmoCountShotgun();
    }

    private void UpdateStaminaBar()
    {
        _staminaCount.text = ((int)Stamina).ToString();

        if (Stamina <= 0.0f)
        {
            _staminaCount.color = Color.red;
        }
        else
        {
            _staminaCount.color = Color.white;
        }
    }

    public void GameEnd()
    {
        GameOver = true;
        _gameOverText.SetActive(true);
        if (Highscore < _score)
        {
            Highscore = _score;
            _newHighscoreText.SetActive(true);
            PlayerPrefs.SetInt("highscore", Highscore);
        }
    }

    public void AddScore(int score)
    {
        _score += score;
        _scoreCount.text = _score.ToString();
    }

    private void UpdateTimer()
    {
        _timerSeconds += Time.deltaTime;

        if (_timerSeconds >= 60.0f)
        {
            _timerMinutes++;
            _timerSeconds -= 60.0f;
            _minuteCount.text = _timerMinutes.ToString();
            AddScore(50);
        }

        _secondCount.text = ((int)_timerSeconds).ToString();
    }
}
