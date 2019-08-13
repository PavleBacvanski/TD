using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    [SerializeField] private float timeBetweanAttack;
    [SerializeField] private float attackRadius;
    [SerializeField] private Projectile projectile;

    private Enemy targetEnemy = null;
    private float attackCounter;
    private bool isAttacking = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        attackCounter -= Time.deltaTime;
        if(targetEnemy == null || targetEnemy.IsDead)
        {
            if(GetNearstEnemyInRange() != null && Vector2.Distance(transform.position, GetNearstEnemyInRange().transform.position) <= attackRadius)
            {
                targetEnemy = GetNearstEnemyInRange();
            }
        }
        else
        {
            if (attackCounter <= 0)
            {
                isAttacking = true;
                attackCounter = timeBetweanAttack;
            }
            else
                isAttacking = false;

            //stop
            if (Vector2.Distance(transform.position, targetEnemy.transform.position) > attackRadius)
                targetEnemy = null;
        }

        
    }

    private void FixedUpdate()
    {
        if (isAttacking)
            Attack();
    }

    public void Attack()
    {
        isAttacking = false;
        Projectile newProjectile = Instantiate(projectile) as Projectile;
        newProjectile.transform.localPosition = transform.localPosition;

        if(newProjectile.ProjectileType == ProjectileType.arrow)
          GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Arrow);
        
        else if(newProjectile.ProjectileType == ProjectileType.fireball)
          GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Fireball);
        
        else if(newProjectile.ProjectileType == ProjectileType.rock)
          GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Rock); 
        


        if (targetEnemy == null)
            Destroy(newProjectile);
        else
        {
            //pomeri projektil
            StartCoroutine(MoveProjectile(newProjectile));
        }
    }

    IEnumerator MoveProjectile(Projectile projectile)
    {
        while(GetTargetDIstance(targetEnemy) > 0.20f && projectile != null && targetEnemy != null)
        {
            //info
            var direction = targetEnemy.transform.localPosition - transform.localPosition;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            projectile.transform.localPosition = Vector2.MoveTowards(projectile.transform.localPosition, targetEnemy.transform.localPosition, 5f * Time.deltaTime);
            yield return null;
        }

        if (projectile != null || targetEnemy == null)
            Destroy(projectile);
    }


    private float GetTargetDIstance(Enemy thisEnemy)
    {
        if(thisEnemy == null)
           thisEnemy = GetNearstEnemyInRange();
        if (thisEnemy == null)
            return 0f;

        return Mathf.Abs(Vector2.Distance(transform.localPosition, thisEnemy.transform.localPosition));
        
    }

    private List<Enemy> GetEnemiesInRange()  //naci poziciju towera i enemija i ako je u radijusu dodaj
    {
        List<Enemy> enemiesInRange = new List<Enemy>();
        foreach(Enemy enemy in GameManager.Instance.EnemyList)
        {
            if(Vector2.Distance(transform.localPosition, enemy.transform.localPosition) <= attackRadius)
            {
                enemiesInRange.Add(enemy);
            }
        }

        return enemiesInRange;
    }

    private Enemy GetNearstEnemyInRange()
    {
        Enemy nearestEnemy = null;
        float smallestDistance = float.PositiveInfinity;
        foreach(Enemy enemy in GetEnemiesInRange())
        {
            if(Vector2.Distance(transform.localPosition, enemy.transform.localPosition) < smallestDistance)
            {
                smallestDistance = Vector2.Distance(transform.localPosition, enemy.transform.localPosition);
                nearestEnemy = enemy;
            }
        }
        return nearestEnemy;
    }

}
