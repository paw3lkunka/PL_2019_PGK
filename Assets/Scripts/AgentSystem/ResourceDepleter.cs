using UnityEngine;

public class ResourceDepleter : MonoBehaviour
{
#pragma warning disable
    [SerializeField] private bool isTimeBased = false;
    [SerializeField] private bool shouldDepleteHealth = false;
    [SerializeField] private float velocityThreshold = 0.01f;
    [SerializeField] private float waterHealthLosingThreshold = 0.01f;

    public float CurrentFaithMultiplier { get; private set; }

    // These properties are left as not-auto to allow for easy modification for getters
    [SerializeField] private float _waterDepletionRate;
    public float WaterDepletionRate
    {
        get => _waterDepletionRate * GameplayManager.Instance.cultistInfos.Count;
        private set => _waterDepletionRate = value;
    }

    [SerializeField] private float _faithDepletionRate;
    public float FaithDepletionRate
    {
        get  
        {
            float finalValue =_faithDepletionRate * CurrentFaithMultiplier;
            if(GameplayManager.Instance.IsAvoidingFight)
            {
                finalValue *= 1.0f + GameplayManager.Instance.avoidingFightsFaithDebuf;
            }
            return finalValue;
        }
        private set => _faithDepletionRate = value;
    }

    [SerializeField] private float _healthDepletionRate;
    public float HealthDepletionRate
    {
        get => _healthDepletionRate;
        private set => _healthDepletionRate = value;
    }
#pragma warning restore

    private Vector3 lastFramePos;
    private float cultistLeaveTime;
    //Change in cult leader prefabs (CultLeader and CultLeader_WorldMap)
    public float leaveCultTime = 30.0f;

    private void Start()
    {
        CurrentFaithMultiplier = 1.0f;
        cultistLeaveTime = 0.0f;
        GameplayManager.Instance.OverfaithStart += OnOverfaithStart;
        GameplayManager.Instance.OverfaithEnd += OnOverfaithEnd;
    }

    private void FixedUpdate()
    {
        if (isTimeBased || (lastFramePos - transform.position).magnitude > velocityThreshold)
        {
            if(GameplayManager.Instance.overfaith)
            {
                CurrentFaithMultiplier += 1.0f;
            }
            GameplayManager.Instance.Water -= WaterDepletionRate;
            GameplayManager.Instance.Faith -= FaithDepletionRate;

            if (shouldDepleteHealth && GameplayManager.Instance.Water <= waterHealthLosingThreshold)
            {
                foreach (var cultistInfo in GameplayManager.Instance.cultistInfos)
                {
                    cultistInfo.HP -= HealthDepletionRate;
                    if (cultistInfo.HP < 0.01f)
                    {
                        GameplayManager.Instance.cultistInfos.Remove(cultistInfo);
                    }
                }
            }

            if(GameplayManager.Instance.Faith <= 0.01f)
            {
                cultistLeaveTime += Time.deltaTime;
                if(cultistLeaveTime >= leaveCultTime)
                {
                    var cultist = GameplayManager.Instance.cultistInfos[Random.Range(0, GameplayManager.Instance.cultistInfos.Count)];
                    GameplayManager.Instance.cultistInfos.Remove(cultist);
                    cultistLeaveTime = 0.0f;
                }
            }
        }
        lastFramePos = transform.position;
    }

    private void OnDisable()
    {
        GameplayManager.Instance.OverfaithStart -= OnOverfaithStart;
        GameplayManager.Instance.OverfaithEnd -= OnOverfaithEnd;
    }

    private void OnOverfaithStart() 
    {
        GameplayManager.Instance.overfaith = true;
        CurrentFaithMultiplier = GameplayManager.Instance.overfaithDepletionMultiplierBase;
    }

    private void OnOverfaithEnd() 
    {
        GameplayManager.Instance.overfaith = false;
        CurrentFaithMultiplier = 1.0f;
    }
}
