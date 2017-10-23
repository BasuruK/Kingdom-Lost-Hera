using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cyclopEnemyController_L1 : MonoBehaviour
{

    private GameObject mainPlayer;

    private float MoveSpeed = 4;
    private float MaxDist = 10;
    private float MinDist = 5;

    private float timeFrequncy = 0.5f;
    private float noOfTimes = 1.0f;

    private float _x = 0;
    private bool _attack_1 = true;

    private float currentDistance = 0;

    public int NoHits = 4;
    private float maxhealth = 100;
    private float currentHealth = 0;

    public GameObject healthBar;

    public AudioClip enemyHitClip;
    public AudioClip enemyDieClip;

    private AudioSource audioSource;

    private Vector3 cyclopPos = new Vector3(0, 0, 0);

    RaycastHit hit, hitRight;

    private bool CyclopHit = false;

    private string direction = "left";

    AnimatorStateInfo animationState;
    AnimatorClipInfo[] myAnimatorClip;

    private bool enemyDead = false;



    // Use this for initialization
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        //mainPlayer = GameObject.FindWithTag ("king_dutugamunu");
        mainPlayer = GameObject.Find("King Dutugamunu");
        currentHealth = maxhealth;
        _x = 75.0f;//Random.Range (0.1f, 5.5f);
        currentDistance = mainPlayer.transform.position.x + _x;
        gameObject.transform.position = new Vector3(currentDistance, 0, 10);

        InvokeRepeating("Attack", timeFrequncy, noOfTimes);





    }

    // Update is called once per frame
    void Update()
    {
        CyclopHitCheck();


    }

    void Attack()
    {

        if (mainPlayer.transform.position.x > gameObject.transform.position.x)
        {
            if (direction == "left")
            {
                // rotate enemy
                gameObject.transform.Rotate(Vector3.up * 180f);
                // rotate healthbar
                //healthBar.transform.Rotate (Vector3.left * 180f);
                direction = "right";
            }
        }
        else
        {
            if (direction == "right")
            {
                // rotate enemy
                gameObject.transform.Rotate(Vector3.up * -180f);
                // rotate healthbar
                //healthBar.transform.Rotate (Vector3.left * -180f);
                direction = "left";
            }
        }

        if ((currentDistance - mainPlayer.transform.position.x) < 25.0f && (currentDistance - mainPlayer.transform.position.x) > -25.0f)
        {

            if (_attack_1)
            {
                if (!gameObject.GetComponent<Animation>().IsPlaying("death"))
                {
                    gameObject.GetComponent<Animation>()["attack_2"].speed = 1.5f;
                    gameObject.GetComponent<Animation>().Play("attack_2");
                    _attack_1 = false;
                }
            }
            else
            {
                if (!gameObject.GetComponent<Animation>().IsPlaying("death"))
                {
                    gameObject.GetComponent<Animation>()["attack_1"].speed = 1.5f;
                    gameObject.GetComponent<Animation>().Play("attack_1");
                    _attack_1 = true;
                }
            }
        }




    }

    void CyclopHitCheck()
    {
        animationState = mainPlayer.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        myAnimatorClip = mainPlayer.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0);

        if (mainPlayer.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Sword_Attack"))
        {



            if (myAnimatorClip[0].clip.length * animationState.normalizedTime < 0.735f)
            {
                CyclopHit = false;


                for (int var = 0; var < 20; var++)
                {

                    cyclopPos = new Vector3(gameObject.transform.position.x, (float)var, gameObject.transform.position.z);
                    if ((Physics.Raycast(cyclopPos, Vector2.left, out hit, 5F)))
                    {
                        if ((hit.collider.gameObject.name == "Sword"))
                        {
                            //Debug.Log ("Weapon hit " + hit.collider.gameObject.name);
                            CyclopHit = true;
                            break;
                        }

                    }
                    else if ((Physics.Raycast(cyclopPos, Vector2.right, out hitRight, 5F)))
                    {

                        if ((hitRight.collider.gameObject.name == "Sword"))
                        {
                            //Debug.Log ("Weapon hit " + hit.collider.gameObject.name);
                            CyclopHit = true;
                            break;
                        }
                    }
                }

                if (CyclopHit == true)
                {
                    audioSource.clip = enemyHitClip;

                    audioSource.Play();
                    gameObject.GetComponent<Animation>()["hit"].speed = 1f;
                    gameObject.GetComponent<Animation>().Play("hit");
                    currentHealth = currentHealth - (maxhealth / NoHits);
                    float calcHealth = (float)currentHealth / maxhealth;
                    if (currentHealth <= 0)
                    {
                        currentHealth = 0;
                        calcHealth = 0;
                    }


                    SetHealthBar(calcHealth);
                    if (currentHealth == 0)
                    {

                        GameObject.Find("player_points").GetComponent<PointsController>().SendMessage("WalkerKill");
                        audioSource.clip = enemyDieClip;

                        audioSource.Play();

                        gameObject.GetComponent<Animation>().Play("death");
                        if (!enemyDead)
                        {
                            GameObject.Find("player_points").GetComponent<PointsController>().SendMessage("WalkerKill");
                            Destroy(gameObject, 1.4f);
                            enemyDead = true;
                        }
                    }

                }
            }
        }


    }

    public void SetHealthBar(float cHealth)
    {

        healthBar.transform.localScale = new Vector3(cHealth, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
    }
}
