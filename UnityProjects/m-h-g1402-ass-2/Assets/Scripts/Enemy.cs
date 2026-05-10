using System.Collections;
using Enums;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
///     Controls enemy patrol behavior using a simple state machine.
///     The enemy moves between random patrol points, pauses when it arrives,
///     then selects a new destination.
/// </summary>
public class Enemy : MonoBehaviour
{
    #region variables

    

    /// <summary>
    ///     World-space points the enemy can patrol between.
    /// </summary>
    [SerializeField] private Transform[] patrolPoints;

    /// <summary>
    ///     NavMesh agent used to move the enemy.
    /// </summary>
    [SerializeField] private NavMeshAgent agent;
    
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float chaseDistance;
    [SerializeField] private float giveUpDistance;
    [SerializeField] private float chaseCheckAngle;
    [SerializeField] private Animator enemyAnim;

    /// <summary>
    ///     Current high-level behavior state (e.g., idle or patrolling).
    /// </summary>
    private EnemyState _currentState;

    /// <summary>
    ///     The currently selected patrol destination.
    /// </summary>
    private Transform _currentTarget;
    
    private bool _isWaiting = false;
    #endregion

    /// <summary>
    ///     Initializes patrol by selecting and moving to a random point.
    /// </summary>
    private void Start()
    {
        ChooseARandomPointAndMove();
    }

    /// <summary>
    ///     - If idle, starts a delay before selecting a new patrol point.
    ///     - If patrolling, switches to idle once close to destination.
    /// </summary>
    private void FixedUpdate()
    {
        if (_currentState == EnemyState.IDLE)
        {
            enemyAnim.SetBool("idle", true);
            Debug.Log("Current State: " + _currentState);
            if (!_isWaiting)
            {
                StartCoroutine(WaitAndChooseARandomPointAndMove(5));
                
                //check for the player to chase
                if (IsPlayerInRange() && IsInFOV())
                {
                    _currentState = EnemyState.CHASE;
                    enemyAnim.SetBool("idle", false);
                    Debug.Log("Current State: " + _currentState);
                }
            }
        }
        else if (_currentState == EnemyState.PATROL)
        {
            enemyAnim.SetBool("walk", true);
            Debug.Log("Current State: " + _currentState);

            if (agent.remainingDistance <= 0.2f)
            {
                _currentState = EnemyState.IDLE;
                enemyAnim.SetBool("walk", false);
                Debug.Log("Current State: " + _currentState);
            }
            
                //check for the player to chase
            if (IsPlayerInRange() && IsInFOV())
            {
                _currentState = EnemyState.CHASE;
                enemyAnim.SetBool("walk", true);
                Debug.Log("Current State: " + _currentState);
            }
            
            
            else if (_currentState == EnemyState.CHASE)
            {
                agent.SetDestination(playerTransform.position);
                enemyAnim.SetBool("walk", false);
                
                //give up
                if (HasPlayerGoneAwayFromMeTooSad())
                {
                    _currentState = EnemyState.IDLE;
                    enemyAnim.SetBool("walk", false);
                    Debug.Log("Current State: " + _currentState);
                }
            }
        }
    }

    /// <summary>
    ///     Waits for the specified duration, then resumes patrol
    ///     by choosing a new random point.
    /// </summary>
    /// <param name="timeToWait">Time in seconds to wait before moving again.</param>
    /// <returns>Coroutine enumerator used by Unity.</returns>
    private IEnumerator WaitAndChooseARandomPointAndMove(float timeToWait)
    {
        _isWaiting = true;
        yield return new WaitForSeconds(timeToWait);
        _currentState = EnemyState.PATROL;
        enemyAnim.SetBool("idle", false);
        Debug.Log("Current State: " + _currentState);
        ChooseARandomPointAndMove();
        _isWaiting = false;
    }

    /// <summary>
    ///     Picks a random patrol point and sets it as the NavMesh destination.
    ///     If no patrol points are assigned, the method exits safely.
    /// </summary>
    private void ChooseARandomPointAndMove()
    {
        if (patrolPoints.Length <= 0) return;

        _currentTarget = patrolPoints[Random.Range(0, patrolPoints.Length)];
        agent.SetDestination(_currentTarget.position);
        Debug.Log("Random point chosen");
    }

    private bool IsPlayerInRange()
    {
        return Vector3.Distance(transform.position, playerTransform.position) <= chaseDistance;
    }

    private bool HasPlayerGoneAwayFromMeTooSad()
    {
        return Vector3.Distance(transform.position, playerTransform.position) >= giveUpDistance;
    }
    
    
    private Vector3 _directionToPlayer;
    
    private bool IsInFOV()
    {
        _directionToPlayer = (playerTransform.position - transform.position).normalized;
        return Vector3.Angle(transform.forward, _directionToPlayer) <= chaseCheckAngle;
    }
}