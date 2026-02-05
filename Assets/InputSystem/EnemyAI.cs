using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float damageRange = 1.5f;
    public float damage = 10f;
    public float damageCooldown = 1f;

    private NavMeshAgent agent;
    private float nextDamageTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        agent.SetDestination(player.position);

        if (player == null) return;

        // Keep chasing the player
        agent.SetDestination(player.position);

        // Damage if close
        if (Vector3.Distance(transform.position, player.position) <= damageRange)
        {
            if (Time.time >= nextDamageTime)
            {
                player.GetComponent<PlayerHealth>()?.TakeDamage(damage);
                nextDamageTime = Time.time + damageCooldown;
            }
        }
    }
    void OnDestroy(){
        Debug.Log("Monster Died");
    }
}
