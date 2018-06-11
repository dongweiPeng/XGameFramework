using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Pool;

[CustomEditor(typeof(SpawnPool))]
public class SpawnPoolInspector : Editor
{
    public bool expandPrefabs = true;

    public override void OnInspectorGUI()
	{
        var script = (SpawnPool)target;

        EditorGUI.indentLevel = 0;
        PGEditorUtils.LookLikeControls();

        script.poolName = EditorGUILayout.TextField("Pool Name", script.poolName);

        script.matchPoolScale = EditorGUILayout.Toggle("Match Pool Scale", script.matchPoolScale);
        script.matchPoolLayer = EditorGUILayout.Toggle("Match Pool Layer", script.matchPoolLayer);
        
        script.dontReparent = EditorGUILayout.Toggle("Don't Reparent", script.dontReparent);

        script._dontDestroyOnLoad = EditorGUILayout.Toggle("Don't Destroy On Load", script._dontDestroyOnLoad);
        
        script.logMessages = EditorGUILayout.Toggle("Log Messages", script.logMessages);

        this.expandPrefabs = PGEditorUtils.SerializedObjFoldOutList<PrefabPool>
                            (
                                "Per-Prefab Pool Options", 
                                script._perPrefabPoolOptions,
                                this.expandPrefabs,
                                ref script._editorListItemStates,
                                true
                            );

        // Flag Unity to save the changes to to the prefab to disk
        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }

}


