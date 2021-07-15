using System.Collections.Generic;

namespace NECS.Editor.Hierarchy
{
    public class EntityHierarchyItem : ITreeViewItem
    {
        public int id { get; }
        public ITreeViewItem parent { get; }
        public IEnumerable<ITreeViewItem> children { get; }
        public bool hasChildren { get; }
        public void AddChild(ITreeViewItem child)
        {
            throw new System.NotImplementedException();
        }

        public void AddChildren(IList<ITreeViewItem> children)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveChild(ITreeViewItem child)
        {
            throw new System.NotImplementedException();
        }
    }
}