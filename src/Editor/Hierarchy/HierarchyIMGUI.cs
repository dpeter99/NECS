using System.Collections.Generic;
using src.Runtime;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace NECS.Editor.Hierarchy
{
    public class HierarchyIMGUI  : UnityEditor.IMGUI.Controls.TreeView
    {
        private Dictionary<int, EntityWrapper> _LUT = new Dictionary<int, EntityWrapper>();
        
        public HierarchyIMGUI(TreeViewState treeViewState)
            : base(treeViewState)
        {
            Reload();
        }
    
        protected override TreeViewItem BuildRoot ()
        {
            var root = new TreeViewItem {id = 0, depth = -1, displayName = "Root"};

            int nextID = 0;
            var allItems = new List<TreeViewItem>();
            
            var worlds = ECSWorldManager.Instance.AllWorlds;

            foreach (var world in worlds)
            {
                Debug.Log(world);
                
                var w = new TreeViewItem(nextID++, 0, world.Name);
                root.AddChild(w);
                foreach (var entity in world.Entities)
                {
                    //GUILayout.Button("Test Id:" + entity.RuntimeUID, GetBtnStyle())
                    var e = new TreeViewItem(nextID, 1, "Entity:" + entity.entitiy.Uuid + $@"({entity.entitiy._runtimeIndex})");
                    w.AddChild(e);
                    allItems.Add(e);
                    
                    
                    
                    _LUT.Add(nextID, EntityWrapper.CreateInstance(entity.entitiy));

                    nextID++;
                }
                
            }
            
            SetupDepthsFromParentsAndChildren(root);
        
            // Return root of the tree
            return root;
        }

        protected override void SelectionChanged(IList<int> selectedIds)
        {
            base.SelectionChanged(selectedIds);

            int selID = this.GetSelection()[0];

            if (_LUT.ContainsKey(selID))
            {
                Selection.activeObject = _LUT[selID];
            }
            
        }
        
        
        
    }
}