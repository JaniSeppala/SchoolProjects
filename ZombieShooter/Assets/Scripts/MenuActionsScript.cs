using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuActionsScript : MonoBehaviour
{
    [SerializeField] private GameObject _instructionsPanel;
    [SerializeField] private GameObject _optionsPanel;
    [SerializeField] private GameObject _menuWindow;
    [SerializeField] private Text _highscoreText;
    [SerializeField] private Slider _zombieSpawnIntervalSlider;
    [SerializeField] private Text _zombieSpawnIntervalText;
    [SerializeField] private Slider _volumeSlider;

    private GameController _controller;

    void Start()
    {
        _controller = GameController.Controller;
        GameController.Highscore = PlayerPrefs.GetInt("highscore", 0);
        GameController.SoundVolume = PlayerPrefs.GetFloat("volume", 1.0f);
        AudioListener.volume = GameController.SoundVolume;
        GameController.ZombieSpawnInterval = PlayerPrefs.GetFloat("spawninterval", 10.0f);

        if (_highscoreText != null)
        {
            _highscoreText.text = "HIGH SCORE: " + GameController.Highscore;
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (_instructionsPanel.activeInHierarchy)
            {
                _instructionsPanel.SetActive(false);
            }
            else if (_optionsPanel.activeInHierarchy)
            {
                _optionsPanel.SetActive(false);
            }
            else if (_menuWindow != null && !_controller.GameOver)
            {
                if (_menuWindow.activeInHierarchy)
                {
                    Cursor.visible = false;
                    _menuWindow.SetActive(false);
                    Time.timeScale = 1;
                }
                else
                {
                    Cursor.visible = true;
                    _menuWindow.SetActive(true);
                    Time.timeScale = 0;
                }
            }
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void HowToPlay()
    {
        if (_instructionsPanel.activeInHierarchy)
        {
            _instructionsPanel.SetActive(false);
        }
        else
        {
            _instructionsPanel.SetActive(true);
        }
    }

    public void Options()
    {
        if (_optionsPanel.activeInHierarchy)
        {
            _optionsPanel.SetActive(false);
        }
        else
        {
            _optionsPanel.SetActive(true);
            _volumeSlider.value = GameController.SoundVolume;
            _zombieSpawnIntervalSlider.value = GameController.ZombieSpawnInterval;
        }
    }

    public void SetZombieSpawnInterval()
    {
        GameController.ZombieSpawnInterval = _zombieSpawnIntervalSlider.value;

        if (GameController.ZombieSpawnInterval == 1)
        {
            _zombieSpawnIntervalText.text = GameController.ZombieSpawnInterval + " SECOND";
        }
        else
        {
            _zombieSpawnIntervalText.text = GameController.ZombieSpawnInterval + " SECONDS";
        }

        PlayerPrefs.SetFloat("spawninterval", GameController.ZombieSpawnInterval);
    }

    public void SetSoundVolume()
    {
        GameController.SoundVolume = _volumeSlider.value;
        AudioListener.volume = GameController.SoundVolume;
        PlayerPrefs.SetFloat("volume", GameController.SoundVolume);
    }

    public void ReturnToGame()
    {
        Cursor.visible = false;
        _menuWindow.SetActive(false);
        Time.timeScale = 1;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void ResetHighscore()
    {
        GameController.Highscore = 0;
        PlayerPrefs.SetInt("highscore", 0);
        if (_highscoreText != null)
        {
            _highscoreText.text = "HIGH SCORE: " + GameController.Highscore;
        }
    }
}
