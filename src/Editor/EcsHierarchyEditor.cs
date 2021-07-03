using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace NECS.Editor
{
    public class EcsHierarchyEditor : EditorWindow
    {
        string myString = "Hello World";
        bool groupEnabled;
        bool myBool = true;
        float myFloat = 1.23f;
        
        [MenuItem("Window/My Window")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            EcsHierarchyEditor window = (EcsHierarchyEditor)EditorWindow.GetWindow(typeof(EcsHierarchyEditor));
            window.Show();
        }
        
        void OnGUI()
        {
            var worlds = EcsWorldTest.AllWorlds;

            foreach (var world in worlds)
            {
                foreach (var entity in world.World.Entities)
                {
                    EditorGUILayout.LabelField("Test Id:" + entity.RuntimeUID );
                }
            }
        }
    }
}