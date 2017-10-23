using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTwoEnemyController : EnemyListener
{
    // references
    public GameObject healthBar;
    public AudioClip hit_by_sword_clip;
    public AudioClip death_clip;

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

    private AnimatorStateInfo animationState;
    private AnimatorClipInfo[] myAnimatorClip;

    // Use this for initialization
    void Start()
    {
        Constant.current_level = 2; // set current level
        this.audioSource = gameObject.GetComponent<AudioSource>();
        this.healthBar_renderer = this.healthBar.GetComponent<Renderer>();
        this.walk_distance = Constant.enemy_margin_right;
        this.player = GameObject.FindGameObjectWithTag(Constant.main_character_l2); // get main player ref
    }

    // Update is called once per frame
    void Update()
    {
        // make move towards player
        if (!first_walk && this.walk_distance > 0)
        {
            //this.anim.SetInteger("attack", 1);
            if (!this.gameObject.GetComponent<Animation>().IsPlaying("walk"))
            {
                this.gameObject.GetComponent<Animation>()["walk"].speed = 1f;
                this.gameObject.GetComponent<Animation>().Play("walk");
            }
            this.walk_distance -= this.walk_step;
            this.gameObject.transform.position = new Vector3(Constant.level_2_max + Constant.view_margin_right + this.walk_distance, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
            if (this.walk_distance <= 0)
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
        // update health bar
        displayHealthBar();
    }

    // check opponent distance
    override
    public void checkOpponentDistance()
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
                if (!this.gameObject.GetComponent<Animation>().IsPlaying("walk"))
                {
                    this.gameObject.GetComponent<Animation>()["walk"].speed = 1f;
                    this.gameObject.GetComponent<Animation>().Play("walk");
                }
                if (!walking)
                {
                    this.walking = true;
                    //this.anim.SetInteger("state", 1); // start walking
                }
                if (this.hit.distance > Constant.p_e_run_gap)
                {
                    this.speed = Constant.level_1_enemy_run;
                    //this.anim.SetFloat("walk_speed", Constant.level_1_enemy_run);
                }
                else
                {
                    this.speed = Constant.level_1_enemy_walk;
                    //this.anim.SetFloat("walk_speed", Constant.level_1_enemy_walk);
                }
                // walk with player
                this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x + (this.state * this.speed * this.walk_step), this.gameObject.transform.position.y, this.gameObject.transform.position.z);
                this.near = false; // update near
            }
            else
            {
                this.walking = false;
                if (!this.gameObject.GetComponent<Animation>().IsPlaying("idle"))
                {
                    this.gameObject.GetComponent<Animation>()["idle"].speed = 1f;
                    this.gameObject.GetComponent<Animation>().Play("idle");
                }
                this.near = true; // update near
            }
        }
    }

    // perform relevant atack to distance
    override
    public void doAtack()
    {
        if (this.hit.distance <= Constant.level_2_Walker_attack_mode_1_distance)
        {
            if (!this.gameObject.GetComponent<Animation>().IsPlaying("attack_1"))
            {
                Debug.Log("attack_1");
                this.gameObject.GetComponent<Animation>()["attack_1"].speed = 1f;
                this.gameObject.GetComponent<Animation>().Play("attack_1");
            }
            this.attack = true;
        }
        else
        {
            this.attack = false;
            if (!this.gameObject.GetComponent<Animation>().IsPlaying("idle"))
            {
                Debug.Log("idle");
                this.gameObject.GetComponent<Animation>()["idle"].speed = 1f;
                this.gameObject.GetComponent<Animation>().Play("idle");
            }
        }
    }

    // detect attacks from sword
    void OnTriggerEnter(Collider other)
    {
        if (!this.get_hit && other.gameObject.tag == "sword_l2" && this.player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Sword_Attack"))
        {
            this.changeHealth(this.hit.distance);
            if (!this.gameObject.GetComponent<Animation>().IsPlaying("stunned_idle_hit"))
            {
                this.gameObject.GetComponent<Animation>()["stunned_idle_hit"].speed = 1f;
                this.gameObject.GetComponent<Animation>().Play("stunned_idle_hit");
            }
            this.get_hit = true;
        }
    }

    override
    public void changeHealth(float distance)
    {
        // play audio
        this.audioSource.clip = hit_by_sword_clip;
        this.audioSource.Play();

        if (distance <= Constant.damage_distance_b)
        {
            // update health
            this.health -= Constant.damage_distance_b_points;
        }
        else if (distance <= Constant.damage_distance_a)
        {
            // update health
            this.health -= Constant.damage_distance_a_points;
        }
    }

    override
    public void displayHealthBar()
    {
        //this.anim.SetBool("dead", false);
        // check hit status and sound play to change state
        if (this.get_hit && !this.audioSource.isPlaying)
        {
            //this.anim.SetInteger("idle_hit", 0);
            this.get_hit = false;
        }

        if (this.health > 95)
        {
            this.offset_x = 0;
            this.offset_y = 0.86f;
        }
        else if (this.health > 85)
        {
            this.offset_x = 0;
            this.offset_y = 0.72f;
        }
        else if (this.health > 75)
        {
            this.offset_x = 0;
            this.offset_y = 0.58f;
        }
        else if (this.health > 50)
        {
            this.offset_x = 0;
            this.offset_y = 0.437f;
        }
        else if (this.health > 35)
        {
            this.offset_x = 0;
            this.offset_y = 0.293f;
        }
        else if (this.health > 20)
        {
            this.offset_x = 0;
            this.offset_y = 0.15f;
        }
        else if (this.health > 10)
        {
            this.offset_x = 0;
            this.offset_y = 0;
        }
        else
        {
            if (!this.dead)
            {
                // dead
                this.dead = true;
                //this.anim.SetBool("dead", true);
                this.audioSource.clip = this.death_clip;
                this.audioSource.Play();
                Destroy(this.gameObject, 3f);
                // update level progress
                UIPanelController.getInstance().viewGameOver(2, true);
            }
        }
        this.healthBar_renderer.material.mainTextureOffset = new Vector2(this.offset_x, this.offset_y);
    }
}

