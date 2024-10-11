using UnityEngine;

public static class Settings
{
    #region OBJECT TAGS
    
    public const string ENEMY_TAG = "Enemy";
    public const string SPAWNED_ENEMY_TAG = "SpawnedEnemy";
    public const string PLAYER_TAG = "Player";
    
    #endregion

    #region SCENE TAGS

    public const string GAME_SCENE_TAG = "GameScene";
    public const string MAIN_MENU_SCENE_TAG = "MainMenuScene";
    public const string SETTINGS_SCENE_TAG = "SettingsScene";
    public const string HOW_TO_PLAY_SCENE_TAG = "HowToPlayScene";

    #endregion
    
    #region GAME PARAMETERS

    public const float RESET_CONTACT_COLLISION_TIME = 1f;
    
    #endregion

    #region ANIMATORS PARAMETERS

    public static int IsMoving = Animator.StringToHash("isMoving");
    public static int IsIdle = Animator.StringToHash("isIdle");
    public static int IsDying = Animator.StringToHash("isDying");
    public static int IsDodging = Animator.StringToHash("isDodging");
    public static int IsUsing = Animator.StringToHash("isUsing");

    #endregion

    #region OBJECT KEYS

    public const string HIGH_SCORE_KEY = "HighScore";

    #endregion

    #region OBJECT TEXTS

    public const string CHEST_OPEN_TEXT = "Press 'E' To Open";
    public const string CHEST_PICKUP_TEXT = "Press 'E' To Pickup";

    #endregion
}
