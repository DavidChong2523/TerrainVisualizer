using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(mesh_generator))]
public class mesh_generator_editor : Editor
{
    public override void OnInspectorGUI()
    {
        mesh_generator meshGen = (mesh_generator)target;
        DrawDefaultInspector();

        if (DrawDefaultInspector())
        {
            if (meshGen.autoUpdate)
                meshGen.generate_mesh();
        }

        if (GUILayout.Button("Generate"))
        {
            meshGen.generate_mesh();
        }
    }
}
