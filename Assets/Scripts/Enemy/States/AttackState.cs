using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    private float moveTimer;
    private float losePlayerTimer;
    private float shotTimer;
    public override void Enter()
    {
        enemy.audioSource.PlayOneShot(enemy.discoverySound);
        // Move towards the last known position of the player
        if (enemy.LastKnowPos != Vector3.zero)
        {
            enemy.Agent.SetDestination(enemy.LastKnowPos);
        }
    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer())
        {
            if (enemy.isDead)
            {
                return;
            }

            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;
            shotTimer += Time.deltaTime;
            enemy.transform.LookAt(enemy.Player.transform);


            if (shotTimer > enemy.fireRate)
            {
                Shoot();
            }

            if (moveTimer > Random.Range(3, 7))
            {
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 5));
                moveTimer = 0;
            }
            enemy.LastKnowPos = enemy.Player.transform.position;
        }
        else
        {
            losePlayerTimer += Time.deltaTime;
            if (losePlayerTimer > 8)
            {
                stateMachine.ChangeState(new SearchState());
                enemy.audioSource.PlayOneShot(enemy.lostSound);
            }
        }
    }

    public void Shoot()
    {
        if (enemy.isDead)
        {
            return;
        }
        Transform gunBarrel = enemy.gunBarrel;
        GameObject bullet = GameObject.Instantiate(Resources.Load("Prefabs/Bullet") as GameObject, gunBarrel.position, enemy.transform.rotation);

        Vector3 shootDirection = (enemy.Player.transform.position - gunBarrel.transform.position).normalized;

        bullet.GetComponent<Rigidbody>().velocity = Quaternion.AngleAxis(Random.Range(-3f, 3f), Vector3.up) * shootDirection * 40;

        enemy.audioSource.PlayOneShot(enemy.shootSound);

        shotTimer = 0;
    }

    public override void Exit()
    {

    }
}
