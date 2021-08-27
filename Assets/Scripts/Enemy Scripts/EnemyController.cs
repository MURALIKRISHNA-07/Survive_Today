using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum EnemyState {
    PATROL,
    CHASE,
    ATTACK,
    REST
}

public class EnemyController : MonoBehaviour {

    private Animator enemy_Anim;
    private NavMeshAgent navAgent;

    private EnemyState enemy_State;

    public float walk_Speed = 0.5f;
    public float run_Speed = 4f;

    public float chase_Distance = 5f;
    private float current_Chase_Distance;
    public float attack_Distance = 1.8f;
    public float chase_After_Attack_Distance = 2f;

    public float patrol_Radius_Min = 20f, patrol_Radius_Max = 60f;
    public float patrol_For_This_Time = 15f;
    private float patrol_Timer;

    public float wait_Before_Attack = 2f;
    private float attack_Timer;

    public float RestTime=5f;
    private float RestTimeCounter;

    private Transform target;

    public GameObject attack_Point;

    //private EnemyAudio enemy_Audio;

    public float maxhealth;
    public float Currenthealth;
    public Image healthImage;

    [HideInInspector] public bool aggro = false;

    public PlayerStats playerStats;

    private bool Att;

    void Awake() {
        enemy_Anim = GetComponentInChildren<Animator>();
        navAgent = GetComponent<NavMeshAgent>();

        target = GameObject.FindWithTag(Tags.PLAYER_TAG).transform;

        // enemy_Audio = GetComponentInChildren<EnemyAudio>();
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
    }

    private void OnEnable()
    {
        setMax_Health();
    }

    // Use this for initialization
    void Start () {

        enemy_State = EnemyState.PATROL;

        //patrol_Timer = patrol_For_This_Time;

        // when the enemy first gets to the player
        // attack right away
        attack_Timer = wait_Before_Attack;

        // memorize the value of chase distance
        // so that we can put it back
        current_Chase_Distance = chase_Distance;

        setMax_Health();
        attack_Point = playerStats.gameObject;
        SetNewRandomDestination();

        RestTime = Random.Range(3, 7);
        patrol_For_This_Time = Random.Range(10, 20);

    }

    // Update is called once per frame
    void Update () {
        if (enemy_State == EnemyState.REST)
        {
            TakeRest();
            //Debug.Log("Resting");
        }
        if (enemy_State == EnemyState.PATROL) {
            Patrol();
           // Debug.Log("pATROLING");
        }

        if(enemy_State == EnemyState.CHASE) {
            Chase();
          //  Debug.Log("Chasing");
        }

        if (enemy_State == EnemyState.ATTACK) {
            Attack();
            enemy_Anim.SetBool("Hit", true);
           // Debug.Log("attacking");
        }
        else
        {
            enemy_Anim.SetBool("Hit", false);
        }

        if (Currenthealth <= 0)
        {
            //gameObject.SetActive(false);

            GameManager.instance.DeadEnemy(this.gameObject);
        }

        healthImage.fillAmount = Currenthealth / maxhealth;

        if(aggro)
        {
            chase_Distance = 20f;
            run_Speed = Random.Range(10f, 15f);
;       }

      //  Debug.Log(navAgent.speed);
        enemy_Anim.SetFloat("WalkSpeed", navAgent.speed);

    }

    void Patrol() {

        // tell nav agent that he can move
        navAgent.isStopped = false;
        navAgent.speed = walk_Speed;

        // add to the patrol timer
        patrol_Timer += Time.deltaTime;

        if(patrol_Timer > patrol_For_This_Time) {


            SetNewRandomDestination();
            enemy_State = EnemyState.REST;
            patrol_Timer = 0f;
            

        }

        if (navAgent.velocity.sqrMagnitude > 0) {

            //enemy_Anim.Walk(true);
           // enemy_Anim.IdleToWalk();


        } else {

            //enemy_Anim.Walk(false);
          //  enemy_Anim.WalkToIdle();

        }

        // test the distance between the player and the enemy
        if(Vector3.Distance(transform.position, target.position) <= chase_Distance) {

            //enemy_Anim.Walk(false);

            enemy_State = EnemyState.CHASE;

            // play spotted audio
            //enemy_Audio.Play_ScreamSound();

        }


    } // patrol

    void Chase() {

        // enable the agent to move again
        navAgent.isStopped = false;
        navAgent.speed = run_Speed;

        // set the player's position as the destination
        // because we are chasing(running towards) the player
        navAgent.SetDestination(target.position);


        transform.LookAt(target);

        if (navAgent.velocity.sqrMagnitude > 0) {

            //enemy_Anim.Run(true);

        } else {

            //enemy_Anim.Run(false);

        }

        // if the distance between enemy and player is less than attack distance
        if(Vector3.Distance(transform.position, target.position) <= attack_Distance) {

            // stop the animations
            //enemy_Anim.Run(false);
            //enemy_Anim.Walk(false);
            enemy_State = EnemyState.ATTACK;
            // reset the chase distance to previous
            if(chase_Distance != current_Chase_Distance) {
                chase_Distance = current_Chase_Distance;
            }

        } else if(Vector3.Distance(transform.position, target.position) > chase_Distance) {
            // player run away from enemy

            // stop running
            //enemy_Anim.Run(false);

            enemy_State = EnemyState.PATROL;

            // reset the patrol timer so that the function
            // can calculate the new patrol destination right away
            //patrol_Timer = patrol_For_This_Time;

            // reset the chase distance to previous
            if (chase_Distance != current_Chase_Distance) {
                chase_Distance = current_Chase_Distance;
            }

            if (Vector3.Distance(transform.position, target.position) < attack_Distance)
            {
               // Debug.Log("Changed state to attack");
                enemy_State = EnemyState.ATTACK;
                //transform.GetComponentInChildren<Transform>().rotation = Quaternion.Euler(Vector3.zero);


            }

        } // else

    } // chase

    public void TakeRest()
    {
        navAgent.isStopped = true;
        navAgent.speed = 0;
        navAgent.velocity = Vector3.zero;


        RestTimeCounter += Time.deltaTime;

        if (RestTimeCounter > RestTime)
        {
            enemy_State = EnemyState.PATROL;
            RestTimeCounter = 0;
        }
        if (Vector3.Distance(transform.position, target.position) < attack_Distance + chase_After_Attack_Distance)
        {

            enemy_State = EnemyState.CHASE;

        }
    }

    void Attack() {

        //Debug.LogError("attacking");
        navAgent.velocity = Vector3.zero;
        navAgent.isStopped = true;

        attack_Timer += Time.deltaTime;
        playerStats.is_Player = true;

        transform.LookAt(target);


        if (attack_Timer > wait_Before_Attack) {

            //enemy_Anim.Attack();
            CamShake.Camshake = true;
    
            playerStats.ApplyDamage(5f);
            attack_Timer = 0f;

            //playerStats.ApplyDamage(5f);
            // play attack sound

            //enemy_Audio.Play_AttackSound();
            playerStats.is_Player = false;
        }

        if(Vector3.Distance(transform.position, target.position) >
           attack_Distance + chase_After_Attack_Distance)
        {

            enemy_State = EnemyState.CHASE;

            transform.GetComponentInChildren<Transform>().rotation = Quaternion.Euler(Vector3.zero);

        }


    } // attack

    void SetNewRandomDestination() {

        float rand_Radius = Random.Range(patrol_Radius_Min, patrol_Radius_Max);

        Vector3 randDir = Random.insideUnitSphere * rand_Radius;
        randDir += transform.position;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDir, out navHit, rand_Radius, -1);

        navAgent.SetDestination(navHit.position);
        

    }

    void Turn_On_AttackPoint() {
        attack_Point.SetActive(true);
    }

    void Turn_Off_AttackPoint() {
        if (attack_Point.activeInHierarchy) {
            attack_Point.SetActive(false);
        }
    }

    public EnemyState Enemy_State {
        get; set;
    }


    IEnumerator attackDelay()
    {
        yield return new WaitForSeconds(2f);
        playerStats.ApplyDamage(5f);

    }

    public void setMax_Health()
    {
        Currenthealth = maxhealth;
    }
} // class


































