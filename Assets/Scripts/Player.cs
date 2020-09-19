using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleLaserPrefab;
    private float _canFire;
    [SerializeField]
    private float _fireRate = 0.15f;
    [SerializeField] private int _lives = 3;
    private SpawnManager _spawnManager;
    private bool _isTripleLaserActive = false;
    private bool _isSpeedPowerupActive = false;
    private bool _isShieldActive = false;
    [SerializeField] private int _score=0;
    private UIManager _uiManager;
    [SerializeField] private GameObject[] _enginesDamaged;
    private int _engineDamagedFirst;

    //variable to store the audio clip
    [SerializeField] private AudioClip [] _PlayerSoundClip;
    private AudioSource _audioSource;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }

        if (_uiManager == null)
        {
            Debug.Log("The UI Manager is NULL.");
        }

        if (_audioSource == null)
        {
            Debug.LogError("Audiosource is NULL");
        }
 


        _uiManager.Update_Lives(3);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && ((_canFire + _fireRate) <= Time.time))
        {
            FireLaser();
        }

    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        
        if (_isSpeedPowerupActive == false) { 
            transform.Translate(direction * _speed * Time.deltaTime);
        } else
        {
           transform.Translate(direction * _speed *2* Time.deltaTime);
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -5f, 5), 0);

        if (transform.position.x > 11)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        }
        else if (transform.position.x < -11)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }

    }

    void FireLaser()
    {
        _canFire = Time.time;
        
        if (_isTripleLaserActive == true)
        {
            Instantiate(_tripleLaserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }
        else
        { 
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

        //play the laser audio clip
        _audioSource.clip = _PlayerSoundClip[0];
        _audioSource.Play();
    }

    public void Damage()
    {
        if (_isShieldActive == true)
        {
            _isShieldActive = false;
            ShieldVisible(_isShieldActive);
            return;
        }    
            
        _lives -= 1;

        DamageEngines();


        _uiManager.Update_Lives(_lives);

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            _uiManager.Game_Over_Sequence();
            Destroy(this.gameObject);
        }
    }

    public void Powerup(int powerup)
    {
        _audioSource.clip = _PlayerSoundClip[1];
        _audioSource.Play();

        switch (powerup)
        {
        case 0:
                StartCoroutine(TripleShotPowerupRoutine());
                break;
        case 1:
                StartCoroutine(SpeedPowerupRoutine());
            break;
        case 2:
            _isShieldActive = true;
            ShieldVisible(_isShieldActive);
            break;
        }

    }

    private IEnumerator TripleShotPowerupRoutine()
    {
        _isTripleLaserActive = true;
        yield return new WaitForSeconds(5f);
        _isTripleLaserActive = false;
    }

    private IEnumerator SpeedPowerupRoutine()
    {
        _isSpeedPowerupActive = true;
        yield return new WaitForSeconds(5f);
        _isSpeedPowerupActive = false;
    }

    private void ShieldVisible (bool shieldVisible)
    {
        Transform shield = transform.GetChild(0);
        shield.gameObject.SetActive(shieldVisible);
    }


    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    private void DamageEngines()
    {

        if (_lives == 2)
        {
            _engineDamagedFirst = Random.Range(0, 2);
            _enginesDamaged[_engineDamagedFirst].gameObject.SetActive(true);

        }
        else if (_lives == 1)
        {
            if (_engineDamagedFirst == 0)
            {
                _enginesDamaged[1].gameObject.SetActive(true);
            }
            else if (_engineDamagedFirst == 1)
            {
                _enginesDamaged[0].gameObject.SetActive(true);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag== "EnemyLaser")
        {
            Damage();
            Destroy(other.gameObject);
        }
    }
}

