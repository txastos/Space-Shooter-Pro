using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _speed = 4.0f;
    private Player _player;
    private Animator _explosion;
    private AudioSource _audioSource;
    [SerializeField] private GameObject _laserPrefab;
    private float _fireRate = 3.0f;
    private float _canFire = -1;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        //StartCoroutine(FireLaserRamdomly());

        if(_player == null)
        {
            Debug.LogError("Player is NULL");
        }

        _explosion = GetComponent<Animator>();

        if(_explosion == null)
        {
            Debug.LogError("Explosion is NULL");
        }
    }



    // Update is called once per frame
    void Update()
    {
        EnemyMovement();
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for(int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    void EnemyMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6.0f)
        {
            transform.position = new Vector3(Random.Range(-9.0f, 9.0f), 5, 0);
        }

    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            _explosion.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(this.gameObject, 2.8f);
            

        } else if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);

            }
            _explosion.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
            
        }
    }

    //IEnumerator FireLaserRamdomly()
    //{
    //    yield return new WaitForSeconds(Random.Range(1,3));
    //    Instantiate(_enemyLaser, transform.position, Quaternion.identity);

    //}
}
