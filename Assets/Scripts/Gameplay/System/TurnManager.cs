using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    private List<Enemy> _enemies;
    private List<Enemy> _activeEnemies;
    private List<Enemy> _aggroedEnemies;
    private EnemyStateMachine _currentEnemyTarget;
    
    private Queue<ICommand> _turnQueue;
    private TurnType _currentTurn;
    private bool _isBattling;
    private bool _isCommandExecuting;
    private int _actionsThisTurn;
    private int _totalActionsRequired;
    
    [SerializeField] private PlayerTurnEventChannel playerTurnEventChannel;
    [SerializeField] private EnemyActionCompleteEventChannel enemyActionCompleteEventChannel;   
    [SerializeField] private EnemyLeftEventChannel enemyLeftEventChannel;
    [SerializeField] private FloorClearedEventChannel floorClearedEventChannel;
    
    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
        }
        else
        {
            _isBattling = false;
            _isCommandExecuting = false;
            Instance = this;
            _currentTurn = TurnType.PlayerTurn;
            _turnQueue = new Queue<ICommand>();
            _activeEnemies = new List<Enemy>();
            _aggroedEnemies = new List<Enemy>();
            
            playerTurnEventChannel.playerTurnEvent.AddListener(SwitchToEnemyTurn);
            enemyActionCompleteEventChannel.OnActionComplete.AddListener(CommandCompleted);
        }
    }

    public void AddQueue(ICommand command)
    {
        _turnQueue.Enqueue(command);
        _actionsThisTurn++;
    }

    private void CommandCompleted()
    {
        // Debug.Log("Enemy Action completed, action number: " + _actionsThisTurn);
        _isCommandExecuting = false;
        if (_actionsThisTurn >= _totalActionsRequired + 1)
        {
            SwitchToPlayerTurn();
        }
    }

    private void UpdateEnemyCount()
    {
        _enemies = new List<Enemy>(FindObjectsOfType<Enemy>());
        if (_enemies.Count == 0)
        {
            floorClearedEventChannel?.RaiseEvent();
        }
        else 
        {
            enemyLeftEventChannel?.RaiseEvent(_enemies.Count);
        }
    }
    
    void Update()
    {
        UpdateEnemyCount();
        if(_turnQueue.Count > 0)
        {
            Debug.Log(" -------------- Queue Count: " + _turnQueue.Count);
        }
        // Debug.Log(_currentTurn);
        // Debug.Log(_isCommandExecuting);
        foreach (var queueItem in _turnQueue)
        {
            Debug.Log(queueItem);
        }

        if (_isCommandExecuting)
        {
            // Debug.Log("Don't exec next");
            return;
        }
        
        ExecuteNext();
    }   

    public void SwitchToEnemyTurn()
    {
        _isCommandExecuting = false;
        
        if(PlayerStateMachine.Instance.CurrentState is not PlayerMoveState && _activeEnemies.Count > 0)
        {
            _currentTurn = TurnType.EnemyTurn;
            Debug.Log("Switching to enemy turn");
        }

        ResetLosChecker();
    }
    
    public void SwitchToPlayerTurn()
    {
        Debug.Log("Switching to player turn");
        _actionsThisTurn = 1;
        _currentTurn = TurnType.PlayerTurn;
    }
    
    private void CheckBattlingState()
    {
        _isBattling = _activeEnemies.Count > 0;
        _totalActionsRequired = _activeEnemies.Count;
        // Debug.Log("isBattling during check" + _isBattling);
        // Debug.Log("Current total actions required " + _totalActionsRequired);
        // Debug.Log("Current enemy Count: " + _activeEnemies.Count);
        
        if (!_isBattling)
        {
            _turnQueue.Clear();
            SwitchToPlayerTurn();
        }
    }

    private void ResetLosChecker()
    {
        foreach (var enemy in _activeEnemies)
        {
            if (enemy.EnemyStateMachine.CurrentState is EnemyAlertState)
            {
                ((EnemyAlertState) enemy.EnemyStateMachine.CurrentState).CommandQueued = false;
            }
        }
    }

    public void AddEnemy(Enemy enemy)
    {
        if (!_activeEnemies.Contains(enemy))
        {
            _activeEnemies.Add(enemy);
            CheckBattlingState();
        }
    }

    public void RemoveEnemy(Enemy enemy)
    {
        // Debug.Log("Removing Enemy");
        if (_activeEnemies.Remove(enemy))
        {
            // Debug.Log("Successfully Removed!");
            CheckBattlingState();
        }
    }
    
    public void AddAggroEnemy(Enemy enemy)
    {
        if (!_aggroedEnemies.Contains(enemy))
        {
            _aggroedEnemies.Add(enemy);
        }
    }

    public void RemoveAggroEnemy(Enemy enemy)
    {
        if (_aggroedEnemies.Contains(enemy))
        {
            _aggroedEnemies.Remove(enemy);
        }
    }
    
    private void ExecuteNext()
    {
        if (_turnQueue.Count > 0)
        {
            ICommand currentCommand = _turnQueue.Dequeue();
            _isCommandExecuting = true;
            currentCommand.Execute();
            
            // Debug.Log("Queue Count: " + _turnQueue.Count);
            // Debug.Log("Enemy Count " + _activeEnemies.Count);
            Debug.Log(currentCommand + " Executed! Turn: " + _actionsThisTurn);
        }
    }

    public TurnType CurrentTurn
    {
        get => _currentTurn;
        set => _currentTurn = value;
    }

    public bool IsBattling
    {
        get => _isBattling;
        set => _isBattling = value;
    }

    public List<Enemy> Enemies
    {
        get => _enemies;
        set => _enemies = value;
    }

    public List<Enemy> ActiveEnemies
    {
        get => _activeEnemies;
        set => _activeEnemies = value;
    }

    public List<Enemy> AggroedEnemies
    {
        get => _aggroedEnemies;
        set => _aggroedEnemies = value;
    }

    public EnemyStateMachine CurrentEnemyTarget
    {
        get => _currentEnemyTarget;
        set => _currentEnemyTarget = value;
    }

    public int ActionsThisTurn => _actionsThisTurn;
    public int TotalActionsRequired => _totalActionsRequired;
}
