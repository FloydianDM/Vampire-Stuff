using UnityEditor;
using UnityEngine;

public class PoolEditorWindow : EditorWindow
{
    private GameObject _poolManagerPrefab;
    private SerializedObject _serializedPoolObject;
    private SerializedProperty _poolListProp;
    private Vector2 _scrollPosition = Vector2.zero;

    [MenuItem("Tools/Pool Creator")]
    private static void ShowWindow()
    {
        var window = GetWindow<PoolEditorWindow>();
        window.titleContent = new GUIContent("PoolCreator");
        window.Show();
    }

    private void OnGUI()
    {
        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, true, true);

        GUILayout.Space(10);

        GUILayout.Label("Pool Editor", EditorStyles.boldLabel);

        if (_poolManagerPrefab == null)
        {
            _poolManagerPrefab = Resources.Load<GameObject>("PoolManager");
        }

        if (_poolManagerPrefab != null)
        {
            EditPoolManager();

            if (GUILayout.Button("Save Pool Manager"))
            {
                SavePoolManager();
            }         
        }

        GUILayout.EndScrollView();
    }

    private void EditPoolManager()
    {
        PoolManager poolManager = _poolManagerPrefab.GetComponent<PoolManager>();
        
        if (_serializedPoolObject == null || _serializedPoolObject.targetObject != poolManager)
        {
            _serializedPoolObject = new SerializedObject(poolManager);

            // get values from pool manager
            _poolListProp = _serializedPoolObject.FindProperty("_poolList");
        }

        _serializedPoolObject.Update();

        EditorGUILayout.PropertyField(_poolListProp);

        _serializedPoolObject.ApplyModifiedProperties();
    }

    private void SavePoolManager()
    {
        if (_poolManagerPrefab != null)
        {
            EditorUtility.SetDirty(_poolManagerPrefab);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = _poolManagerPrefab;
        }
    }
}
