using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SecretScript : MonoBehaviour
{
    [SerializeField] private GameObject _secretButton;
    [SerializeField] private AudioClip _secretClip;

    private int _secretCounter;
    private AudioSource _audio;
    private bool _secret;

    void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.anyKeyDown && !_secret)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && (_secretCounter == 0 || _secretCounter == 1))
            {
                _secretCounter++;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && (_secretCounter == 2 || _secretCounter == 3))
            {
                _secretCounter++;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && (_secretCounter == 4 || _secretCounter == 6))
            {
                _secretCounter++;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow)&& (_secretCounter == 5 || _secretCounter == 7))
            {
                _secretCounter++;
            }
            else if (Input.GetKeyDown(KeyCode.B) && _secretCounter == 8)
            {
                _secretCounter++;
            }
            else if (Input.GetKeyDown(KeyCode.A) && _secretCounter == 9)
            {
                _secret = true;
                _audio.PlayOneShot(_secretClip);
                _secretButton.SetActive(true);
            }
            else
            {
                _secretCounter = 0;
            }
        }
    }

    public void Secret()
    {
        SceneManager.LoadScene("Unitris");
    }
}
