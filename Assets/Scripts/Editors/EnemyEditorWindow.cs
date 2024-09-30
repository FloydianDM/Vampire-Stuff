using UnityEngine;
using UnityEditor;

// editor for creating enemy details SOs
public class EnemyEditorWindow : EditorWindow
{
    private string _enemyType;
    private string _assetPath = "Assets/ScriptableObjects/Enemies/";
    private EnemyDetailsSO _newEnemyDetails;
    private EnemyDetailsSO _selectedEnemyDetails;
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
        GUILayout.Space(20);

        // create enemy section

        GUILayout.Label("Create Enemy Details Scriptable Object", EditorStyles.boldLabel);

        _enemyType = EditorGUILayout.TextField("Type", _enemyType);

        if (GUILayout.Button("Create Enemy Details Scriptable Object"))
        {
            CreateScriptableObject();
        }

        if (_newEnemyDetails != null)
        {
            GUILayout.Label("New Fields", EditorStyles.boldLabel);

            InitializeScriptableObject();

            if (GUILayout.Button("Save Enemy Details Scriptable Object"))
            {
                SaveScriptableObject();
            }
        }

        GUILayout.Space(20);

        // edit enemy section

        GUILayout.Label("Edit Enemy Details Scriptable Object", EditorStyles.boldLabel);

        _selectedEnemyDetails = (EnemyDetailsSO)EditorGUILayout.ObjectField(
            "Select Scriptable Object", _selectedEnemyDetails, typeof(EnemyDetailsSO), false);

        if (_selectedEnemyDetails != null)
        {
            EditScriptableObject();

            if (GUILayout.Button("Save Enemy Details Scriptable Object"))
            {
                SaveEditedScriptableObject();
            }
        }
    }

    private void CreateScriptableObject()
    {
        _newEnemyDetails = CreateInstance<EnemyDetailsSO>();
    }

    private void InitializeScriptableObject()
    {
        _newEnemyDetails.Type = _enemyType;
        _newEnemyDetails.EnemyPrefab = (GameObject)EditorGUILayout.ObjectField(
            "EnemyPrefab", _newEnemyDetails.EnemyPrefab, typeof(GameObject), false);
        _newEnemyDetails.SpeedMin = EditorGUILayout.FloatField("SpeedMin", _newEnemyDetails.SpeedMin);
        _newEnemyDetails.SpeedMax = EditorGUILayout.FloatField("SpeedMax", _newEnemyDetails.SpeedMax);
        _newEnemyDetails.DodgeThrust = EditorGUILayout.FloatField("DodgeThrust", _newEnemyDetails.DodgeThrust);
        _newEnemyDetails.HealthMin = EditorGUILayout.IntField("HealthMin", _newEnemyDetails.HealthMin);
        _newEnemyDetails.HealthMax = EditorGUILayout.IntField("HealthMax", _newEnemyDetails.HealthMax);
        _newEnemyDetails.Damage = EditorGUILayout.FloatField("Damage", _newEnemyDetails.Damage);
        _newEnemyDetails.ExperienceDrop = EditorGUILayout.IntSlider("ExperienceDrop", _newEnemyDetails.ExperienceDrop, 1, 20);
        _newEnemyDetails.Awareness = EditorGUILayout.Slider("Awareness", _newEnemyDetails.Awareness, 0, 2);
        _newEnemyDetails.Agility = EditorGUILayout.Slider("Agility", _newEnemyDetails.Agility, 0, 20);
        _newEnemyDetails.EnemyDeathEffect = (EnemyDeathEffectSO)EditorGUILayout.ObjectField(
            "EnemyDeathEffect", _newEnemyDetails.EnemyDeathEffect, typeof(EnemyDeathEffectSO), false);
    }

    private void EditScriptableObject()
    {
        if (_serializedObject == null || _serializedObject.targetObject != _selectedEnemyDetails)
        {
            _newEnemyDetails = null; // to clear the create section

            _serializedObject = new SerializedObject(_selectedEnemyDetails);

            // get values from SO

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
        EditorGUILayout.PropertyField(_damageProp);
        EditorGUILayout.PropertyField(_experienceDropProp);
        EditorGUILayout.PropertyField(_awarenessProp);
        EditorGUILayout.PropertyField(_agilityProp);
        EditorGUILayout.PropertyField(_enemyDeathEffect);

        _serializedObject.ApplyModifiedProperties();
    }

    private void SaveScriptableObject()
    {
        if (_newEnemyDetails != null)
        {
            string path = AssetDatabase.GenerateUniqueAssetPath(_assetPath + "EnemyDetails_" + _enemyType + ".asset");
            AssetDatabase.CreateAsset(_newEnemyDetails, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = _newEnemyDetails;
        }
    }

    private void SaveEditedScriptableObject()
    {
        if (_selectedEnemyDetails != null)
        {
            EditorUtility.SetDirty(_selectedEnemyDetails);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = _selectedEnemyDetails;
        }
    }

}
