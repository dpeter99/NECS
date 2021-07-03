using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace NECS
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    public class EcsWorldTest : MonoBehaviour
    {
        public SceneAsset scene;

        public Scene EditingScene;

        private EcsWorld world;

        private static List<EcsWorldTest> _allWorlds = new List<EcsWorldTest>();

        public static List<EcsWorldTest> AllWorlds
        {
            get => _allWorlds;
            set => _allWorlds = value;
        }

        //TODO: Loading
        public bool IsLoaded => true;

        void OnEnable()
        {
            _allWorlds.Add(this);

            //EditingScene = EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(scene));
            
            //SceneHierarchyHooks.ReloadAllSceneHierarchies();
        }

        private void OnDisable()
        {
            _allWorlds.Remove(this);
        }

        private void Start()
        {
            //world = new EcsWorld();
            //world.Init();

            //world.AddNewEntity();
            //world.AddNewEntity();
            //world.AddNewEntity();
        }
    }
}