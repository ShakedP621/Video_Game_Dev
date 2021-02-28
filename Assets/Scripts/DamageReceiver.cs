using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReceiver : MonoBehaviour, IEntity
{
    public float playerHP = 100;

    [SerializeField] private PlayerController _playerController;


    public void ApplyDamage(float points)
    {
        playerHP -= points;
        if (playerHP <= 0)
        {
            _playerController.canMove = false;
            playerHP = 0;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
