using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOneEnemyAttack : MonoBehaviour
{
    private GameObject mainCharacter;

    // Use this for initialization
    void Start()
    {
        mainCharacter = GameObject.Find("MainCharacterController");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == Constant.main_character)
        {
            this.mainCharacter.GetComponent<HealthController>().SendMessage("MainEnemyHit");
        }
    }
}
