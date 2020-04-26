using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class UnitrisGameplayLoop : MonoBehaviour
{
    public GameObject BlockPrefab;
    public Button StartButton;
    public Dropdown LevelDropdown;
    public Text Score;
    public Text Level;
    public Text Lines;
    public GameObject GameOver;

    [SerializeField] private GameObject _newHighscoreText;
    [SerializeField] private Text _highscoreText;
    [SerializeField] private AudioClip _click;
    [SerializeField] private AudioClip _levelUp;
    [SerializeField] private AudioClip _clearRow;
    [SerializeField] private AudioClip _theEnd;

    private static int HighScore;

    private readonly GameObject[,] _blocks = new GameObject[10,20];
    private readonly GameObject[] _nextBlock = new GameObject[4];
    private GameObject[] _tetrisBlock = new GameObject[4];
    private AudioSource _audioSource;
    private int _blockType;
    private int _nextBlockType;
    private int _blockRotation;
    private float _interval;
    private int _score;
    private int _level;
    private int _lines;
    private float _timer;
    private bool _gameOver = true;
    private bool _softDrop;

    // Start is called before the first frame update
    void Start()
    {
        HighScore = PlayerPrefs.GetInt("unitrisHighscore", 0);
        _audioSource = GetComponent<AudioSource>();
        StartButton.onClick.AddListener(OnClick);
        LevelDropdown.onValueChanged.AddListener(delegate {OnValueChanged(LevelDropdown);});
        _nextBlockType = Random.Range(0, 7);
        _nextBlock[0] = Instantiate(BlockPrefab, new Vector3(0,0, -200), gameObject.transform.rotation);
        _nextBlock[1] = Instantiate(BlockPrefab, new Vector3(0, 0, -200), gameObject.transform.rotation);
        _nextBlock[2] = Instantiate(BlockPrefab, new Vector3(0, 0, -200), gameObject.transform.rotation);
        _nextBlock[3] = Instantiate(BlockPrefab, new Vector3(0, 0, -200), gameObject.transform.rotation);
        _highscoreText.text = "HIGH SCORE: " + HighScore;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_gameOver)
        {
            _timer += Time.deltaTime;
            if (_timer >= _interval)
            {
                Descent();
                _timer = 0;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _audioSource.PlayOneShot(_click);
                Descent();
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _audioSource.PlayOneShot(_click);
                bool collision = false;
                foreach (GameObject block in _tetrisBlock)
                {
                    if (block.transform.position.x <= 0)
                    {
                        collision = true;
                        break;
                    }

                    if (block.transform.position.y < 20 &&
                        _blocks[(int) block.transform.position.x - 1, (int) block.transform.position.y] != null)
                    {
                        collision = true;
                        break;
                    }
                }

                if (!collision)
                {
                    foreach (GameObject block in _tetrisBlock)
                    {
                        block.transform.position += Vector3.left;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                _audioSource.PlayOneShot(_click);
                bool collision = false;
                foreach (GameObject block in _tetrisBlock)
                {
                    if (block.transform.position.x >= 9)
                    {
                        collision = true;
                        break;
                    }

                    if (block.transform.position.y < 20 &&
                        _blocks[(int) block.transform.position.x + 1, (int) block.transform.position.y] != null)
                    {
                        collision = true;
                        break;
                    }
                }

                if (!collision)
                {
                    foreach (GameObject block in _tetrisBlock)
                    {
                        block.transform.position += Vector3.right;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _audioSource.PlayOneShot(_click);
                if (_blockType == 0) //I-Piece
                {
                    if (_blockRotation == 0)
                    {
                        if (!CollisionChecker(0, 2, 2) && !CollisionChecker(1, 1, 1) && !CollisionChecker(3, -1, -1))
                        {
                            _tetrisBlock[0].transform.position += new Vector3(2, 2, 0);
                            _tetrisBlock[1].transform.position += new Vector3(1, 1, 0);
                            _tetrisBlock[3].transform.position += new Vector3(-1, -1, 0);
                            _blockRotation++;
                        }
                    }
                    else
                    {
                        if (!CollisionChecker(0, -2, -2) && !CollisionChecker(1, -1, -1) && !CollisionChecker(3, 1, 1))
                        {
                            _tetrisBlock[0].transform.position += new Vector3(-2, -2, 0);
                            _tetrisBlock[1].transform.position += new Vector3(-1, -1, 0);
                            _tetrisBlock[3].transform.position += new Vector3(1, 1, 0);
                            _blockRotation = 0;
                        }
                    }
                }

                else if (_blockType == 1) //J-Piece
                {
                    if (_blockRotation == 0)
                    {
                        if (!CollisionChecker(0, 1, 1) && !CollisionChecker(2, -1, -1) && !CollisionChecker(3, -2, 0))
                        {
                            _tetrisBlock[0].transform.position += new Vector3(1, 1, 0);
                            _tetrisBlock[2].transform.position += new Vector3(-1, -1, 0);
                            _tetrisBlock[3].transform.position += new Vector3(-2, 0, 0);
                            _blockRotation++;
                        }
                    }
                    else if (_blockRotation == 1)
                    {
                        if (!CollisionChecker(0, 1, -1) && !CollisionChecker(2, -1, 1) && !CollisionChecker(3, 0, 2))
                        {
                            _tetrisBlock[0].transform.position += new Vector3(1, -1, 0);
                            _tetrisBlock[2].transform.position += new Vector3(-1, 1, 0);
                            _tetrisBlock[3].transform.position += new Vector3(0, 2, 0);
                            _blockRotation++;
                        }
                    }
                    else if (_blockRotation == 2)
                    {
                        if (!CollisionChecker(0, -1, -1) && !CollisionChecker(2, 1, 1) && !CollisionChecker(3, 2, 0))
                        {
                            _tetrisBlock[0].transform.position += new Vector3(-1, -1, 0);
                            _tetrisBlock[2].transform.position += new Vector3(1, 1, 0);
                            _tetrisBlock[3].transform.position += new Vector3(2, 0, 0);
                            _blockRotation++;
                        }
                    }
                    else if (_blockRotation == 3)
                    {
                        if (!CollisionChecker(0, -1, 1) && !CollisionChecker(2, 1, -1) && !CollisionChecker(3, 0, -2))
                        {
                            _tetrisBlock[0].transform.position += new Vector3(-1, 1, 0);
                            _tetrisBlock[2].transform.position += new Vector3(1, -1, 0);
                            _tetrisBlock[3].transform.position += new Vector3(0, -2, 0);
                            _blockRotation = 0;
                        }
                    }
                }
                else if (_blockType == 2) //L-Piece
                {
                    if (_blockRotation == 0)
                    {
                        if (!CollisionChecker(0, 1, 1) && !CollisionChecker(2, -1, -1) && !CollisionChecker(3, 0, 2))
                        {
                            _tetrisBlock[0].transform.position += new Vector3(1, 1, 0);
                            _tetrisBlock[2].transform.position += new Vector3(-1, -1, 0);
                            _tetrisBlock[3].transform.position += new Vector3(0, 2, 0);
                            _blockRotation++;
                        }
                    }
                    else if (_blockRotation == 1)
                    {
                        if (!CollisionChecker(0, 1, -1) && !CollisionChecker(2, -1, 1) && !CollisionChecker(3, 2, 0))
                        {
                            _tetrisBlock[0].transform.position += new Vector3(1, -1, 0);
                            _tetrisBlock[2].transform.position += new Vector3(-1, 1, 0);
                            _tetrisBlock[3].transform.position += new Vector3(2, 0, 0);
                            _blockRotation++;
                        }
                    }
                    else if (_blockRotation == 2)
                    {
                        if (!CollisionChecker(0, -1, -1) && !CollisionChecker(2, 1, 1) && !CollisionChecker(3, 0, -2))
                        {
                            _tetrisBlock[0].transform.position += new Vector3(-1, -1, 0);
                            _tetrisBlock[2].transform.position += new Vector3(1, 1, 0);
                            _tetrisBlock[3].transform.position += new Vector3(0, -2, 0);
                            _blockRotation++;
                        }
                    }
                    else if (_blockRotation == 3)
                    {
                        if (!CollisionChecker(0, -1, 1) && !CollisionChecker(2, 1, -1) && !CollisionChecker(3, -2, 0))
                        {
                            _tetrisBlock[0].transform.position += new Vector3(-1, 1, 0);
                            _tetrisBlock[2].transform.position += new Vector3(1, -1, 0);
                            _tetrisBlock[3].transform.position += new Vector3(-2, 0, 0);
                            _blockRotation = 0;
                        }
                    }
                }
                else if (_blockType == 3) //S-Piece
                {
                    if (_blockRotation == 0)
                    {
                        if (!CollisionChecker(2, 1, 2) && !CollisionChecker(3, 1, 0))
                        {
                            _tetrisBlock[2].transform.position += new Vector3(1, 2, 0);
                            _tetrisBlock[3].transform.position += new Vector3(1, 0, 0);
                            _blockRotation++;
                        }
                    }
                    else
                    {
                        if (!CollisionChecker(2, -1, -2) && !CollisionChecker(3, -1, 0))
                        {
                            _tetrisBlock[2].transform.position += new Vector3(-1, -2, 0);
                            _tetrisBlock[3].transform.position += new Vector3(-1, 0, 0);
                            _blockRotation = 0;
                        }
                    }
                }
                else if (_blockType == 4) //T-Piece
                {
                    if (_blockRotation == 0)
                    {
                        if (!CollisionChecker(2, -1, 1))
                        {
                            _tetrisBlock[2].transform.position += new Vector3(-1, 1, 0);
                            _blockRotation++;
                        }
                    }
                    else if (_blockRotation == 1)
                    {
                        if (!CollisionChecker(3, 1, 1))
                        {
                            _tetrisBlock[3].transform.position += new Vector3(1, 1, 0);
                            _blockRotation++;
                        }
                    }
                    else if (_blockRotation == 2)
                    {
                        if (!CollisionChecker(0, 1, -1))
                        {
                            _tetrisBlock[0].transform.position += new Vector3(1, -1, 0);
                            _blockRotation++;
                        }
                    }
                    else if (_blockRotation == 3)
                    {
                        if (!CollisionChecker(0, -1, 1) && !CollisionChecker(2, 1, -1) && !CollisionChecker(3, -1, -1))
                        {
                            _tetrisBlock[0].transform.position += new Vector3(-1, 1, 0);
                            _tetrisBlock[2].transform.position += new Vector3(1, -1, 0);
                            _tetrisBlock[3].transform.position += new Vector3(-1, -1, 0);
                            _blockRotation = 0;
                        }
                    }
                }
                else if (_blockType == 5) //Z-Piece
                {
                    if (_blockRotation == 0)
                    {
                        if (!CollisionChecker(0, 2, 1) && !CollisionChecker(3, 0, 1))
                        {
                            _tetrisBlock[0].transform.position += new Vector3(2, 1, 0);
                            _tetrisBlock[3].transform.position += new Vector3(0, 1, 0);
                            _blockRotation++;
                        }
                    }
                    else
                    {
                        if (!CollisionChecker(0, -2, -1) && !CollisionChecker(3, 0, -1))
                        {
                            _tetrisBlock[0].transform.position += new Vector3(-2, -1, 0);
                            _tetrisBlock[3].transform.position += new Vector3(0, -1, 0);
                            _blockRotation = 0;
                        }
                    }
                }
            }

            if (Input.GetKey(KeyCode.Space) && !_softDrop)
            {
                _softDrop = true;
                Invoke("SoftDrop", 0.05f);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("Unitris");
        }
    }

    private void OnClick()//Kun painetaan Starttinappulaa
    {
        Level.text = "LEVEL: " + _level;
        _interval = 1 - (0.1f * _level);
        StartButton.gameObject.SetActive(false);
        LevelDropdown.gameObject.SetActive(false);
        Level.gameObject.SetActive(true);
        Score.gameObject.SetActive(true);
        Lines.gameObject.SetActive(true);
        BlockCreator();
        _gameOver = false;
    }

    private void OnValueChanged(Dropdown levelSelect)
    {
        _level = levelSelect.value;
    }

    private void BlockCreator()
    {
        _tetrisBlock = new GameObject[4];
        _blockType = _nextBlockType;
        _blockRotation = 0;

        if (_blockType == 0)//I-piece
        {
            _tetrisBlock[0] = Instantiate(BlockPrefab, new Vector3(3, 21, 0), gameObject.transform.rotation);
            _tetrisBlock[1] = Instantiate(BlockPrefab, new Vector3(4, 21, 0), gameObject.transform.rotation);
            _tetrisBlock[2] = Instantiate(BlockPrefab, new Vector3(5, 21, 0), gameObject.transform.rotation);
            _tetrisBlock[3] = Instantiate(BlockPrefab, new Vector3(6, 21, 0), gameObject.transform.rotation);
        }

        else if (_blockType == 1)//J-piece
        {
            _tetrisBlock[0] = Instantiate(BlockPrefab, new Vector3(3, 21, 0), gameObject.transform.rotation);
            _tetrisBlock[1] = Instantiate(BlockPrefab, new Vector3(4, 21, 0), gameObject.transform.rotation);
            _tetrisBlock[2] = Instantiate(BlockPrefab, new Vector3(5, 21, 0), gameObject.transform.rotation);
            _tetrisBlock[3] = Instantiate(BlockPrefab, new Vector3(5, 20, 0), gameObject.transform.rotation);
        }

        else if (_blockType == 2)//L-piece
        {
            _tetrisBlock[0] = Instantiate(BlockPrefab, new Vector3(3, 21, 0), gameObject.transform.rotation);
            _tetrisBlock[1] = Instantiate(BlockPrefab, new Vector3(4, 21, 0), gameObject.transform.rotation);
            _tetrisBlock[2] = Instantiate(BlockPrefab, new Vector3(5, 21, 0), gameObject.transform.rotation);
            _tetrisBlock[3] = Instantiate(BlockPrefab, new Vector3(3, 20, 0), gameObject.transform.rotation);
        }

        else if (_blockType == 3)//S-piece
        {
            _tetrisBlock[0] = Instantiate(BlockPrefab, new Vector3(4, 21, 0), gameObject.transform.rotation);
            _tetrisBlock[1] = Instantiate(BlockPrefab, new Vector3(5, 21, 0), gameObject.transform.rotation);
            _tetrisBlock[2] = Instantiate(BlockPrefab, new Vector3(3, 20, 0), gameObject.transform.rotation);
            _tetrisBlock[3] = Instantiate(BlockPrefab, new Vector3(4, 20, 0), gameObject.transform.rotation);
        }

        else if (_blockType == 4)//T-piece
        {
            _tetrisBlock[0] = Instantiate(BlockPrefab, new Vector3(3, 21, 0), gameObject.transform.rotation);
            _tetrisBlock[1] = Instantiate(BlockPrefab, new Vector3(4, 21, 0), gameObject.transform.rotation);
            _tetrisBlock[2] = Instantiate(BlockPrefab, new Vector3(5, 21, 0), gameObject.transform.rotation);
            _tetrisBlock[3] = Instantiate(BlockPrefab, new Vector3(4, 20, 0), gameObject.transform.rotation);
        }

        else if (_blockType == 5)//Z-piece
        {
            _tetrisBlock[0] = Instantiate(BlockPrefab, new Vector3(3, 21, 0), gameObject.transform.rotation);
            _tetrisBlock[1] = Instantiate(BlockPrefab, new Vector3(4, 21, 0), gameObject.transform.rotation);
            _tetrisBlock[2] = Instantiate(BlockPrefab, new Vector3(4, 20, 0), gameObject.transform.rotation);
            _tetrisBlock[3] = Instantiate(BlockPrefab, new Vector3(5, 20, 0), gameObject.transform.rotation);
        }

        else//O-piece
        {
            _tetrisBlock[0] = Instantiate(BlockPrefab, new Vector3(4, 21, 0), gameObject.transform.rotation);
            _tetrisBlock[1] = Instantiate(BlockPrefab, new Vector3(5, 21, 0), gameObject.transform.rotation);
            _tetrisBlock[2] = Instantiate(BlockPrefab, new Vector3(4, 20, 0), gameObject.transform.rotation);
            _tetrisBlock[3] = Instantiate(BlockPrefab, new Vector3(5, 20, 0), gameObject.transform.rotation);
        }

        BlockPainter(_tetrisBlock, _blockType);
        NextBlock();
    }

    private void BlockPainter(GameObject[] blocks, int color)
    {
        foreach (GameObject block in blocks)
        {
            Renderer rend = block.GetComponent<Renderer>();

            if (color == 0)
            {
                rend.material.color = Color.cyan;
            }

            else if (color == 1)
            {
                rend.material.color = Color.blue;
            }

            else if (color == 2)
            {
                rend.material.color = new Color(1f, 0.498f, 0, 1f);
            }

            else if (color == 3)
            {
                rend.material.color = Color.green;
            }

            else if (color == 4)
            {
                rend.material.color = new Color(0.502f, 0f, 0.502f, 1f);
            }

            else if (color == 5)
            {
                rend.material.color = Color.red;
            }

            else if (color == 6)
            {
                rend.material.color = Color.yellow;
            }
        }
    }

    private bool CollisionChecker(int i, int x, int y)
    {
        bool collision = false;

        if ((int)_tetrisBlock[i].transform.position.x + x > 9 || (int)_tetrisBlock[i].transform.position.x + x < 0 || (int)_tetrisBlock[i].transform.position.y + y < 0)
        {
            collision = true;
        }

        else if ((int)_tetrisBlock[i].transform.position.y + y < 20 && _blocks[(int)_tetrisBlock[i].transform.position.x + x, (int)_tetrisBlock[i].transform.position.y + y] != null)
        {
            collision = true;
        }

        return collision;
    }
    private void Descent()
    {
        bool collision = false;

        foreach (GameObject block in _tetrisBlock)
        {
            if (block.transform.position.y <= 0 || block.transform.position.y < 20 && _blocks[(int)block.transform.position.x, (int)block.transform.position.y - 1] != null)
            {
                collision = true;
                break;
            }
        }

        if (!collision)
        {
            foreach (GameObject block in _tetrisBlock)
            {
                block.transform.position += Vector3.down;
            }
        }

        else
        {
            foreach (GameObject block in _tetrisBlock)
            {
                if (block.transform.position.y >= 20)
                {
                    _gameOver = true;
                    break;
                }
            }

            if (!_gameOver)
            {
                bool clearLine = false;

                foreach (GameObject block in _tetrisBlock)
                {
                    _blocks[(int) block.transform.position.x, (int) block.transform.position.y] = block;
                }

                int multiplier = 0;
                for (int y = 0; y < 20; y++)
                {
                    int counter = 0;
                    for (int x = 0; x < 10; x++)
                    {
                        if (_blocks[x, y] != null)
                        {
                            counter++;
                        }
                    }

                    if (counter == 10)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            Destroy(_blocks[i, y]);
                            _blocks[i, y] = null;
                            
                        }

                        _lines++;
                        clearLine = true;
                        Lines.text = "LINES: " + _lines;
                        multiplier++;
                        for (int j = y + 1; j < 20; j++)
                        {
                            for (int k = 0; k < 10; k++)
                            {
                                if (_blocks[k, j] != null)
                                {
                                    _blocks[k, j].transform.position += Vector3.down;
                                    _blocks[k, j - 1] = _blocks[k, j];
                                    _blocks[k, j] = null;
                                }
                            }
                        }

                        y--;
                    }
                }

                if (_lines >= (_level + 1) * 10)
                {
                    _level++;
                    _audioSource.PlayOneShot(_levelUp);
                    if (_level < 10)
                    {
                        _interval -= 0.1f;
                    }
                    else
                    {
                        _interval -= 0.01f;
                    }
                    Level.text = "LEVEL: " + _level;
                }
                else if (clearLine)
                {
                    _audioSource.PlayOneShot(_clearRow);
                }
                _score += 10 * (_level + 1) + 100 * multiplier * (_level + 1);
                Score.text = "SCORE: " + _score;
                BlockCreator();

            }
            else
            {
                GameOver.SetActive(true);
                if (HighScore < _score)
                {
                    PlayerPrefs.SetInt("unitrisHighscore", _score);
                    _newHighscoreText.SetActive(true);
                }
                _audioSource.PlayOneShot(_theEnd);
            }
        }
    }

    private void NextBlock()
    {
        _nextBlockType = Random.Range(0, 7);
        if (_nextBlockType == 0)//I
        {
            _nextBlock[0].transform.position = new Vector3(-5, 1, 0);
            _nextBlock[1].transform.position = new Vector3(-4, 1, 0);
            _nextBlock[2].transform.position = new Vector3(-3, 1, 0);
            _nextBlock[3].transform.position = new Vector3(-2, 1, 0);
        }
        else if (_nextBlockType == 1)//J
        {
            _nextBlock[0].transform.position = new Vector3(-4.5f, 1.5f, 0);
            _nextBlock[1].transform.position = new Vector3(-3.5f, 1.5f, 0);
            _nextBlock[2].transform.position = new Vector3(-2.5f, 1.5f, 0);
            _nextBlock[3].transform.position = new Vector3(-2.5f, 0.5f, 0);
        }
        else if (_nextBlockType == 2)//L
        {
            _nextBlock[0].transform.position = new Vector3(-4.5f, 1.5f, 0);
            _nextBlock[1].transform.position = new Vector3(-3.5f, 1.5f, 0);
            _nextBlock[2].transform.position = new Vector3(-2.5f, 1.5f, 0);
            _nextBlock[3].transform.position = new Vector3(-4.5f, 0.5f, 0);
        }
        else if (_nextBlockType == 3)//S
        {
            _nextBlock[0].transform.position = new Vector3(-4.5f, 0.5f, 0);
            _nextBlock[1].transform.position = new Vector3(-3.5f, 0.5f, 0);
            _nextBlock[2].transform.position = new Vector3(-3.5f, 1.5f, 0);
            _nextBlock[3].transform.position = new Vector3(-2.5f, 1.5f, 0);
        }
        else if (_nextBlockType == 4)//T
        {
            _nextBlock[0].transform.position = new Vector3(-4.5f, 1.5f, 0);
            _nextBlock[1].transform.position = new Vector3(-3.5f, 1.5f, 0);
            _nextBlock[2].transform.position = new Vector3(-2.5f, 1.5f, 0);
            _nextBlock[3].transform.position = new Vector3(-3.5f, 0.5f, 0);
        }
        else if (_nextBlockType == 5)//Z
        {
            _nextBlock[0].transform.position = new Vector3(-4.5f, 1.5f, 0);
            _nextBlock[1].transform.position = new Vector3(-3.5f, 1.5f, 0);
            _nextBlock[2].transform.position = new Vector3(-3.5f, 0.5f, 0);
            _nextBlock[3].transform.position = new Vector3(-2.5f, 0.5f, 0);

        }
        else//O
        {
            _nextBlock[0].transform.position = new Vector3(-4f, 1.5f, 0);
            _nextBlock[1].transform.position = new Vector3(-3f, 1.5f, 0);
            _nextBlock[2].transform.position = new Vector3(-4f, 0.5f, 0);
            _nextBlock[3].transform.position = new Vector3(-3f, 0.5f, 0);
        }
        BlockPainter(_nextBlock, _nextBlockType);
    }

    private void SoftDrop()
    {
        _audioSource.PlayOneShot(_click);
        Descent();
        _softDrop = false;
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ResetHighScore()
    {
        HighScore = 0;
        PlayerPrefs.SetInt("unitrisHighscore", HighScore);
        _highscoreText.text = "HIGH SCORE: " + HighScore;
    }
}
