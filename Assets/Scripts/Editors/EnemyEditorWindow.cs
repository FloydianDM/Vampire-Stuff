using UnityEngine;
using UnityEditor;

public class EnemyEditorWindow : EditorWindow
{
    // editor for creating enemy details SOs
    private string _enemyType;
    private string _assetPath = "Assets/ScriptableObjects/Enemies/";
    private EnemyDetailsSO _enemyDetailsInstance;
    private EnemyDetailsSO _selectedEnemyDetailsInstance;
    private SerializedObject _serializedObject;
    private SerializedProperty _typeProp;
    private SerializedProperty _enemyPrefabProp;
    private SerializedProperty _speedMinProp;
    private SerializedProperty _speedMaxProp;
    private SerializedProperty _dodgeThrustProp;
    private SerializedProperty _healthMinProp;
    private SerializedProperty _healthMaxProp;
    private SerializedProperty _damageProp;
    private SerializedProperty _experienceDropProp;
    private SerializedProperty _awarenessProp;
    private SerializedProperty _agilityProp;
    private SerializedProperty _enemyDeathEffect;

    [MenuItem("Tools/Enemy Creator")]
    private static void ShowWindow()
    {
        var window = GetWindow<EnemyEditorWindow>();
        window.titleContent = new GUIContent("Enemy Creator");
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Create Enemy Scriptable Object", EditorStyles.boldLabel);

        _enemyType = EditorGUILayout.TextField("Type", _enemyType);

        if (GUILayout.Button("Create Enemy Details Scriptable Object"))
        {
            CreateScriptableObject();
        }

        if (_enemyDetailsInstance != null)
        {
            GUILayout.Label("New Fields", EditorStyles.boldLabel);

            _enemyDetailsInstance.Type = _enemyType;
            _enemyDetailsInstance.EnemyPrefab = (GameObject)EditorGUILayout.ObjectField(
                "EnemyPrefab", _enemyDetailsInstance.EnemyPrefab, typeof(GameObject), false);
            _enemyDetailsInstance.SpeedMin = EditorGUILayout.FloatField("SpeedMin", _enemyDetailsInstance.SpeedMin);
            _enemyDetailsInstance.SpeedMax = EditorGUILayout.FloatField("SpeedMax", _enemyDetailsInstance.SpeedMax);
            _enemyDetailsInstance.DodgeThrust = EditorGUILayout.FloatField("DodgeThrust", _enemyDetailsInstance.DodgeThrust);
            _enemyDetailsInstance.HealthMin = EditorGUILayout.IntField("HealthMin", _enemyDetailsInstance.HealthMin);
            _enemyDetailsInstance.HealthMax = EditorGUILayout.IntField("HealthMax", _enemyDetailsInstance.HealthMax);
            _enemyDetailsInstance.Damage = EditorGUILayout.FloatField("Damage", _enemyDetailsInstance.Damage);
            _enemyDetailsInstance.ExperienceDrop = EditorGUILayout.IntSlider("ExperienceDrop", _enemyDetailsInstance.ExperienceDrop, 1, 20);
            _enemyDetailsInstance.Awareness = EditorGUILayout.Slider("Awareness", _enemyDetailsInstance.Awareness, 0, 2);
            _enemyDetailsInstance.Agility = EditorGUILayout.Slider("Agility", _enemyDetailsInstance.Agility, 0, 20);
            _enemyDetailsInstance.EnemyDeathEffect = (EnemyDeathEffectSO)EditorGUILayout.ObjectField(
                "EnemyDeathEffect", _enemyDetailsInstance.EnemyDeathEffect, typeof(EnemyDeathEffectSO), false);

            if (GUILayout.Button("Save Enemy Details Scriptable Object"))
            {
                SaveScriptableObject();
            }
        }

        GUILayout.Space(10);
        GUILayout.Label("Edit Enemy Details Scriptable Object", EditorStyles.boldLabel);

        _selectedEnemyDetailsInstance = (EnemyDetailsSO)EditorGUILayout.ObjectField(
            "Select Scriptable Object", _selectedEnemyDetailsInstance, typeof(EnemyDetailsSO), false);

        if (_selectedEnemyDetailsInstance != null)
        {
            if (_serializedObject == null || _serializedObject.targetObject != _selectedEnemyDetailsInstance)
            {
                _enemyDetailsInstance = null; // to clear the create section

                _serializedObject = new SerializedObject(_selectedEnemyDetailsInstance);

                _typeProp = _serializedObject.FindProperty("Type");
                _enemyPrefabProp = _serializedObject.FindProperty("EnemyPrefab");
                _speedMinProp = _serializedObject.FindProperty("SpeedMin");
                _speedMaxProp = _serializedObject.FindProperty("SpeedMax");
                _dodgeThrustProp = _serializedObject.FindProperty("DodgeThrust");
                _healthMinProp = _serializedObject.FindProperty("HealthMin");
                _healthMaxProp = _serializedObject.FindProperty("HealthMax");
                _damageProp = _serializedObject.FindProperty("Damage");
                _experienceDropProp = _serializedObject.FindProperty("ExperienceDrop");
                _awarenessProp = _serializedObject.FindProperty("Awareness");
                _agilityProp = _serializedObject.FindProperty("Agility");
                _enemyDeathEffect = _serializedObject.FindProperty("EnemyDeathEffect");
            }

            _serializedObject.Update();

            EditorGUILayout.PropertyField(_typeProp);
            EditorGUILayout.PropertyField(_enemyPrefabProp);
            EditorGUILayout.PropertyField(_speedMinProp);
            EditorGUILayout.PropertyField(_speedMaxProp);
            EditorGUILayout.PropertyField(_dodgeThrustProp);
            EditorGUILayout.PropertyField(_healthMinProp);
            EditorGUILayout.PropertyField(_healthMaxProp);
            EditorGUILayout.PropertyField (_damageProp);
            EditorGUILayout.PropertyField(_experienceDropProp);
            EditorGUILayout.PropertyField(_awarenessProp);
            EditorGUILayout.PropertyField(_agilityProp);
            EditorGUILayout.PropertyField(_enemyDeathEffect);

            _serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Save Enemy Details Scriptable Object"))
            {
                SaveEditedScriptableObject();
            }
        }
    }

    private void CreateScriptableObject()
    {
        _enemyDetailsInstance = CreateInstance<EnemyDetailsSO>();
    }

    private void SaveScriptableObject()
    {
        if (_enemyDetailsInstance != null)
        {
            string path = AssetDatabase.GenerateUniqueAssetPath(_assetPath + "EnemyDetails_" + _enemyType + ".asset");
            AssetDatabase.CreateAsset(_enemyDetailsInstance, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = _enemyDetailsInstance;
        }
    }

    private void SaveEditedScriptableObject()
    {
        EditorUtility.SetDirty(_selectedEnemyDetailsInstance);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = _selectedEnemyDetailsInstance;
    }

}
