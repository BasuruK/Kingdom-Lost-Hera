using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walkerController_L2 : MonoBehaviour
{

    public GameObject healthBar;
    public AudioClip hit_by_sword_clip;
    public AudioClip death_clip;

    private Animator anim;
    private AudioSource audioSource;
    private Renderer healthBar_renderer;
    private GameObject player;

    // Controllers
    private float health = 100;
    private float walk_step = 0.05f;
    private float speed = 1f;
    private bool first_walk = false;
    private bool walking = false;
    private float walk_distance = 0;
    private RaycastHit hit = new RaycastHit();
    private string direction = "left";
    private float state = 0;
    private bool near = false;
    private bool attack = false;
    private bool get_hit = false;
    private bool dead = false;
    private float offset_x = 0, offset_y = 0;

    public float GroundBlockDistance = 697.54f;
    public float WaterBlockStart = 146f;
    public float WaterBlockEnd = 168f;

    public float BridgeBlockStart = 314f;
    public float BridgeBlockEnd = 377f;
    public float BridgeBlockheight = 6.7455f;

    public float BoxBlockStart = 514f;
    public float BoxBlockEnd = 545f;

    private float TempWaterStart = 0;
    private float TempBridgeStart = 0;
    private float TempWaterEnd = 0;
    private float TempBridgeEnd = 0;
    private float TempBoxStart = 0;
    private float TempBoxEnd = 0;


    private float _x = 0;

    private bool enemyDead = false;
    private float currentDistance = 0;

    AnimatorStateInfo animationState;
    AnimatorClipInfo[] myAnimatorClip;

    private bool CyclopHit = false;
    private Vector3 cyclopPos = new Vector3(0, 0, 0);


    public int NoHits = 4;
    private float maxhealth = 100;
    private float currentHealth = 0;

    bool WaterBlockFound = false;
    bool BridgeFound = false;
    bool BoxFound = false;

    private float updatePosition = 0;
    private bool updatestatus = true;

    RaycastHit hitRight;

    // Use this for initialization
    void Start()
    {
        this.anim = gameObject.GetComponent<Animator>();
        this.audioSource = gameObject.GetComponent<AudioSource>();
        this.healthBar_renderer = this.healthBar.GetComponent<Renderer>();
        this.walk_distance = Constant.enemy_margin_right;
        this.anim.SetInteger("state", 1);
        this.player = GameObject.FindGameObjectWithTag(Constant.main_character_l2); // get main player ref
        currentHealth = maxhealth;
        _x = 75.0f;


        currentDistance = this.player.transform.position.x + _x;

        for (int x = 1; x <= 3; x++)
        {
            TempWaterStart = WaterBlockStart;
            TempBridgeStart = BridgeBlockStart;
            TempWaterEnd = WaterBlockEnd;
            TempBridgeEnd = BridgeBlockEnd;
            TempBoxStart = BoxBlockStart;
            TempBoxEnd = BoxBlockEnd;
            if (x > 1)
            {
                TempWaterStart = (WaterBlockStart * x * GroundBlockDistance);
                TempBridgeStart = (BridgeBlockStart * x * GroundBlockDistance);
                TempWaterEnd = (WaterBlockEnd * x * GroundBlockDistance);
                TempBridgeEnd = (BridgeBlockEnd * x * GroundBlockDistance);
                TempBoxStart = (BoxBlockStart * x * GroundBlockDistance);
                TempBoxEnd = (BoxBlockEnd * x * GroundBlockDistance);
            }

            WaterBlockFound = ((currentDistance > TempWaterStart) && (currentDistance < TempWaterEnd));
            BridgeFound = ((currentDistance > TempBridgeStart) && (currentDistance < TempBridgeEnd));
            BoxFound = ((currentDistance > TempBoxStart) && (currentDistance < TempBoxEnd));

            if (WaterBlockFound)
            {
                currentDistance = currentDistance + (WaterBlockStart + WaterBlockEnd);
                break;
            }

            if (BridgeFound)
            {
                currentDistance = currentDistance + (BridgeBlockStart + BridgeBlockEnd);
                break;
            }

            if (BoxFound)
            {
                currentDistance = currentDistance + (BoxBlockStart + BoxBlockEnd);
                break;
            }

        }
        this.gameObject.transform.position = new Vector3(currentDistance, 0, 10);

    }

    // Update is called once per frame
    void Update()
    {
        // make move towards player
        if (!first_walk && this.walk_distance > 0)
        {

            this.anim.SetInteger("attack", 1);
            this.walk_distance -= this.walk_step;

            updatePosition = currentDistance + this.walk_distance;

            for (int x = 1; x <= 3; x++)
            {
                TempWaterStart = WaterBlockStart;
                TempBridgeStart = BridgeBlockStart;
                TempWaterEnd = WaterBlockEnd;
                TempBridgeEnd = BridgeBlockEnd;
                TempBoxStart = BoxBlockStart;
                TempBoxEnd = BoxBlockEnd;
                if (x > 1)
                {
                    TempWaterStart = (WaterBlockStart * x * GroundBlockDistance);
                    TempBridgeStart = (BridgeBlockStart * x * GroundBlockDistance);
                    TempWaterEnd = (WaterBlockEnd * x * GroundBlockDistance);
                    TempBridgeEnd = (BridgeBlockEnd * x * GroundBlockDistance);
                    TempBoxStart = (BoxBlockStart * x * GroundBlockDistance);
                    TempBoxEnd = (BoxBlockEnd * x * GroundBlockDistance);
                }

                WaterBlockFound = ((currentDistance > TempWaterStart) && (currentDistance < TempWaterEnd));
                BridgeFound = ((currentDistance > TempBridgeStart) && (currentDistance < TempBridgeEnd));
                BoxFound = ((currentDistance > TempBoxStart) && (currentDistance < TempBoxEnd));

                if (WaterBlockFound)
                {
                    updatestatus = false;
                    break;
                }
                else
                {
                    updatestatus = true;
                }

                if (BridgeFound)
                {
                    updatestatus = false;
                    break;
                }
                else if (BoxFound)
                {
                    updatestatus = false;
                }
                else
                {
                    updatestatus = true;
                }

            }



            if (updatestatus)
            {
                this.gameObject.transform.position = new Vector3(updatePosition, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
            }
            if (this.walk_distance <= 4)
            {
                this.anim.SetInteger("attack", 0); // stop attack
                this.anim.SetInteger("idle_attack", 0); // stop idle attack
                this.anim.SetInteger("state", 0); // end walking
            }
            else if (this.walk_distance <= 0)
            {
                this.first_walk = true;
            }

        }
        else
        {
            // do action
            this.checkOpponentDistance();
            if (this.near)
            {
                this.doAtack();
            }
        }

        CyclopHitCheck();

    }

    // check opponent distance
    private void checkOpponentDistance()
    {
        if (GameObject.FindGameObjectWithTag(Constant.main_character_l2))
        {
            // opponent found
            if (Physics.Raycast(transform.position, Vector3.left, out hit, 60f))
            {
                if (this.hit.collider.gameObject.tag == Constant.main_character_l2)
                {
                    this.state = -1f;
                    if (this.direction != "left")
                    {
                        // rotate enemy
                        this.gameObject.transform.Rotate(Vector3.up * 180f);
                        // rotate healthbar
                        this.healthBar.transform.Rotate(Vector3.left * 180f);
                    }
                    this.direction = "left";
                }
            }
            if (Physics.Raycast(transform.position, Vector3.right, out hit, 60f))
            {
                if (this.hit.collider.gameObject.tag == Constant.main_character_l2)
                {
                    this.state = +1f;
                    if (this.direction != "right")
                    {
                        // rotate enemy
                        this.gameObject.transform.Rotate(Vector3.up * -180f);
                        // rotate healthbar
                        this.healthBar.transform.Rotate(Vector3.left * -180f);
                    }
                    this.direction = "right";
                }
            }

            if (this.hit.distance > Constant.p_e_gap)
            {
                if (!walking)
                {
                    this.walking = true;
                    this.anim.SetInteger("state", 1); // start walking
                }
                if (this.hit.distance > Constant.p_e_run_gap)
                {
                    this.speed = Constant.level_1_enemy_run;
                    this.anim.SetFloat("walk_speed", Constant.level_1_enemy_run);
                }
                else
                {
                    this.speed = Constant.level_1_enemy_walk;
                    this.anim.SetFloat("walk_speed", Constant.level_1_enemy_walk);
                }



                updatePosition = this.gameObject.transform.position.x + (this.state * this.speed * this.walk_step);

                for (int x = 1; x <= 3; x++)
                {
                    TempWaterStart = WaterBlockStart;
                    TempBridgeStart = BridgeBlockStart;
                    TempWaterEnd = WaterBlockEnd;
                    TempBridgeEnd = BridgeBlockEnd;
                    if (x > 1)
                    {
                        TempWaterStart = (WaterBlockStart * x * GroundBlockDistance);
                        TempBridgeStart = (BridgeBlockStart * x * GroundBlockDistance);
                        TempWaterEnd = (WaterBlockEnd * x * GroundBlockDistance);
                        TempBridgeEnd = (BridgeBlockEnd * x * GroundBlockDistance);
                    }

                    WaterBlockFound = ((updatePosition > TempWaterStart) && (updatePosition < TempWaterEnd));
                    BridgeFound = ((updatePosition > TempBridgeStart) && (updatePosition < TempBridgeEnd));

                    if (WaterBlockFound)
                    {
                        updatestatus = false;
                        break;
                    }
                    else
                    {
                        updatestatus = true;
                    }

                    if (BridgeFound)
                    {
                        updatestatus = false;
                        break;
                    }
                    else
                    {
                        updatestatus = true;
                    }

                }



                if (updatestatus)
                {

                    // walk with player
                    this.gameObject.transform.position = new Vector3(updatePosition, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
                    this.near = false; // update near
                }
            }
            else
            {
                this.walking = false;
                this.anim.SetInteger("state", 0); // end walking
                this.near = true; // update near
            }
        }
    }

    // perform relevant atack to distance
    private void doAtack()
    {
        if (this.hit.distance < Constant.level_2_Walker_attack_mode_1_distance)
        {
            if (!this.attack && !this.anim.GetCurrentAnimatorStateInfo(0).IsTag("Walk_Attack"))
            {
                this.anim.SetInteger("attack", 1);
                this.attack = true;
            }
        }
        else
        {
            this.attack = false;
            this.anim.SetInteger("attack", 0); // move to idle
        }
    }



    void CyclopHitCheck()
    {
        animationState = this.player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        myAnimatorClip = this.player.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0);

        if (this.player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Sword_Attack"))
        {



            if (myAnimatorClip[0].clip.length * animationState.normalizedTime < 0.735f)
            {
                CyclopHit = false;


                for (int var = 0; var < 20; var++)
                {

                    cyclopPos = new Vector3(this.gameObject.transform.position.x, (float)var, this.gameObject.transform.position.z);
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
                    audioSource.clip = hit_by_sword_clip;

                    audioSource.Play();
                    this.anim.SetInteger("idle_hit", 1);
                    currentHealth = currentHealth - (maxhealth / NoHits);
                    float calcHealth = (float)currentHealth / maxhealth;
                    if (currentHealth <= 0)
                    {
                        currentHealth = 0;
                        calcHealth = 0;
                    }


                    changeHealth(calcHealth);
                    if (currentHealth == 0)
                    {
                        this.anim.SetBool("dead", true);
                        this.audioSource.clip = this.death_clip;
                        this.audioSource.Play();

                        if (!enemyDead)
                        {
                            GameObject.Find("player_points").GetComponent<PointsController>().SendMessage("WalkerKill");
                            Destroy(this.gameObject, 2f);
                            enemyDead = true;
                        }

                    }

                }
                else
                {
                    this.anim.SetInteger("idle_hit", 0);
                }
            }
        }


    }

    private void changeHealth(float distance)
    {

        // update health
        this.health = distance;
        healthBar.transform.localScale = new Vector3(distance, healthBar.transform.localScale.y, healthBar.transform.localScale.z);

    }
}
