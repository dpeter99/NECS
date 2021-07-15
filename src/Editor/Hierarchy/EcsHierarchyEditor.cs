using System;
using System.Collections.Generic;
using System.Linq;
using src.Runtime;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


namespace NECS.Editor
{
    public class EcsHierarchyEditor : EditorWindow
    {
        
        private VisualElement m_Root;
        private EntityHierarchy m_EntityHierarchy;

        [MenuItem("Window/My Window")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            EcsHierarchyEditor window = (EcsHierarchyEditor)EditorWindow.GetWindow(typeof(EcsHierarchyEditor));
            window.Show();
        }

        void CreateGUI()
        {
            //titleContent = new GUIContent(k_WindowName, EditorIcons.EntityGroup);
            //minSize = k_MinWindowSize;

            m_Root = new VisualElement { style = { flexGrow = 1 }, name = "hierarchy_root"};
            rootVisualElement.Add(m_Root);
            
            
            m_EntityHierarchy = new EntityHierarchy();
            m_Root.Add(m_EntityHierarchy);

            m_EntityHierarchy.Refresh(this);
            
            rootVisualElement.Add(m_Root);
        }

        private void Update()
        {
            if (ECSWorldManager.Instance.worldDirty)
            {
                //m_EntityHierarchy = rootVisualElement.Q<EntityHierarchy>();

                if (m_EntityHierarchy != null)
                {
                    m_EntityHierarchy.Refresh(this);
                }

                Debug.Log("Update Tree");
                
                ECSWorldManager.Instance.ClearDirty();
            }
        }
        
    }
}