using UnityEditor;
using UnityEngine;

// editor for creating upgrade details SOs
public class UpgradeEditorWindow : EditorWindow
{
    #region Prefabs
    private UpgradeTypeEnum _upgradeTypeEnum;
    private GameObject _basePrefab;
    private GameObject _tempPrefabVariant;
    private GameObject _newPrefab;
    private string _newPrefabVariantName;
    private GameObject _upgradeSpawnerPrefab;
    private UpgradeSpawner _upgradeSpawner;
    private SerializedObject _serializedUpgradeSpawnerObject;
    private SerializedProperty _bombUpgradePrefabListProp;
    private SerializedProperty _weaponEnhancerUpgradePrefabListProp;
    private SerializedProperty _speedEnhancerUpgradePrefabListProp;
    #endregion
    
    #region Scriptable Objects
    private string _upgradeName;
    private readonly string _bombUpgradeAssetPath = "Assets/ScriptableObjects/Upgrades/Bomb/";
    private readonly string _weaponEnhancerUpgradeAssetPath = "Assets/ScriptableObjects/Upgrades/WeaponEnhancer/";
    private readonly string _speedEnhancerUpgradeAssetPath = "Assets/ScriptableObjects/Upgrades/SpeedEnhancer/";
    private BombUpgradeDetailsSO _newBombUpgradeDetails;
    private WeaponEnhancerUpgradeDetailsSO _newWeaponEnhancerUpgradeDetails;
    private SpeedEnhancerUpgradeDetailsSO _newSpeedEnhancerUpgradeDetails;
    private BombUpgradeDetailsSO _selectedBombUpgradeDetails;
    private WeaponEnhancerUpgradeDetailsSO _selectedWeaponEnhancerUpgradeDetails;
    private SpeedEnhancerUpgradeDetailsSO _selectedSpeedEnhancerUpgradeDetails;
    private SerializedObject _serializedUpgradeScriptableObject;
    private SerializedProperty _typeProp;
    private SerializedProperty _spriteProp;
    private SerializedProperty _spriteColorProp;
    private SerializedProperty _upgradeTypeProp;
    private SerializedProperty _impactAreaProp;
    private SerializedProperty _cooldownTimeProp;
    private SerializedProperty _damageProp;
    private SerializedProperty _attackModifierProp;
    private SerializedProperty _speedModifierProp;
    #endregion

    private bool _isNameAndEnumInitialized;
    private bool _isTypeEnumInitialized;
    private UpgradeTypeEnum _typeEnumForModification;
    
    [MenuItem("Tools/Upgrade Creator")]
    private static void ShowWindow()
    {
        var window = GetWindow<UpgradeEditorWindow>();
        window.titleContent = new GUIContent("Upgrade Creator");
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Space(20);

        GUILayout.Label("Create Upgrades", EditorStyles.boldLabel);

        if (!_isNameAndEnumInitialized)
        {
            _upgradeName = EditorGUILayout.TextField("Upgrade Name", _upgradeName);
            _upgradeTypeEnum = (UpgradeTypeEnum)EditorGUILayout.EnumPopup("Select Upgrade Type", _upgradeTypeEnum);
            
            switch (_upgradeTypeEnum)
            {
                case UpgradeTypeEnum.Bomb:
                    _basePrefab = Resources.Load<GameObject>("BombUpgrade");
                    break;

                case UpgradeTypeEnum.WeaponEnhancer:
                    _basePrefab = Resources.Load<GameObject>("WeaponEnhancer");
                    break;

                case UpgradeTypeEnum.SpeedEnhancer:
                    _basePrefab = Resources.Load<GameObject>("SpeedEnhancer");
                    break;
            }

            if (_upgradeSpawnerPrefab == null)
            {
                _upgradeSpawnerPrefab = Resources.Load<GameObject>("UpgradeSpawner");
            }

            if (GUILayout.Button("Create New Upgrade"))
            {
                _isNameAndEnumInitialized = true;

                if (_basePrefab == null || _upgradeSpawnerPrefab == null)
                {
                    GUIContent notificationWindow = new GUIContent("There is no base prefab and/or upgrade spawner prefab in resources");
                    ShowNotification(notificationWindow);
                }
                else
                {
                    CreateUpgradeDetailsScriptableObject();
                }
            }
        }

        if (_newBombUpgradeDetails != null)
        {
            GUILayout.Label("Initialize Bomb Upgrade Details", EditorStyles.boldLabel);

            InitializeUpgradeDetailsScriptableObject();

            if (GUILayout.Button("Save Bomb Upgrade Details Scriptable Object"))
            {
                SaveScriptableObject();
                CreatePrefabVariant();
            }
        }
        else if (_newWeaponEnhancerUpgradeDetails != null)
        {
            GUILayout.Label("Initialize Weapon Enhancer Upgrade Details", EditorStyles.boldLabel);

            InitializeUpgradeDetailsScriptableObject();

            if (GUILayout.Button("Save Weapon Enhancer Upgrade Details Scriptable Object"))
            {
                SaveScriptableObject();
                CreatePrefabVariant();
            }
        }
        else if (_newSpeedEnhancerUpgradeDetails != null)
        {
            GUILayout.Label("Initialize Speed Enhancer Upgrade Details", EditorStyles.boldLabel);

            InitializeUpgradeDetailsScriptableObject();

            if (GUILayout.Button("Save Speed Enhancer Upgrade Details Scriptable Object"))
            {
                SaveScriptableObject();
                CreatePrefabVariant();
            }
        }

        if (_newPrefab != null && _upgradeSpawner == null)
        {
            _upgradeSpawner = AddUpgradeToUpgradeSpawner(_newPrefab);
        }
        

        if (_upgradeSpawner != null)
        {
            EditUpgradeSpawner();

            if (GUILayout.Button("Save Upgrade Spawner"))
            {
                SaveUpgradeSpawner();
            }
        }

        GUILayout.Space(20);
        
        // edit upgrade section
        
        GUILayout.Label("Edit Upgrade Details Scriptable Object", EditorStyles.boldLabel);

        if (!_isTypeEnumInitialized)
        {
            _typeEnumForModification = (UpgradeTypeEnum)EditorGUILayout.EnumPopup(
                "Select Type of Upgrade", _typeEnumForModification);    
        }
            
        switch (_typeEnumForModification)
        {
            case UpgradeTypeEnum.Bomb:
                _selectedBombUpgradeDetails = (BombUpgradeDetailsSO)EditorGUILayout.ObjectField(
                    "Select Bomb Upgrade Details", _selectedBombUpgradeDetails, typeof(BombUpgradeDetailsSO), false);

                if (_selectedBombUpgradeDetails != null)
                {
                    _isTypeEnumInitialized = true;
                    
                    EditUpgradeScriptableObject(_typeEnumForModification);

                    if (GUILayout.Button("Save Bomb Upgrade Scriptable Object"))
                    {
                        SaveEditedScriptableObject(_typeEnumForModification);
                    }
                }

                break;

            case UpgradeTypeEnum.WeaponEnhancer:
                _selectedWeaponEnhancerUpgradeDetails = (WeaponEnhancerUpgradeDetailsSO)EditorGUILayout.ObjectField(
                    "Select Weapon Enhancer Upgrade Details", _selectedWeaponEnhancerUpgradeDetails,
                    typeof(WeaponEnhancerUpgradeDetailsSO), false);

                if (_selectedWeaponEnhancerUpgradeDetails != null)
                {
                    _isTypeEnumInitialized = true;

                    EditUpgradeScriptableObject(_typeEnumForModification);

                    if (GUILayout.Button("Save Weapon Enhancer Upgrade Scriptable Object"))
                    {
                        SaveEditedScriptableObject(_typeEnumForModification);
                    }
                }

                break;

            case UpgradeTypeEnum.SpeedEnhancer:
                _selectedSpeedEnhancerUpgradeDetails = (SpeedEnhancerUpgradeDetailsSO)EditorGUILayout.ObjectField(
                    "Select Weapon Enhancer Upgrade Details", _selectedSpeedEnhancerUpgradeDetails,
                    typeof(SpeedEnhancerUpgradeDetailsSO),
                    false);

                if (_selectedSpeedEnhancerUpgradeDetails != null)
                {
                    _isTypeEnumInitialized = true;

                    EditUpgradeScriptableObject(_typeEnumForModification);

                    if (GUILayout.Button("Save Speed Enhancer Upgrade Scriptable Object"))
                    {
                        SaveEditedScriptableObject(_typeEnumForModification);
                    }
                }
                    
                break;
        }
    }

    private void CreateUpgradeDetailsScriptableObject()
    {
        switch (_upgradeTypeEnum)
        {
            case UpgradeTypeEnum.Bomb:
                _newBombUpgradeDetails = CreateInstance<BombUpgradeDetailsSO>();
                break;
            
            case UpgradeTypeEnum.WeaponEnhancer:
                _newWeaponEnhancerUpgradeDetails = CreateInstance<WeaponEnhancerUpgradeDetailsSO>();
                break;
            
            case UpgradeTypeEnum.SpeedEnhancer:
                _newSpeedEnhancerUpgradeDetails = CreateInstance<SpeedEnhancerUpgradeDetailsSO>();
                break;
        }
    }

    private void InitializeUpgradeDetailsScriptableObject()
    {
        switch (_upgradeTypeEnum)
        {
            case UpgradeTypeEnum.Bomb:
                _newBombUpgradeDetails.Type = _upgradeName;
                _newBombUpgradeDetails.Sprite = (Sprite)EditorGUILayout.ObjectField(
                    "Sprite", _newBombUpgradeDetails.Sprite, typeof(Sprite), false);
                _newBombUpgradeDetails.SpriteColor = EditorGUILayout.ColorField("Sprite Color", _newBombUpgradeDetails.SpriteColor);
                _newBombUpgradeDetails.UpgradeType = _upgradeTypeEnum;
                _newBombUpgradeDetails.ImpactArea = EditorGUILayout.FloatField("Impact Area", _newBombUpgradeDetails.ImpactArea);
                _newBombUpgradeDetails.CooldownTime = EditorGUILayout.FloatField("Cooldown Time", _newBombUpgradeDetails.CooldownTime);
                _newBombUpgradeDetails.Damage = EditorGUILayout.IntField("Damage", _newBombUpgradeDetails.Damage);
                break;

            case UpgradeTypeEnum.WeaponEnhancer:
                _newWeaponEnhancerUpgradeDetails.Type = _upgradeName;
                _newWeaponEnhancerUpgradeDetails.Sprite = (Sprite)EditorGUILayout.ObjectField(
                    "Sprite", _newWeaponEnhancerUpgradeDetails.Sprite, typeof(Sprite), false);
                _newWeaponEnhancerUpgradeDetails.SpriteColor = EditorGUILayout.ColorField(
                    "Sprite Color", _newWeaponEnhancerUpgradeDetails.SpriteColor);
                _newWeaponEnhancerUpgradeDetails.UpgradeType = _upgradeTypeEnum;
                _newWeaponEnhancerUpgradeDetails.AttackModifier= EditorGUILayout.FloatField(
                    "Attack Modifier", _newWeaponEnhancerUpgradeDetails.AttackModifier);
                break;

            case UpgradeTypeEnum.SpeedEnhancer:
                _newSpeedEnhancerUpgradeDetails.Type = _upgradeName;
                _newSpeedEnhancerUpgradeDetails.Sprite = (Sprite)EditorGUILayout.ObjectField(
                    "Sprite", _newSpeedEnhancerUpgradeDetails.Sprite, typeof(Sprite), false);
                _newSpeedEnhancerUpgradeDetails.SpriteColor = EditorGUILayout.ColorField(
                    "Sprite Color", _newSpeedEnhancerUpgradeDetails.SpriteColor);
                _newSpeedEnhancerUpgradeDetails.UpgradeType = _upgradeTypeEnum;
                _newSpeedEnhancerUpgradeDetails.SpeedModifier= EditorGUILayout.FloatField(
                    "Speed Modifier", _newSpeedEnhancerUpgradeDetails.SpeedModifier);
                break;   
        }        
    }

    private void SaveScriptableObject()
    {
        string path = string.Empty;

        switch (_upgradeTypeEnum)
        {
            case UpgradeTypeEnum.Bomb:
                path = AssetDatabase.GenerateUniqueAssetPath(_bombUpgradeAssetPath + "BombUpgradeDetails_" + _upgradeName + ".asset");
                AssetDatabase.CreateAsset(_newBombUpgradeDetails, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = _newBombUpgradeDetails;
                break;

            case UpgradeTypeEnum.WeaponEnhancer:
                path = AssetDatabase.GenerateUniqueAssetPath(
                    _weaponEnhancerUpgradeAssetPath + "WeaponEnhancerUpgradeDetails_" + _upgradeName + ".asset");
                AssetDatabase.CreateAsset(_newWeaponEnhancerUpgradeDetails, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = _newWeaponEnhancerUpgradeDetails;
                break;       

            case UpgradeTypeEnum.SpeedEnhancer:             
                path = AssetDatabase.GenerateUniqueAssetPath(
                    _speedEnhancerUpgradeAssetPath + "SpeedEnhancerUpgradeDetails_" + _upgradeName + ".asset");
                AssetDatabase.CreateAsset(_newSpeedEnhancerUpgradeDetails, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = _newSpeedEnhancerUpgradeDetails;
                break;
        }
    }

    private void CreatePrefabVariant()
    {
        _newPrefabVariantName = _upgradeName;
        _tempPrefabVariant = (GameObject)PrefabUtility.InstantiatePrefab(_basePrefab); // temp
        
        if (_tempPrefabVariant.TryGetComponent(out BombUpgrade bombUpgrade))
        {
            bombUpgrade.BombUpgradeDetails = _newBombUpgradeDetails;
        }
        else if (_tempPrefabVariant.TryGetComponent(out WeaponEnhancerUpgrade weaponEnhancerUpgrade))
        {
            weaponEnhancerUpgrade.WeaponEnhancerUpgradeDetails = _newWeaponEnhancerUpgradeDetails;
        }
        else if (_tempPrefabVariant.TryGetComponent(out SpeedEnhancerUpgrade speedEnhancerUpgrade))
        {
            speedEnhancerUpgrade.SpeedEnhancerUpgradeDetails = _newSpeedEnhancerUpgradeDetails;
        }

        SavePrefabVariant();
    }

    private void SavePrefabVariant()
    {
        string path = string.Empty;

        switch (_upgradeTypeEnum)
        {
            case UpgradeTypeEnum.Bomb:
                path = "Assets/Prefabs/Upgrades/Bomb/BombUpgrade_" + _newPrefabVariantName + ".prefab";
                path = AssetDatabase.GenerateUniqueAssetPath(path);
                break;
            
            case UpgradeTypeEnum.WeaponEnhancer:
                path = "Assets/Prefabs/Upgrades/WeaponEnhancer/WeaponEnhancer_" + _newPrefabVariantName + ".prefab";
                path = AssetDatabase.GenerateUniqueAssetPath(path);   
                break;
            
            case UpgradeTypeEnum.SpeedEnhancer:
                path = "Assets/Prefabs/Upgrades/SpeedEnhancer/SpeedEnhancer_" + _newPrefabVariantName + ".prefab";
                path = AssetDatabase.GenerateUniqueAssetPath(path);
                break;  
        }

        _newPrefab = PrefabUtility.SaveAsPrefabAssetAndConnect(_tempPrefabVariant, path, InteractionMode.UserAction);
        
        DestroyImmediate(_tempPrefabVariant);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = _newPrefab;
    }
    
    private UpgradeSpawner AddUpgradeToUpgradeSpawner(GameObject prefab)
    {
        UpgradeSpawner upgradeSpawner = _upgradeSpawnerPrefab.GetComponent<UpgradeSpawner>();
        upgradeSpawner.AddUpgradeToList(prefab);

        return upgradeSpawner;
    }

    private void EditUpgradeSpawner()
    {
        if (_serializedUpgradeSpawnerObject == null || _serializedUpgradeSpawnerObject.targetObject != _upgradeSpawner)
        {
            _serializedUpgradeSpawnerObject = new SerializedObject(_upgradeSpawner);

            _bombUpgradePrefabListProp = _serializedUpgradeSpawnerObject.FindProperty("_bombList");
            _weaponEnhancerUpgradePrefabListProp = _serializedUpgradeSpawnerObject.FindProperty("_weaponEnhancerList");
            _speedEnhancerUpgradePrefabListProp = _serializedUpgradeSpawnerObject.FindProperty("_speedEnhancerList");
        }

        _serializedUpgradeSpawnerObject.Update();

        EditorGUILayout.PropertyField(_bombUpgradePrefabListProp);
        EditorGUILayout.PropertyField(_weaponEnhancerUpgradePrefabListProp);
        EditorGUILayout.PropertyField(_speedEnhancerUpgradePrefabListProp);

        _serializedUpgradeSpawnerObject.ApplyModifiedProperties();
    }

    private void SaveUpgradeSpawner()
    {
        if (_upgradeSpawnerPrefab != null)
        {
            EditorUtility.SetDirty(_upgradeSpawnerPrefab);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
        }
    }
    
    private void EditUpgradeScriptableObject(UpgradeTypeEnum upgradeTypeEnum)
    {
        switch (upgradeTypeEnum)
        {
            case UpgradeTypeEnum.Bomb:
                if (_serializedUpgradeScriptableObject == null ||
                    _serializedUpgradeScriptableObject.targetObject != _selectedBombUpgradeDetails)
                {
                    _serializedUpgradeScriptableObject = new SerializedObject(_selectedBombUpgradeDetails);

                    _typeProp = _serializedUpgradeScriptableObject.FindProperty("Type");
                    _spriteProp = _serializedUpgradeScriptableObject.FindProperty("Sprite");
                    _spriteColorProp = _serializedUpgradeScriptableObject.FindProperty("SpriteColor");
                    _impactAreaProp = _serializedUpgradeScriptableObject.FindProperty("ImpactArea");
                    _cooldownTimeProp = _serializedUpgradeScriptableObject.FindProperty("CooldownTime");
                    _damageProp = _serializedUpgradeScriptableObject.FindProperty("Damage");
                }
                
                _serializedUpgradeScriptableObject.Update();

                EditorGUILayout.PropertyField(_typeProp);
                EditorGUILayout.PropertyField(_spriteProp);
                EditorGUILayout.PropertyField(_spriteColorProp);
                EditorGUILayout.PropertyField(_impactAreaProp);
                EditorGUILayout.PropertyField(_cooldownTimeProp);
                EditorGUILayout.PropertyField(_damageProp);

                _serializedUpgradeScriptableObject.ApplyModifiedProperties();
                break;
            
            case UpgradeTypeEnum.WeaponEnhancer:
                if (_serializedUpgradeScriptableObject == null ||
                    _serializedUpgradeScriptableObject.targetObject != _selectedWeaponEnhancerUpgradeDetails)
                {
                    _serializedUpgradeScriptableObject = new SerializedObject(_selectedWeaponEnhancerUpgradeDetails);

                    _typeProp = _serializedUpgradeScriptableObject.FindProperty("Type");
                    _spriteProp = _serializedUpgradeScriptableObject.FindProperty("Sprite");
                    _spriteColorProp = _serializedUpgradeScriptableObject.FindProperty("SpriteColor");
                    _attackModifierProp = _serializedUpgradeScriptableObject.FindProperty("AttackModifier");
                }
                
                _serializedUpgradeScriptableObject.Update();

                EditorGUILayout.PropertyField(_typeProp);
                EditorGUILayout.PropertyField(_spriteProp);
                EditorGUILayout.PropertyField(_spriteColorProp);
                EditorGUILayout.PropertyField(_attackModifierProp);

                _serializedUpgradeScriptableObject.ApplyModifiedProperties();
                break;
            
            case UpgradeTypeEnum.SpeedEnhancer:
                if (_serializedUpgradeScriptableObject == null ||
                    _serializedUpgradeScriptableObject.targetObject != _selectedSpeedEnhancerUpgradeDetails)
                {
                    _serializedUpgradeScriptableObject = new SerializedObject(_selectedSpeedEnhancerUpgradeDetails);

                    _typeProp = _serializedUpgradeScriptableObject.FindProperty("Type");
                    _spriteProp = _serializedUpgradeScriptableObject.FindProperty("Sprite");
                    _spriteColorProp = _serializedUpgradeScriptableObject.FindProperty("SpriteColor");
                    _speedModifierProp = _serializedUpgradeScriptableObject.FindProperty("SpeedModifier");
                }
                
                _serializedUpgradeScriptableObject.Update();

                EditorGUILayout.PropertyField(_typeProp);
                EditorGUILayout.PropertyField(_spriteProp);
                EditorGUILayout.PropertyField(_spriteColorProp);
                EditorGUILayout.PropertyField(_speedModifierProp);

                _serializedUpgradeScriptableObject.ApplyModifiedProperties();
                break;
        }
    }
    
    private void SaveEditedScriptableObject(UpgradeTypeEnum upgradeTypeEnum)
    {
        switch (upgradeTypeEnum)
        {
            case UpgradeTypeEnum.Bomb:
                if (_selectedBombUpgradeDetails != null)
                {
                    EditorUtility.SetDirty(_selectedBombUpgradeDetails);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                    EditorUtility.FocusProjectWindow();
                    Selection.activeObject = _selectedBombUpgradeDetails;
                }
                break;
            
            case UpgradeTypeEnum.WeaponEnhancer:
                if (_selectedWeaponEnhancerUpgradeDetails != null)
                {
                    EditorUtility.SetDirty(_selectedWeaponEnhancerUpgradeDetails);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                    EditorUtility.FocusProjectWindow();
                    Selection.activeObject = _selectedWeaponEnhancerUpgradeDetails;
                }
                break;
            
            case UpgradeTypeEnum.SpeedEnhancer:
                if (_selectedSpeedEnhancerUpgradeDetails != null)
                {
                    EditorUtility.SetDirty(_selectedSpeedEnhancerUpgradeDetails);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                    EditorUtility.FocusProjectWindow();
                    Selection.activeObject = _selectedSpeedEnhancerUpgradeDetails;
                }
                break;
        }
    }
}
