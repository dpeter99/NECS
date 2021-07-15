using UnityEditor;
using UnityEngine.UIElements;

namespace NECS.Editor
{
    static public class Resources
    {
        public const string PackageName = "com.dpeter99.necs";
        public const string PackagePath = "Packages/" + PackageName;

        public const string EditorDefaultResourcesPath = PackagePath + "/Resources/";
        
        //public static readonly UITemplate EntityHierarchyItem = new UITemplate("entity-hierarchy-item.uxml");
        public static VisualTreeAsset EntityHierarchyItem => EditorGUIUtility.Load( EditorDefaultResourcesPath + "entity-hierarchy-item.uxml") as VisualTreeAsset;
    }
}