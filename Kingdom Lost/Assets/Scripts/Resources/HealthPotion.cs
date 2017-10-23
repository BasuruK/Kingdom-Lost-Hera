using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : ResourceController {


	public int RefillAmount;
	private GameObject m_KingController;
	private KingController m_PlayerScript;
	private HealthController m_HealthControllerScript;
	private ParticleSystem m_PlayerParticleSystem;
	private AudioSource m_PlayerParticalAudioSource;
	private int m_PlayerHealth;

	// Use this for initialization
	void Start () {
		m_KingController = GameObject.FindGameObjectWithTag ("MainCharacterController");
		m_PlayerScript = m_KingController.GetComponent<KingController> ();
		m_HealthControllerScript = m_KingController.GetComponent<HealthController> ();
		m_PlayerParticleSystem = GameObject.FindGameObjectWithTag("HealthParticaleAnimation").GetComponent<ParticleSystem> ();
		m_PlayerParticalAudioSource = GameObject.FindGameObjectWithTag ("HealthParticaleAnimation").GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		GetValuesFromPlayer ();
	}

	void OnCollisionEnter(Collision Col)
	{
		if (Col.gameObject.tag == "MainCharacter" || Col.gameObject.name == "King Dutugamunu") {
			if (m_PlayerHealth != 100) {
				m_HealthControllerScript.SendMessage ("IncreaseHealthByPotion", RefillAmount);
				//Play Smoke Animation
				m_PlayerParticleSystem.Play();
				// Play Health Absorb Sound
				m_PlayerParticalAudioSource.Play ();
				Destroy (gameObject);
			}
		}
	}

	// Take the Values from the Player
	void GetValuesFromPlayer()
	{
		m_PlayerHealth = m_PlayerScript.m_health;
	}
}
