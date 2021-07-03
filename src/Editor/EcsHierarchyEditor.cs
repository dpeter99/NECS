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
    [InitializeOnLoad]
    public class EcsHierarchyEditor
    {
        static EcsHierarchyEditor()
        {
            SceneHierarchyHooks.provideSubScenes = ProvideSubScenes;
            SceneHierarchyHooks.provideSubSceneName = ProvideSubSceneName;
            //SceneHierarchyHooks.addItemsToGameObjectContextMenu += SubSceneContextMenu.AddExtraGameObjectContextMenuItems;
            //SceneHierarchyHooks.addItemsToSceneHeaderContextMenu += SubSceneContextMenu.AddExtraSceneHeaderContextMenuItems;
            
        }

        static SceneHierarchyHooks.SubSceneInfo[] ProvideSubScenes()
        {
            var scenes = new SceneHierarchyHooks.SubSceneInfo[EcsWorldTest.AllWorlds.Count];

            int index = 0;
            foreach (var world in EcsWorldTest.AllWorlds)
            {
                scenes[index].transform = world.transform;
                scenes[index].sceneName = "Tst";
                scenes[index].sceneAsset = world.scene;
                scenes[index].scene = world.EditingScene;

                index++;
            }
            
            

            return scenes;
        }

        static string ProvideSubSceneName(SceneHierarchyHooks.SubSceneInfo subSceneInfo)
        {
            return "Test scene name";
        }
        
    }
}