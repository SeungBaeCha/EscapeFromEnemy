using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Attack Setting")]
    public int attackDamage;

    [Header("Attack CoolTime Setting")]
    public float attackCooldown;

    [Header("Stun Setting")]
    public float stunDuration;




    float lastAttackTime = 0f;


    NavMeshAgent agent;
    bool isStunned = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isStunned) return;

        if (other.CompareTag("Player") && Time.time - lastAttackTime >= attackCooldown)
        {
            PlayerHeart player = other.GetComponent<PlayerHeart>();

            if (player != null)
            {
                player.TakeDamage(attackDamage);
                lastAttackTime = Time.time;

                StartCoroutine(MakeStun());
            }
        }
    }

    IEnumerator MakeStun()
    {
        isStunned = true;

        if (agent != null)
            agent.isStopped = true;

        yield return new WaitForSeconds(stunDuration);

        if (agent != null)
            agent.isStopped = false;

        isStunned = false;
    }
}
