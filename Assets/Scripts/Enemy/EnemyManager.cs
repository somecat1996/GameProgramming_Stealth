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

    public Material enemyNormal;
    public Material enemyBlinky;
    public Material enemyPinky;
    public Material enemyInky;

    private void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        playerDetected = false;
        playerLost = true;
    }

    private void Update()
    {
        if (!playerLost)
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
                TryEnterBlinkStateFromPatrol(enemy);
                break;
            case EnemyState.Blinky:
                break;
            case EnemyState.Inky:
                TryEnterBlinkStateFromInky(enemy);
                break;
            case EnemyState.Pinky:
                TryEnterBlinkStateFromPinky(enemy);
                break;
            default:
                break;
        }
    }

    private void TryEnterBlinkStateFromPatrol(EnemyController enemy)
    {
        Debug.Log($"{enemy.gameObject.name} try enter blinky state from patrol state");
        if (GetEnemy(EnemyState.Blinky, out EnemyController enemy1))
        {
            if (enemy1.canSeePlayer)
            {
                if (GetEnemy(EnemyState.Pinky, out _)) SetEnemyState(enemy1, EnemyState.Pinky);
                else SetEnemyState(enemy1, EnemyState.Inky);
                return;
            }
            float distance1 = (player.position - enemy.transform.position).magnitude;
            float distance2 = (player.position - enemy1.transform.position).magnitude;
            if (distance1 < distance2)
            {
                SetEnemyState(enemy, EnemyState.Blinky);
                if (GetEnemy(EnemyState.Pinky, out _)) SetEnemyState(enemy1, EnemyState.Pinky);
                else SetEnemyState(enemy1, EnemyState.Inky);
            }
            else
            {
                if (GetEnemy(EnemyState.Pinky, out _)) SetEnemyState(enemy, EnemyState.Pinky);
                else SetEnemyState(enemy, EnemyState.Inky);
            }
        }
        else SetEnemyState(enemy, EnemyState.Blinky);
    }

    private void TryEnterBlinkStateFromPinky(EnemyController enemy)
    {
        if (GetEnemy(EnemyState.Blinky, out EnemyController enemy1))
        {
            if (enemy1.canSeePlayer)
                return;
            float distance1 = (player.position - enemy.transform.position).magnitude;
            float distance2 = (player.position - enemy1.transform.position).magnitude;
            if (distance1 < distance2)
            {
                SetEnemyState(enemy, EnemyState.Blinky);
                SetEnemyState(enemy1, EnemyState.Pinky);
            }
        }
        else SetEnemyState(enemy, EnemyState.Blinky);
    }

    private void TryEnterBlinkStateFromInky(EnemyController enemy)
    {
        if (GetEnemy(EnemyState.Blinky, out EnemyController enemy1))
        {
            if (enemy1.canSeePlayer)
                return;
            float distance1 = (player.position - enemy.transform.position).magnitude;
            float distance2 = (player.position - enemy1.transform.position).magnitude;
            if (distance1 < distance2)
            {
                SetEnemyState(enemy, EnemyState.Blinky);
                SetEnemyState(enemy1, EnemyState.Inky);
            }
        }
        else SetEnemyState(enemy, EnemyState.Blinky);
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
        if (!GetEnemy(EnemyState.Pinky, out _))
        {
            SetEnemyState(enemy, EnemyState.Pinky);
        }
        else
        {
            SetEnemyState(enemy, EnemyState.Inky);
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
        switch (state)
        {
            case EnemyState.Patrol:
                enemy.enemyBody.material = enemyNormal;
                break;
            case EnemyState.Blinky:
                enemy.enemyBody.material = enemyBlinky;
                break;
            case EnemyState.Inky:
                enemy.enemyBody.material = enemyInky;
                break;
            case EnemyState.Pinky:
                enemy.enemyBody.material = enemyPinky;
                break;
            default:
                enemy.enemyBody.material = enemyNormal;
                break;
        }
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
