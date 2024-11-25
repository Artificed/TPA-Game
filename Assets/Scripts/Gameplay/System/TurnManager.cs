using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    private List<EnemyStateMachine> _enemies;
    private Queue<ICommand> _turnQueue;
    private TurnType _currentTurn;
    private bool _isBattling;
    private bool _isCommandExecuting;
    private int _actionsThisTurn;
    private int _totalActionsRequired;
    
    [SerializeField] private PlayerTurnEventChannel playerTurnEventChannel;
    [SerializeField] private EnemyActionCompleteEventChannel enemyActionCompleteEventChannel;   
    
    private void Awake()
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
            _enemies = new List<EnemyStateMachine>();
            
            playerTurnEventChannel.playerTurnEvent.AddListener(SwitchToEnemyTurn);
            enemyActionCompleteEventChannel.OnActionComplete.AddListener(CommandCompleted);
        }
    }

    public void AddQueue(ICommand command)
    {
        _turnQueue.Enqueue(command);
    }

    private void CommandCompleted()
    {
        if(CurrentTurn == TurnType.EnemyTurn)
        {
            Debug.Log("One Enemy Turn Completed!");
            _actionsThisTurn++;
        }
        _isCommandExecuting = false;
        if (_actionsThisTurn >= _totalActionsRequired)
        {
            Debug.Log("All Enemy turns completed, Switching Back To Player Turn");
            SwitchToPlayerTurn();
        }
    }
    
    void Update()
    {
        // Debug.Log(_currentTurn);
        // Debug.Log(_isCommandExecuting);
        foreach (var queueItem in _turnQueue)
        {
            Debug.Log(queueItem);
        }

        if (_enemies.Count == 0 || _isCommandExecuting)
        {
            // Debug.Log("Don't exec next");
            return;
        }
        
        ExecuteNext();
    }   

    public void SwitchToEnemyTurn()
    {
        _isCommandExecuting = false;
        _currentTurn = TurnType.EnemyTurn;
        Debug.Log("Switching to enemy turn");

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
        _isBattling = _enemies.Count > 0;
        _totalActionsRequired = _enemies.Count;
        if (!_isBattling)
        {
            _turnQueue.Clear();
        }
    }

    private void ResetLosChecker()
    {
        foreach (var enemy in _enemies)
        {
            if (enemy.CurrentState is EnemyAlertState)
            {
                ((EnemyAlertState) enemy.CurrentState).CommandQueued = false;
            }
        }
    }

    public void AddEnemy(EnemyStateMachine enemy)
    {
        if (!_enemies.Contains(enemy))
        {
            _enemies.Add(enemy);
            CheckBattlingState();
        }
    }

    public void RemoveEnemy(EnemyStateMachine enemy)
    {
        if (_enemies.Remove(enemy))
        {
            CheckBattlingState();
        }
    }
    
    private void ExecuteNext()
    {
        if (_turnQueue.Count > 0)
        {
            ICommand currentCommand = _turnQueue.Dequeue();
            _isCommandExecuting = true;
            currentCommand.Execute();

            Debug.Log("Queue Count: " + _turnQueue.Count);
            Debug.Log("Enemy Count " + _enemies.Count);
            Debug.Log(currentCommand.ToString() + " Executed! Turn: " + _actionsThisTurn);
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

    public List<EnemyStateMachine> Enemies
    {
        get => _enemies;
        set => _enemies = value;
    }
}
