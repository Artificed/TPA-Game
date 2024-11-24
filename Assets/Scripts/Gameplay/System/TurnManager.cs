using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        if (CurrentTurn == TurnType.EnemyTurn)
        {
            Debug.Log("One Enemy Turn Completed!");
        }
        _isCommandExecuting = false;
        _actionsThisTurn++;
        
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
        
        GetBattlingEnemies();
        if (_enemies.Count == 0 || _isCommandExecuting) return;
        
        ExecuteNext();
    }   

    public void SwitchToEnemyTurn()
    {
        if (!_isBattling) return;
        _isCommandExecuting = false;
        _currentTurn = TurnType.EnemyTurn;
        Debug.Log("Switching to enemy turn");
    }

    public void SwitchToPlayerTurn()
    {
        if (!_isBattling) return;
        _actionsThisTurn = 0;
        _currentTurn = TurnType.PlayerTurn;
    }

    private void GetBattlingEnemies()
    {
        _enemies = new List<EnemyStateMachine>(FindObjectsOfType<EnemyStateMachine>().Where(enemy => enemy.CurrentState is not EnemyIdleState));
        if (_enemies.Count > 0)
        {   
            _isBattling = true;
            _totalActionsRequired = _enemies.Count;
        }
        else
        {
            _isBattling = false;
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

    public int ActionsThisTurn
    {
        get => _actionsThisTurn;
        set => _actionsThisTurn = value;
    }
}
