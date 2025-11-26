using System;
using System.Collections;
using UnityEngine;
using PlayerState;
using Unity.Cinemachine;
public class PlayerStateMachine : MonoBehaviour,IDataPersistence
{
    [Header("Componets")]
    [SerializeField] private Rigidbody _body;
    [SerializeField] private Animator animator;
    [SerializeField] private GroundSensor _groundSensor;
    [SerializeField] private CinemachineImpulseSource _cameraShakeSource;
    [SerializeField] private PlayerAudioManager _audioManager;
    public InputReader inputReader;
    private Camera _mainCameraRef;

    [Header("Managers")]
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private PlayerCombatManager _combat;
    [SerializeField] private PlayerHealthManager _health;
    [SerializeField] private Shield _shield;
    public PlayerData currentData;


    [Header("State variables")]
    State _currentState;
    State _oldState;
    PlayerStateFactory _states;
    private bool _gameIsPaused;
    

    #region Getters/Setters

    public State CurrentState { get { return _currentState; } set { _currentState = value; } }
    public bool GameIsPaused { get { return _gameIsPaused; } }
    public State OldState { get { return _oldState; } set { _oldState = value; } }
    public bool IsBlocking;   

    //Managers
    public PlayerMovement Movement { get { return _movement; } }
    public PlayerCombatManager Combat { get { return _combat; } }
    public PlayerHealthManager Health { get { return _health; } }
    public Shield Shield { get { return _shield; } }

    //COMPONENTS
    public Animator PlayerAnimator { get { return animator; } }
    public Rigidbody Body { get { return _body; }}
    public GroundSensor GroundSensor { get { return _groundSensor; } }
    public Camera MainCameraRef {  get { return _mainCameraRef; } }
    public CinemachineImpulseSource CameraShakeSource { get { return _cameraShakeSource; } }
    public PlayerAudioManager AudioManager { get { return _audioManager; } }


    #endregion

    private void Start()
    {
        //Handle States Start
        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.Enter();
        DataPersistenceManager.Instance.LoadGame();
        //Get Components
        _mainCameraRef = Camera.main;
        PauseGameManager.ResumeGame();
    }


    private void OnEnable()
    {
        GameEvents.PauseGame += OnPauseGame;
        GameEvents.ResumeGame += OnResumeGame;
    }

    private void OnDisable()
    {
        GameEvents.PauseGame -= OnPauseGame;
        GameEvents.ResumeGame -= OnResumeGame;
        DataPersistenceManager.Instance.SaveGame();
    }
    private void Update()
    {
        if (_gameIsPaused) return;
        _currentState.UpdateStates();
    }
    private void FixedUpdate()
    {
        _currentState.FixedUpdateStates();
    }

    public void LoadData(PlayerData data)
    {
        currentData.hasSword = data.hasSword;
        currentData.hasShield = data.hasShield; 
        currentData.isFirstPlaythrough = data.isFirstPlaythrough;
    }

    public void SaveData(PlayerData data)
    {
       data.hasSword = currentData.hasSword;  
       data.hasShield = currentData.hasShield;
        data.isFirstPlaythrough= currentData.isFirstPlaythrough;
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
[Serializable]
public class PlayerData
{
    public float health;
    public bool hasSword, hasShield;
    public bool hasCompletedLevel1 = false;
    public bool hasCompletedLevel2 = false;
    public Vector3 spawnPos;
    public string lastTotemId;
    public string lastSceneName;
    public bool isFirstPlaythrough;
    public PlayerData()
    {
        health = 100;
        hasSword = false;
        hasShield = false;
        spawnPos = Vector3.zero;  
        lastTotemId = null;
        lastSceneName = null;
        isFirstPlaythrough = true;

    }  
}
