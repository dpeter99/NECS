using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace NECS.Editor
{
    [CustomEditor(typeof(EcsWorldTest))]
    [CanEditMultipleObjects]
    class EcsWorldEditor : UnityEditor.Editor
    {
        
    }
}