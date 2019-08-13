using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int target = 0;
    [SerializeField] private Transform exitPoint;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float navigationUpdate;
    [SerializeField] private int healthPoints;
    [SerializeField] private int reward;

    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private Transform exitPoint2; 

    private Transform enemy;
    private Collider2D enemyCollider;
    private Animator anim;
    private float navigationTime = 0;

    public bool IsDead { get; private set; } = false;

    


    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Transform>();
        enemyCollider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        GameManager.Instance.RegisterEnemy(this);
    }

    // Update is called once per frame
    void Update()
    {
        var scene1 = SceneManager.GetSceneByBuildIndex(1);
        var scene2 = SceneManager.GetSceneByBuildIndex(2);

        if (scene2.isLoaded)
        {
            waypoints = wayPoints;
            exitPoint = exitPoint2;
        } 
        
        if(waypoints != null && !IsDead)
        {
            navigationTime += Time.deltaTime;
            if (navigationTime > navigationUpdate) 
            {
                if(target < waypoints.Length)
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, waypoints[target].position, navigationTime);
                }
                else
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, exitPoint.position, navigationTime);
                }
                navigationTime = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Checkpoint")
        {
            target += 1;
        }
        else if (collision.tag == "Finish")
        {
            GameManager.Instance.RoundEscape += 1;
            GameManager.Instance.TotalEscape += 1;
            GameManager.Instance.UnregisterEnemy(this);
            GameManager.Instance.WaveOVer();
        }
        else if(collision.tag == "Projectile")
        {
            Projectile newP = collision.gameObject.GetComponent<Projectile>();
            if(newP != null)
             EnemyHit(newP.AttackStrenght);

            Destroy(collision.gameObject);
        }
        
    }

    public void EnemyHit(int hitPoints)
    {
        if (healthPoints - hitPoints > 0)
        { 
            healthPoints -= hitPoints;
            //anim
            anim.Play("Hurt");
            GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Hit);
        }
        else
        {
            //anim
            anim.SetTrigger("didDie");
            Die();
        }
    }

    public void Die()
    {
        IsDead = true;
        enemyCollider.enabled = false; //I dalje je target...

        GameManager.Instance.TotalKilled += 1;
        GameManager.Instance.AddMOney(reward);
        GameManager.Instance.WaveOVer();
        GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Death);
    }
}
