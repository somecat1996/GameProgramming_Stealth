using UnityEngine;

/// <summary>
/// Manager of enemies
/// </summary>
public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    // Look ahead distance for pinky and inky
    public float lookAheadDistance = 10f;
    // Player position
    public Transform player;
    private Vector3 playerPosition;

    // Player sight info
    public bool playerDetected;
    public bool playerLost;
    public float playerLostTime = 5f;
    private float playerLostTimer;

    [SerializeField] private EnemyController[] enemies;

    // Distance for enemy to be activated by another enemy
    public float enemyActivateRange = 10f;

    // Enemy state color
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

    /// <summary>
    /// Called when an enemy detects the player
    /// </summary>
    /// <param name="enemy">The enemy that detects the player</param>
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

    /// <summary>
    /// Put a patrol enemy to blinky state
    /// </summary>
    /// <param name="enemy">Target enemy</param>
    private void TryEnterBlinkStateFromPatrol(EnemyController enemy)
    {
        Debug.Log($"{enemy.gameObject.name} try enter blinky state from patrol state");
        if (GetEnemy(EnemyState.Blinky, out EnemyController enemy1))
        {
            // If existing blinky state enemy can see the player, this enemy enter an avaliable state
            if (enemy1.canSeePlayer)
            {
                if (GetEnemy(EnemyState.Pinky, out _)) SetEnemyState(enemy1, EnemyState.Pinky);
                else SetEnemyState(enemy1, EnemyState.Inky);
                return;
            }
            // If existing blinky enemy can't see the player, the enemy closer to the player enter blinky state, the other one enters an avaliable state
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
        // If there is no existing blinky state enemy, this enemy enter blinky state
        else SetEnemyState(enemy, EnemyState.Blinky);
    }

    /// <summary>
    /// Put a pinky enemy to blinky state
    /// </summary>
    /// <param name="enemy">Target enemy</param>
    private void TryEnterBlinkStateFromPinky(EnemyController enemy)
    {
        if (GetEnemy(EnemyState.Blinky, out EnemyController enemy1))
        {
            // If existing blinky state enemy can see the player, this enemy enter an avaliable state
            if (enemy1.canSeePlayer)
                return;
            // If existing blinky enemy can't see the player, the enemy closer to the player enter blinky state, the other one enters pinky state
            float distance1 = (player.position - enemy.transform.position).magnitude;
            float distance2 = (player.position - enemy1.transform.position).magnitude;
            if (distance1 < distance2)
            {
                SetEnemyState(enemy, EnemyState.Blinky);
                SetEnemyState(enemy1, EnemyState.Pinky);
            }
        }
        // If there is no existing blinky state enemy, this enemy enter blinky state
        else SetEnemyState(enemy, EnemyState.Blinky);
    }


    /// <summary>
    /// Put a pinky enemy to inky state
    /// </summary>
    /// <param name="enemy">Target enemy</param>
    private void TryEnterBlinkStateFromInky(EnemyController enemy)
    {
        if (GetEnemy(EnemyState.Blinky, out EnemyController enemy1))
        {
            // If existing blinky state enemy can see the player, this enemy enter an avaliable state
            if (enemy1.canSeePlayer)
                return;
            // If existing blinky enemy can't see the player, the enemy closer to the player enter blinky state, the other one enters inky state
            float distance1 = (player.position - enemy.transform.position).magnitude;
            float distance2 = (player.position - enemy1.transform.position).magnitude;
            if (distance1 < distance2)
            {
                SetEnemyState(enemy, EnemyState.Blinky);
                SetEnemyState(enemy1, EnemyState.Inky);
            }
        }
        // If there is no existing blinky state enemy, this enemy enter blinky state
        else SetEnemyState(enemy, EnemyState.Blinky);
    }

    /// <summary>
    /// Put all enemies to patrol state
    /// </summary>
    public void AlertClear()
    {
        foreach (EnemyController enemy in enemies)
        {
            SetEnemyState(enemy, EnemyState.Patrol);
        }
    }

    /// <summary>
    /// Check enemies around one enemy and activate them
    /// </summary>
    /// <param name="enemy">Source enemy</param>
    public void ActivateOtherEnemy(EnemyController enemy)
    {
        foreach (EnemyController enemy1 in enemies)
        {
            if (enemy != enemy1 && enemy1.enemyState == EnemyState.Patrol && Vector3.Distance(enemy.enemy.transform.position, enemy1.enemy.transform.position) <= enemyActivateRange)
            {
                OnActivate(enemy1);
            }
        }
    }

    /// <summary>
    /// Active one enemy and put it to valid state
    /// </summary>
    /// <param name="enemy">Target enemy</param>
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

    /// <summary>
    /// Get one enemy of sertain state
    /// </summary>
    /// <param name="state">Search state</param>
    /// <param name="returnEnemy">Found enemy</param>
    /// <returns>Whether find this enemy</returns>
    public bool GetEnemy(EnemyState state, out EnemyController returnEnemy)
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

    /// <summary>
    /// Set one enemy to a state
    /// </summary>
    /// <param name="enemy">Target enemy</param>
    /// <param name="state">Target state</param>
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

    /// <summary>
    /// Update player visible state. If player isn't visible by enemies, start timer
    /// </summary>
    /// <param name="canSeePlayer"></param>
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

    /// <summary>
    /// Get player position
    /// </summary>
    /// <returns>Player position</returns>
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
