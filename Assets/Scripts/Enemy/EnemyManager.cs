using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public float lookAheadDistance = 10f;
    public Transform player;
    private Vector3 playerPosition;

    private bool playerDetected;
    public float playerLostTime = 5f;
    private float playerLostTimer;

    [SerializeField] private EnemyController[] enemies;

    private void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        playerDetected = false;
    }

    private void Update()
    {
        if (!playerDetected)
        {
            playerLostTimer += Time.deltaTime;
            if (playerLostTimer >= playerLostTime)
            {
                playerDetected = false;
                playerPosition = player.position;
            }
        }
    }

    public void OnDetectPlayer(EnemyController enemy)
    {
        switch (enemy.enemyState)
        {
            case EnemyState.Patrol:
                SetEnemyState(enemy, EnemyState.Blinky);
                break;
            case EnemyState.Blinky:
                UpdatePlayerDetection(true);
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

    public void OnActivate(EnemyController enemy)
    {
        EnemyController enemy1;
        if (!GetEnemy(EnemyState.Pinky, out enemy1))
        {

        }
        else
        {

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

    private void UpdatePlayerDetection(bool canSeePlayer)
    {
        playerDetected = canSeePlayer;
        if (!playerDetected)
        {
            playerLostTimer = 0;
        }
    }

    public Vector3 GetPlayerPosition()
    {
        if (playerDetected) return player.position;
        else return playerPosition;
    }
}
