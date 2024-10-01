using UnityEditor;
using UnityEngine;

public class PoolEditorWindow : EditorWindow
{
    private PoolManager _poolManagerPrefab;
    private SerializedObject _serializedPoolObject;
    private SerializedProperty _poolListProp;

    [MenuItem("Tools/Pool Creator")]
    private static void ShowWindow()
    {
        var window = GetWindow<PoolEditorWindow>();
        window.titleContent = new GUIContent("PoolCreator");
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Space(10);

        GUILayout.Label("Pool Editor", EditorStyles.boldLabel);

        _poolManagerPrefab = (PoolManager)EditorGUILayout.ObjectField("Pool Manager", _poolManagerPrefab, typeof(PoolManager), false);

        if (_poolManagerPrefab != null)
        {
            EditPoolManager();

            if (GUILayout.Button("Save Pool Manager"))
            {
                SavePoolManager();
            }         
        }
    }

    private void EditPoolManager()
    {
        if (_serializedPoolObject == null || _serializedPoolObject.targetObject != _poolManagerPrefab)
        {
            _serializedPoolObject = new SerializedObject(_poolManagerPrefab);

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
