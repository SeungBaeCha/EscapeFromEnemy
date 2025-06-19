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

    // 기존 발소리 설정을 열거형과 구조체로 관리
    public enum FootstepType { Walk, Run } /* Modified */
    [System.Serializable] /* Modified */
    public struct FootstepSetting /* Modified */
    {
        public FootstepType type;    /* Modified */
        public AudioClip clip;       /* Modified */
        public float volume;         /* Modified */
    }
    public FootstepSetting[] footstepSettings; /* Modified */

    /* Modified: 플레이어와의 거리에 따라 볼륨을 조절하기 위한 최대 거리.
       이 거리 이상이면 효과음은 0 볼륨 처리됩니다. */
    [Header("Audio Attenuation Settings")] /* Modified */
    public float maxAudioDistance; /* Modified */

    // Modified: enemy의 시야(정면 여부)를 결정하기 위한 각도 설정 (전체 시야각)
    [Header("EnemyChase Angle Settings")] /* Modified */
    public float viewAngle; /* Modified */

    // Modified: 플레이어가 enemy 주변에 매우 가까이 있을 때, 즉시 쫓도록 하는 임계치 (예: 5미터)
    [Header("Close Chase Settings")] /* Modified */
    public float closeChaseDistance; /* Modified */

    private NavMeshAgent agent;
    private float patrolTimer;
    private float chaseTimer;
    private bool isChasing = false;

    private AudioSource audioSource;
    // 현재 재생 중인 발소리 타입과 기본 볼륨을 기록 (클립 길이가 끝날 때까지 유지)
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

        // Modified: 플레이어가 detectionRange 내에 있으며, enemy가 플레이어 정면을 바라보거나 매우 가까이 있을 경우에만 추적
        if (distanceToPlayer <= detectionRange && (IsPlayerInSight() || distanceToPlayer <= closeChaseDistance)) /* Modified */
        {
            isChasing = true;
            chaseTimer = 0f;
            ChasePlayer();
            PlayFootstep(FootstepType.Run); // Modified: 달릴 때는 Run 효과음 재생
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
                // Modified: chaseDurationTime 동안 계속 추적 시 새 효과음이 필요하면 재생하도록 함
                ChasePlayer();
                PlayFootstep(FootstepType.Run); // Modified: 계속해서 Run 효과음 재생
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
            PlayFootstep(FootstepType.Walk); // Modified: 배회 시 Walk 효과음 재생
        }
    }

    /* Modified: enemy의 정면(시야)에서 플레이어가 보이는지 판별.
       enemy의 forward 벡터와 플레이어 방향 벡터 사이의 각도를 계산하여,
       viewAngle의 절반 이내면 true를 반환합니다. */
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
       기존에는 같은 타입의 효과음이 재생 중이면 볼륨만 업데이트하여 클립이 계속 이어졌는데,
       이제 클립이 거의 종료되었거나(예, clip.length - 0.1초 이상 지난 경우)
       또는 새로운 타입이 요청되면 새 랜덤 클립으로 교체하여 씬 내내 효과음이 계속 랜덤하게 바뀌도록 합니다.
    */
    void PlayFootstep(FootstepType type)
    {
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);
        float attenuationFactor = 1f - Mathf.Clamp01(distanceToPlayer / maxAudioDistance);

        // 만약 오디오가 재생 중일 경우...
        if (audioSource.isPlaying)
        {
            // 같은 타입이면서 아직 클립이 거의 다 재생되지 않았다면 볼륨만 업데이트하고 그대로 둡니다.
            if (currentFootstepType == type && audioSource.time < audioSource.clip.length - 0.1f)
            {
                audioSource.volume = currentBaseVolume * attenuationFactor;
                return;
            }
        }

        // 새 클립 선택: clip이 끝났거나 새로운 타입이 요청되었을 때
        FootstepSetting? setting = GetRandomFootstepSetting(type);
        if (setting == null) return;
        currentFootstepType = type;
        AudioClip clip = setting.Value.clip;
        currentBaseVolume = setting.Value.volume;
        float effectiveVolume = currentBaseVolume * attenuationFactor;

        audioSource.clip = clip;
        audioSource.volume = effectiveVolume;
        audioSource.loop = false;  // 클립이 끝나면 자동 정지
        audioSource.Play();
    }

    /* Modified: 주어진 발소리 타입에 해당하는 설정들 중에서 랜덤으로 하나를 반환 */
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