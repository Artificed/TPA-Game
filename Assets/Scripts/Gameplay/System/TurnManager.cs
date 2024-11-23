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
            Instance = this;
            _currentTurn = TurnType.PlayerTurn;
            _turnQueue = new Queue<ICommand>();
            
            playerTurnEventChannel.playerTurnEvent.AddListener(SwitchToEnemyTurn);
        }
    }

    public void AddQueue(ICommand command)
    {
        _turnQueue.Enqueue(command);
    }

    void Update()
    {
        GetBattlingEnemies();
        if (_enemies.Count == 0) return;
        
        ExecuteNext();
    }

    public void SwitchToEnemyTurn()
    {
        if (!_isBattling) return;
        _currentTurn = TurnType.EnemyTurn;
    }

    public void SwitchToPlayerTurn()
    {
        if (!_isBattling) return;
        _actionsThisTurn = 0;
        _currentTurn = TurnType.PlayerTurn;
    }

    private void GetBattlingEnemies()
    {
        _enemies = new List<EnemyStateMachine>(FindObjectsOfType<EnemyStateMachine>().Where(enemy => !(enemy.CurrentState is EnemyIdleState or EnemyAlertState)));
        if (_enemies.Count > 0)
        {   
            _isBattling = true;
            _totalActionsRequired = _enemies.Count + 1;
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
            currentCommand.Execute();
            
            Debug.Log("Enemy Count " + _enemies.Count);
            Debug.Log(currentCommand.ToString() + " Executed! Turn: " + _actionsThisTurn);
            _actionsThisTurn++;
        
            if (_actionsThisTurn >= _totalActionsRequired + 1) // 1 for player
            {
                SwitchToPlayerTurn();
            }
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
