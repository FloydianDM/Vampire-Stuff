using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class GameManager : SingletonMonobehaviour<GameManager>
{
    private PlayerDetailsSO _playerDetails;
    private CinemachineVirtualCamera _cinemachineVirtualCamera;
    public Player Player {get; private set;}
    public GameState GameState;

    private GameResources _gameResources => GameResources.Instance;

    protected override void Awake()
    {
        base.Awake();

        _cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        _playerDetails = _gameResources.CurrentPlayerDetails;

        InstantiatePlayer();
        GameState = GameState.Play;
        StaticEventHandler.CallGameStateChangedEvent(GameState);
    }

    private void OnEnable()
    {
        Player.LevelUpEvent.OnLevelUpEvent += LevelUpEvent_OnLevelUpEvent;
        StaticEventHandler.OnGameStateChanged += StaticEventHandler_OnGameStateChanged;
    }

    private void OnDisable()
    {
        Player.LevelUpEvent.OnLevelUpEvent -= LevelUpEvent_OnLevelUpEvent;
        StaticEventHandler.OnGameStateChanged -= StaticEventHandler_OnGameStateChanged;
    }
    
    private void LevelUpEvent_OnLevelUpEvent(LevelUpEvent @event, LevelUpEventArgs args)
    {
        GameState = GameState.LevelUp;
        StaticEventHandler.CallGameStateChangedEvent(GameState);
    }
    
    private void StaticEventHandler_OnGameStateChanged(GameStateChangedEventArgs args)
    {
        GameState = args.GameState;
    }

    private void InstantiatePlayer()
    {
        GameObject playerGameObject = Instantiate(_playerDetails.Prefab, transform.position, Quaternion.identity);

        Player = playerGameObject.GetComponent<Player>();

        Player.InitialisePlayer(_playerDetails);

        _cinemachineVirtualCamera.Follow = Player.transform;
    }

    public void OpenSettingsMenu()
    {
        SceneManager.LoadScene(Settings.SETTINGS_SCENE_TAG, LoadSceneMode.Additive);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(Settings.GAME_SCENE_TAG);
    }
}