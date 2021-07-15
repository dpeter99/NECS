using System.Collections.Generic;
using NECS.Runtime;
using src.Runtime.Components;
using UnityEditor;

namespace src.Runtime
{
    [InitializeOnLoad]
    public static class ECSManager
    {
        private static ECSWorldManager _worldManager = new ECSWorldManager();

        
        public static void Init()
        {
            _worldManager = new ECSWorldManager();
        }
        
        public static List<ComponentRef> GetComponents(Entity entity)
        {
            return _worldManager.AllWorlds[(int)entity._woldID].GetComponents(entity);
            //return null;
        }

        
    }
}