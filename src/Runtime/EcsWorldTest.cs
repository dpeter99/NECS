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
        private EcsWorld _world;
        public EcsWorld World => _world;
        
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
            _world = new EcsWorld();
            _world.Init();

            _world.AddNewEntity();
            _world.AddNewEntity();
            _world.AddNewEntity();
        }

        private void OnDisable()
        {
            _allWorlds.Remove(this);
            
        }

        private void Start()
        {

        }
    }
}