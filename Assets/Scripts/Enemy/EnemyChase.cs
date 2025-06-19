using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour
{
    public Transform target;
    public float detectionRange;
    public float patrolDelay;
    public float patrolSearchRadius = 100f;
    public float chaseDurationTime;

    // ���� �߼Ҹ� ������ �������� ����ü�� ����
    public enum FootstepType { Walk, Run } /* Modified */
    [System.Serializable] /* Modified */
    public struct FootstepSetting /* Modified */
    {
        public FootstepType type;    /* Modified */
        public AudioClip clip;       /* Modified */
        public float volume;         /* Modified */
    }
    public FootstepSetting[] footstepSettings; /* Modified */

    /* Modified: �÷��̾���� �Ÿ��� ���� ������ �����ϱ� ���� �ִ� �Ÿ�.
       �� �Ÿ� �̻��̸� ȿ������ 0 ���� ó���˴ϴ�. */
    [Header("Audio Attenuation Settings")] /* Modified */
    public float maxAudioDistance; /* Modified */

    // Modified: enemy�� �þ�(���� ����)�� �����ϱ� ���� ���� ���� (��ü �þ߰�)
    [Header("EnemyChase Angle Settings")] /* Modified */
    public float viewAngle; /* Modified */

    // Modified: �÷��̾ enemy �ֺ��� �ſ� ������ ���� ��, ��� �ѵ��� �ϴ� �Ӱ�ġ (��: 5����)
    [Header("Close Chase Settings")] /* Modified */
    public float closeChaseDistance; /* Modified */

    private NavMeshAgent agent;
    private float patrolTimer;
    private float chaseTimer;
    private bool isChasing = false;

    private AudioSource audioSource;
    // ���� ��� ���� �߼Ҹ� Ÿ�԰� �⺻ ������ ��� (Ŭ�� ���̰� ���� ������ ����)
    private FootstepType currentFootstepType; /* Modified */
    private float currentBaseVolume;          /* Modified */

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

        patrolTimer = patrolDelay;
        chaseTimer = 0f;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        // Modified: �÷��̾ detectionRange ���� ������, enemy�� �÷��̾� ������ �ٶ󺸰ų� �ſ� ������ ���� ��쿡�� ����
        if (distanceToPlayer <= detectionRange && (IsPlayerInSight() || distanceToPlayer <= closeChaseDistance)) /* Modified */
        {
            isChasing = true;
            chaseTimer = 0f;
            ChasePlayer();
            PlayFootstep(FootstepType.Run); // Modified: �޸� ���� Run ȿ���� ���
        }
        else if (isChasing)
        {
            chaseTimer += Time.deltaTime;

            if (chaseTimer >= chaseDurationTime)
            {
                isChasing = false;
                patrolTimer = patrolDelay;
            }
            else
            {
                // Modified: chaseDurationTime ���� ��� ���� �� �� ȿ������ �ʿ��ϸ� ����ϵ��� ��
                ChasePlayer();
                PlayFootstep(FootstepType.Run); // Modified: ����ؼ� Run ȿ���� ���
            }
        }
        else
        {
            patrolTimer += Time.deltaTime;

            if (patrolTimer >= patrolDelay || agent.remainingDistance < 0.5f)
            {
                Vector3 randomPoint;
                if (GetRandomPointOnNavMesh(patrolSearchRadius, out randomPoint))
                {
                    NavMeshPath path = new NavMeshPath();
                    if (agent.CalculatePath(randomPoint, path) && path.status == NavMeshPathStatus.PathComplete)
                    {
                        agent.SetDestination(randomPoint);
                    }
                }
                patrolTimer = 0f;
            }
            PlayFootstep(FootstepType.Walk); // Modified: ��ȸ �� Walk ȿ���� ���
        }
    }

    /* Modified: enemy�� ����(�þ�)���� �÷��̾ ���̴��� �Ǻ�.
       enemy�� forward ���Ϳ� �÷��̾� ���� ���� ������ ������ ����Ͽ�,
       viewAngle�� ���� �̳��� true�� ��ȯ�մϴ�. */
    bool IsPlayerInSight() /* Modified */
    {
        Vector3 directionToPlayer = (target.position - transform.position).normalized; /* Modified */
        float angle = Vector3.Angle(transform.forward, directionToPlayer); /* Modified */
        return angle < viewAngle * 0.5f; /* Modified */
    }

    void ChasePlayer()
    {
        NavMeshPath pathToPlayer = new NavMeshPath();
        agent.CalculatePath(target.position, pathToPlayer);

        if (pathToPlayer.status == NavMeshPathStatus.PathComplete)
        {
            agent.SetDestination(target.position);
        }
        else if (pathToPlayer.status == NavMeshPathStatus.PathPartial && pathToPlayer.corners.Length > 1)
        {
            Vector3 lastValidCorner = pathToPlayer.corners[pathToPlayer.corners.Length - 1];
            agent.SetDestination(lastValidCorner);
        }
        else
        {
            Vector3 directionToPlayer = (target.position - transform.position).normalized;
            Vector3 offsetPosition = target.position - directionToPlayer * 1.5f;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(offsetPosition, out hit, 3f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }
    }

    bool GetRandomPointOnNavMesh(float radius, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = Random.insideUnitSphere * radius;
            randomPoint += transform.position;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 5f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    /* Modified:
       �������� ���� Ÿ���� ȿ������ ��� ���̸� ������ ������Ʈ�Ͽ� Ŭ���� ��� �̾����µ�,
       ���� Ŭ���� ���� ����Ǿ��ų�(��, clip.length - 0.1�� �̻� ���� ���)
       �Ǵ� ���ο� Ÿ���� ��û�Ǹ� �� ���� Ŭ������ ��ü�Ͽ� �� ���� ȿ������ ��� �����ϰ� �ٲ�� �մϴ�.
    */
    void PlayFootstep(FootstepType type)
    {
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);
        float attenuationFactor = 1f - Mathf.Clamp01(distanceToPlayer / maxAudioDistance);

        // ���� ������� ��� ���� ���...
        if (audioSource.isPlaying)
        {
            // ���� Ÿ���̸鼭 ���� Ŭ���� ���� �� ������� �ʾҴٸ� ������ ������Ʈ�ϰ� �״�� �Ӵϴ�.
            if (currentFootstepType == type && audioSource.time < audioSource.clip.length - 0.1f)
            {
                audioSource.volume = currentBaseVolume * attenuationFactor;
                return;
            }
        }

        // �� Ŭ�� ����: clip�� �����ų� ���ο� Ÿ���� ��û�Ǿ��� ��
        FootstepSetting? setting = GetRandomFootstepSetting(type);
        if (setting == null) return;
        currentFootstepType = type;
        AudioClip clip = setting.Value.clip;
        currentBaseVolume = setting.Value.volume;
        float effectiveVolume = currentBaseVolume * attenuationFactor;

        audioSource.clip = clip;
        audioSource.volume = effectiveVolume;
        audioSource.loop = false;  // Ŭ���� ������ �ڵ� ����
        audioSource.Play();
    }

    /* Modified: �־��� �߼Ҹ� Ÿ�Կ� �ش��ϴ� ������ �߿��� �������� �ϳ��� ��ȯ */
    FootstepSetting? GetRandomFootstepSetting(FootstepType type)
    {
        List<FootstepSetting> matches = new List<FootstepSetting>(); /* Modified */
        foreach (FootstepSetting fs in footstepSettings) /* Modified */
        {
            if (fs.type == type) /* Modified */
            {
                matches.Add(fs); /* Modified */
            }
        }
        if (matches.Count > 0) /* Modified */
        {
            int index = Random.Range(0, matches.Count); /* Modified */
            return matches[index]; /* Modified */
        }
        return null; /* Modified */
    }
}