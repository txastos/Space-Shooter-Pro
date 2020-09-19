using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _gameOver;
    [SerializeField] private Text _restartText;
    [SerializeField] private Image _LivesImg;
    [SerializeField] private Sprite[] _liveSprite;
    private GameManager _gameManager;
    //private bool _isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {

        _gameOver.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _scoreText.text = "Score: " + 0;
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("GameManager is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void UpdateScore (int PlayerScore)
    {
        _scoreText.text = "Score: " + PlayerScore.ToString();
    }

    public void Update_Lives(int currentLives)
    {
        _LivesImg.sprite = _liveSprite[currentLives];
    }

    public void Game_Over_Sequence()
    {
        StartCoroutine(FlickeringText( _gameOver));
        _restartText.gameObject.SetActive(true);
        _gameManager.GameOver();

    }

    IEnumerator FlickeringText(Text text)
    {
        while (true)
        {
            text.gameObject.SetActive(!text.IsActive());
            yield return new WaitForSeconds(0.5f);
        }
    }
}
