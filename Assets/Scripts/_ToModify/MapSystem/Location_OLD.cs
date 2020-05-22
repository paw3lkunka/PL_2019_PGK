using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class Location_OLD : MonoBehaviour
{
    #region Variables

    public int generationID;

    public string sceneName;
    public Vector2 returnPoint;

    public float RespawnCooldown;
    [field: SerializeField]
    public float cooldownTimer { get; private set; }

    public Vector2 GlobalReturnPoint => (Vector2)transform.position + returnPoint;

    private Slider cooldownVisualizer;
    #endregion

    #region MonoBehaviour

    private void Start()
    {
        cooldownVisualizer = GetComponentInChildren<Slider>();
        cooldownTimer = float.NegativeInfinity;
        if (cooldownVisualizer)
        {
            cooldownVisualizer.value = 0.0f;
        }
    }

    private void Update()
    {
        if (MapSceneManager.Instance.playerPositionController.Moved)
        {
            cooldownTimer -= Time.deltaTime;
            if(cooldownVisualizer)
            {
                cooldownVisualizer.value = Mathf.Clamp01(cooldownTimer/RespawnCooldown);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //GameplayManager.Instance.lastWorldMapPosition = GlobalReturnPoint;
        //GameplayManager.Instance.currentLocation = this;
        //SceneManager.LoadScene(sceneName);
        //GameplayManager.Instance.OnLocationEnterInvoke();
        //if(cooldownTimer < 0)
        //{
        //    GameplayManager.Instance.destroyedDynamicObjects.Remove(this);
        //}
        //ResetCooldownTimer();
    }


    private void OnValidate()
    {
        transform.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(GlobalReturnPoint, .2f);
    }

    #endregion

    #region Component

    public void ResetCooldownTimer() => cooldownTimer = RespawnCooldown;

    #endregion
}
