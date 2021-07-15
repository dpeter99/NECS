using System;
using System.Linq;
using JetBrains.Annotations;
using NECS.Runtime;
using src.Runtime;
using UnityEditor;
using UnityEngine;

using UnityObject = UnityEngine.Object;

namespace NECS.Editor.Inspector
{
    [CustomEditor((typeof(EntityWrapper)))]
    class EntityEditor : UnityEditor.Editor
    {

        
        protected override void OnHeaderGUI()
        {
            //InitStyles();
            //GUILayout.BeginVertical(styles.TitleStyle);
            GUILayout.BeginVertical();
            var targetProxy = (EntityWrapper)target;
            //if (!targetProxy.Exists)
            //    return;

            GUI.enabled = true;
            var entity = targetProxy.Entity;
            var entityName = "Entity:" + entity.Uuid + $@"({entity._runtimeIndex})"; /*targetProxy.World.EntityManager.GetName(entity);*/
            var newName = EditorGUILayout.DelayedTextField(entityName);
            if (newName != entityName)
            {
                //targetProxy.World.EntityManager.SetName(entity, newName);
                //EditorWindow.GetWindow<EntityDebugger>().Repaint();
            }
            GUI.enabled = false;

            GUILayout.Space(2f);
            GUILayout.BeginHorizontal();
            using (new EditorGUI.DisabledScope(true))
            {
                GUILayout.Label("Entity Index");
                GUILayout.TextField(entity._runtimeIndex.ToString(), GUILayout.MinWidth(40f));
                GUILayout.FlexibleSpace();
                GUILayout.Label("UUID");
                GUILayout.TextField(entity._UUID.ToString(), GUILayout.MinWidth(40f));
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
        
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            
            
            
            GUILayout.Label("asdf");
        }
        
        [RootEditor(supportsAddComponent: false), UsedImplicitly]
        public static Type GetEditor(UnityObject[] targets)
        {
            var pooled = targets.OfType<EntityWrapper>().ToList();
            {
                var list = pooled;
                if (list.Count == 0)
                    return null;

                var proxy = list[0];
                if (!proxy)
                    return null;
            }

            //return InspectorUtility.Settings.Backend == InspectorSettings.InspectorBackend.Debug
            //    ? null
            //    : typeof(EntityEditor);

            return typeof(EntityEditor);
        }
    }
}