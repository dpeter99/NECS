using System;
using System.Collections.Generic;
using System.Linq;
using NECS.Editor.Hierarchy;
using Unity.Collections;
/*using Unity.Editor.Bridge;
using Unity.Properties.UI;*/
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UIElements;


namespace NECS.Editor
{
    internal class EntityHierarchy: VisualElement
    {
        private readonly VisualElement m_ViewContainer;

        private IMGUIContainer _treeView;
        
        private List<ITreeViewItem> m_TreeViewRootItems = new List<ITreeViewItem>();
        private readonly TreeView m_TreeView;

        public EntityHierarchy()
        {
            //m_EntityHierarchyFoldingState = entityHierarchyFoldingState;

            style.flexGrow = 1.0f;
            m_ViewContainer = new VisualElement();
            m_ViewContainer.style.flexGrow = 1.0f;
            m_ViewContainer.RegisterCallback<PointerDownEvent>(evt =>
            {
                if (evt.button == (int)MouseButton.LeftMouse)
                    Selection.activeObject = null;
            });

            _treeView = new IMGUIContainer()
            {
                style = { flexGrow = 1 },
                onGUIHandler = DrawTree
            };
            m_ViewContainer.Add(_treeView);

            /*
            m_TreeView = new TreeView(m_TreeViewRootItems, 30, MakeTreeViewItem, BindTreeViewItem)
            {
                selectionType = SelectionType.Single,
                name = "FullView",
                style = { flexGrow = 1 },
            };
            */
            

            /*
            m_TreeView.onSelectionChange += OnLocalSelectionChanged;
            m_TreeView.ItemExpandedStateChanging += (item, isExpanding) =>
            {
                var entityHierarchyItem = (EntityHierarchyItem)item;
                if (entityHierarchyItem.NodeId.Kind == NodeKind.Scene || entityHierarchyItem.NodeId.Kind == NodeKind.SubScene)
                    m_EntityHierarchyFoldingState.OnFoldingStateChanged(entityHierarchyItem.NodeId, isExpanding);
            };
            m_TreeView.Hide();
            */
            
            //m_ViewContainer.Add(m_TreeView);
            
            /*
            m_ListView = new ListView(m_ListViewFilteredItems, Constants.ListView.ItemHeight, MakeListViewItem, ReleaseListViewItem, BindListViewItem)
            {
                selectionType = SelectionType.Single,
                name = Constants.EntityHierarchy.SearchViewName,
                style = { flexGrow = 1 }
            };

            m_ListView.Hide();
            m_ViewContainer.Add(m_ListView);

            m_SearchEmptyMessage = new CenteredMessageElement();
            m_SearchEmptyMessage.Hide();
            Add(m_SearchEmptyMessage);

#if UNITY_2020_1_OR_NEWER
            m_ListView.onSelectionChange += OnLocalSelectionChanged;
#else
            m_ListView.onSelectionChanged += OnSelectionChanged;
#endif

            m_ItemsCache = new HierarchyItemsCache();

            m_CurrentViewMode = ViewMode.Full;

            Add(m_ViewContainer);
            Selection.selectionChanged += OnGlobalSelectionChanged;
            */

            Add(m_ViewContainer);
        }


        private HierarchyIMGUI a = new HierarchyIMGUI(new TreeViewState());
        private void DrawTree()
        {
            var rect = _treeView.layout;
            a.OnGUI(new Rect(0, 0, rect.width, rect.height));
            
            
            
            
        }

        static void BindTreeViewItem(VisualElement element, ITreeViewItem item)
        {
            
        }
        
        private void ReleaseTreeViewItem(VisualElement arg1, ITreeViewItem arg2)
        {
            
        }

        public void Refresh(EcsHierarchyEditor ecsHierarchyEditor)
        {
            OnUpdate();
        }
        
        public void OnUpdate()
        {
            
            RecreateRootItems();
            //RecreateItemsToExpand();
            //RefreshView();
            //m_TreeView.Refresh();
        }

        private void RecreateRootItems()
        {
            m_TreeViewRootItems.Clear();
            
            m_TreeViewRootItems.Add(new TreeViewItem<string>(0,"asd"));
        }

        private VisualElement MakeTreeViewItem()
        {
            VisualElement r = new VisualElement();

            //Resources.EntityHierarchyItem.CloneTree(r);


            return r;
        }
    }
}