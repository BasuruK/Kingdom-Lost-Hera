using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclopWeaponScript_L1 : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision col)
    {

        if (col.gameObject.name == "King Dutugamunu")
        {
            Debug.Log("Weapon hit " + col.gameObject.name);
            GameObject.FindGameObjectWithTag("MainCharacterController").GetComponent<HealthController>().SendMessage("WalkerHit");
        }
    }
}
