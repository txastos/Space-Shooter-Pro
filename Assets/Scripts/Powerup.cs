using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed=3.0f;
    [SerializeField] //0 -> Triple Laser 1 -> Speed 2 -> Shields
    private int _powerupID;


    // Update is called once per frame
    void Update()
    {
        
        gameObject.transform.Translate(Vector3.down*_speed*Time.deltaTime);
        if (gameObject.transform.position.y < -7f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (player!= null)
            {   

                player.Powerup(_powerupID);
            }
                        
            Destroy(this.gameObject);
        }
    }
}
