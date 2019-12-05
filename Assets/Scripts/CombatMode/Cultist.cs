using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEngine;

[RequireComponent(typeof(Shooter))]
public class Cultist : Character
{
    private Shooter shooter;
   
    protected override void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
        SceneManager.sceneUnloaded += OnSceneUnload;
        GameManager.Instance.OnGameOver += OnGameOver;

        DontDestroyOnLoad(gameObject);

        string sceneName = SceneManager.GetActiveScene().name;
        if ( sceneName == "MainMap" || sceneName == "MainMenu")
        {
            gameObject.SetActive(false);
        }

        base.Awake();
        shooter = GetComponent<Shooter>();

        Agent.updateRotation = false;
        Agent.updateUpAxis = false;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
        SceneManager.sceneUnloaded -= OnSceneUnload;

        try
        {
            RhythmController.Instance.OnRageModeStart -= ToRageMode;
        }
        catch (System.NullReferenceException) { }

        try
        {
            RhythmController.Instance.OnRageModeEnd -= ToNormalMode;
        }
        catch (System.NullReferenceException) { }
    }

    protected override void Start()
    {
        base.Start();
        RhythmController.Instance.OnRageModeStart += ToRageMode;
        RhythmController.Instance.OnRageModeEnd += ToNormalMode;
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMap" || scene.name == "MainMenu" )
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            CombatSceneManager.Instance.ourCrew.Add(gameObject);
            Agent.Warp(CombatSceneManager.Instance.startPoint + FormationOffset);
        }
    }

    private void OnSceneUnload(Scene scene)
    {
        if (scene.name != "MainMap" && scene.name != "MainMenu")
        {
            CombatSceneManager.Instance.ourCrew.Remove(gameObject);
            CombatSceneManager.Instance.enemies.Remove(gameObject);
        }
    }

    protected override void Update()
    {


        if (RhythmController.Instance.Combo >= 1)
        {
            SetStateOn(CharacterState.CanAttack);
        }
        else
        {
            SetStateOff(CharacterState.CanAttack);
        }

        float distanceToEnemy = CombatSceneManager.Instance.enemies.NearestFrom(transform.position).Item2;
        bool canMove = CheckState(CharacterState.CanMove);
        bool canAttack = CheckState(CharacterState.CanAttack);

        if ( !canAttack || distanceToEnemy > shooter.range )
        {
            shooter.StopShooting();
        }
        
        if ( canMove && Input.GetMouseButtonDown(0) )
        {
            GoToMousePosition();
        }
        
        if ( canAttack && Input.GetMouseButtonDown(1) )
        {
            AimToMousePosition();
            shooter.StartShooting();
        }

        base.Update();
    }

    private void ToRageMode()
    {
        shooter.baseDamage *= 1.5f;
        Agent.speed *= 1.1f;
        defence = .5f;
    }


    private void ToNormalMode()
    {
        shooter.baseDamage /= 1.5f;
        Agent.speed /= 1.1f;
        defence = 0;
    }

    public void GoToMousePosition()
    {
        if(Agent.enabled)
        {
            Agent.SetDestination(CombatSceneManager.Instance.MousePos + FormationOffset);
        }
    }    
    public void AimToMousePosition() => GetComponent<Shooter>().target = CombatSceneManager.Instance.MousePos + FormationOffset;

    public override void TakeDamage(int damage)
    {
        GameManager.Instance.Faith -= GameManager.Instance.FaithForWoundedCultist;
        base.TakeDamage(damage);
    }

    public override void Die()
    {
        GameManager.Instance.OnGameOver -= OnGameOver;
        base.Die();

        float lossedFaith = GameManager.Instance.FaithForKilledCultist;
        GameManager.Instance.Faith -= lossedFaith;
        fatihTextEemitter.Emit("-" + (int)(lossedFaith * 100), Color.green, 3);
        GameManager.Instance.cultistNumber--;
    }

    private void OnGameOver()
    {
        Destroy(gameObject);
        GameManager.Instance.OnGameOver -= OnGameOver;
    }

    public Vector2 FormationOffset
    {
        get
        {
            int index = CombatSceneManager.Instance.ourCrew.IndexOf(gameObject);
            switch (index)
            {
                default:
                case 0:
                    return Vector2.zero;
                case 1:
                    return Vector2.down;
                case 2:
                    return Vector2.right;
                case 3:
                    return Vector2.right + Vector2.down;
                case 4:
                    return Vector2.up;
                case 5:
                    return Vector2.right + Vector2.up;
                case 6:
                    return Vector2.left;
                case 7:
                    return Vector2.left + Vector2.down;
                case 8:
                    return Vector2.left + Vector2.up;
            }
        }
    }
}
