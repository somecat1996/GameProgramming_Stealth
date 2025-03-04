using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public float lookAheadDistance = 10f;
    public Transform player;
    private Vector3 playerPosition;

    public bool playerDetected;
    public bool playerLost;
    public float playerLostTime = 5f;
    private float playerLostTimer;

    [SerializeField] private EnemyController[] enemies;

    public float enemyActivateRange = 10f;

    private void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        playerDetected = false;
        playerLost = true;
    }

    private void Update()
    {
        if (playerLost)
        {

        }
        else
        {
            playerPosition = player.position;
            if (!playerDetected)
            {
                playerLostTimer += Time.deltaTime;
                if (playerLostTimer >= playerLostTime) playerLost = true;
            }
        }
    }

    public void OnDetectPlayer(EnemyController enemy)
    {
        playerLost = false;
        UpdatePlayerDetection(true);
        switch (enemy.enemyState)
        {
            case EnemyState.Patrol:
                SetEnemyState(enemy, EnemyState.Blinky);
                break;
            case EnemyState.Blinky:
                break;
            case EnemyState.Inky:
                if (GetEnemy(EnemyState.Blinky, out EnemyController enemy1))
                {
                    float distance1 = (player.position - enemy.transform.position).magnitude;
                    float distance2 = (player.position - enemy1.transform.position).magnitude;
                    if (distance1 < distance2)
                    {

                    }
                    else
                    {

                    }
                }
                else SetEnemyState(enemy, EnemyState.Blinky);
                break;
            case EnemyState.Pinky:
                if (GetEnemy(EnemyState.Blinky, out EnemyController enemy2))
                {

                }
                else SetEnemyState(enemy, EnemyState.Blinky);
                break;
            default:
                SetEnemyState(enemy, EnemyState.Blinky);
                break;
        }
    }

    public void AlertClear()
    {
        foreach (EnemyController enemy in enemies)
        {
            SetEnemyState(enemy, EnemyState.Patrol);
        }
    }

    public void ActivateOtherEnemy(EnemyController enemy)
    {
        foreach (EnemyController enemy1 in enemies)
        {
            if (enemy != enemy1 && Vector3.Distance(enemy.enemy.transform.position, enemy1.enemy.transform.position) <= enemyActivateRange)
            {
                OnActivate(enemy1);
            }
        }
    }

    public void OnActivate(EnemyController enemy)
    {
        EnemyController enemy1;
        if (!GetEnemy(EnemyState.Pinky, out enemy1))
        {
            SetEnemyState(enemy, EnemyState.Pinky);
        }
        else
        {
            if (!GetEnemy(EnemyState.Inky, out enemy1))
            {
                SetEnemyState(enemy, EnemyState.Inky);
            }
        }
    }

    private bool GetEnemy(EnemyState state, out EnemyController returnEnemy)
    {
        returnEnemy = null;
        bool returnBool = false;
        foreach (EnemyController enemy in enemies)
        {
            if (enemy.enemyState == state)
            {
                returnBool = true;
                returnEnemy = enemy;
            }
        }
        return returnBool;
    }

    private void SetEnemyState(EnemyController enemy, EnemyState state)
    {
        enemy.SetState(state);
    }

    public void UpdatePlayerDetection(bool canSeePlayer)
    {
        if (playerDetected ^ canSeePlayer)
        {
            playerDetected = canSeePlayer;
            if (!playerDetected)
            {
                playerLostTimer = 0;
            }
        }
    }

    public Vector3 GetPlayerPosition()
    {
        if (playerDetected) return player.position;
        else return playerPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(GetPlayerPosition(), 0.5f);
    }
}
