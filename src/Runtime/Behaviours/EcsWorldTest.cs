using System;
using System.Collections.Generic;
using src.Runtime;
using src.Runtime.Components;
using Unity.Collections;
using UnityEditor;
/*using UnityEditor.SceneManagement;*/
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
        
        
        //TODO: Loading
        public bool IsLoaded => true;

        void OnEnable()
        {
            
            _world = new EcsWorld(name);
            _world.Init();
            
            ECSWorldManager.Instance.AddWorld(_world);

            var ent = _world.AddNewEntity();

            _world.AddComponent<Position>(ent);
            
            
            _world.AddNewEntity();
            _world.AddNewEntity();
        }

        private void OnDisable()
        {
            ECSWorldManager.Instance.Remove(_world);
            _world.Dispose();
            _world = null;
        }

        ~EcsWorldTest()
        {
            if (_world != null)
            {
                _world.Dispose();
            }
        }
        
        private void Start()
        {

        }
    }
}