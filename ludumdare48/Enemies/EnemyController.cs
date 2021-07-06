using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private enum State
    {
        Moving,
        Roaming,
        Attacking
    }

    private event Action OnTargetReached;

    [SerializeField] float speed;
    [SerializeField] int damage;
    [SerializeField] int hp = 1;
    [SerializeField] float dieDelay = 0.2f;
    
    private Transform targetTransform;
    private State state;

    private void OnEnable()
    {
        state = State.Moving;
        OnTargetReached += OnTargetReach;

    }
    
    public void Setup (EnemySO enemyData, Vector3 spawnPosition, Transform liftTransform)
    {
        hp = enemyData.hp;
        speed = enemyData.speed;

        transform.position = spawnPosition;
        state = State.Moving;

        targetTransform = liftTransform;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Attacking:
                break;
            case State.Moving:
                transform.position = Vector2.MoveTowards(transform.position, targetTransform.position, speed * Time.deltaTime);
                transform.up = GetComponent<Rigidbody2D>().velocity;
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log(col.gameObject.name);
        if (col is LiftControl)
        {
            StartCoroutine(DieWithDelay());
            OnTargetReached?.Invoke();
        }
        if (col is Bullet)
        {
            Die();
        }
    }

    private void OnTargetReach()
    {

    }

    private void Die()
    {
        StartCoroutine(DieWithDelay());
        OnTargetReached -= OnTargetReach;
    }

    IEnumerator DieWithDelay()
    {
        yield return new WaitForSeconds(dieDelay);
        Destroy(gameObject);
    }
}
