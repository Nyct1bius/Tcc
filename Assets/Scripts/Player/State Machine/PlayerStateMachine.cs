using System;
using System.Collections;
using UnityEngine;
using PlayerState;
public class PlayerStateMachine : MonoBehaviour
{
    [Header("Componets")]
    [SerializeField] Rigidbody _body;
    [SerializeField] Animator animator;
    [SerializeField] GroundSensor _groundSensor;
    public InputReader inputReader;
    private Camera _mainCameraRef;

    [Header("Managers")]
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private PlayerCombatManager _combat;
    [SerializeField] private PlayerHealthManager _health;



    [Header("State variables")]
    State _currentState;
    State _oldState;
    PlayerStateFactory _states;
    private bool _gameIsPaused;
    

    #region Getters/Setters

    public State CurrentState { get { return _currentState; } set { _currentState = value; } }
    public bool GameIsPaused { get { return _gameIsPaused; } }
    public State OldState { get { return _oldState; } set { _oldState = value; } }
   
    //Managers
    public PlayerMovement Movement { get { return _movement; } }
    public PlayerCombatManager Combat { get { return _combat; } }
    public PlayerHealthManager Health { get { return _health; } }


    //COMPONENTS
    public Animator Animator { get { return animator; } }
    public Rigidbody Body { get { return _body; }}
    public GroundSensor GroundSensor { get { return _groundSensor; } }
    public Camera MainCameraRef {  get { return _mainCameraRef; } }
    #endregion

    private void Start()
    {
        //Handle States Start
        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.Enter();

        //Get Components
        _mainCameraRef = Camera.main;

    }


    private void OnEnable()
    {
        GameEvents.PauseGame += OnPauseGame;
        GameEvents.ResumeGame += OnResumeGame;
    }

    private void OnDisable()
    {
        GameEvents.PauseGame -= OnPauseGame;
        GameEvents.ResumeGame -= OnResumeGame;;
    }
    private void Update()
    {
        _currentState.UpdateStates();
    }
    private void FixedUpdate()
    {
        _currentState.FixedUpdateStates();
    }

    #region Game States
    private void OnPauseGame()
    {
        _gameIsPaused = true;
    }
    private void OnResumeGame()
    {
        _gameIsPaused = false;
    }

    #endregion




}
