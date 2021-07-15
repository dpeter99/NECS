using NECS.Runtime;
using UnityEditor;
using UnityEngine;

namespace NECS.Editor
{
    public class EntityWrapper : ScriptableObject
    {
        private Entity _entity;
        public Entity Entity => _entity;

        public void Initialize(Entity entityEntitiy)
        {
            _entity = entityEntitiy;
        }
        
        public static EntityWrapper CreateInstance(Entity entity)
        {
            /*
            if (!EntityExistsAndIsValid(world, entity))
                entity = Entity.Null;
            */
            
            var proxy = CreateInstance<EntityWrapper>();
            proxy.hideFlags = HideFlags.HideAndDontSave;
            proxy.Initialize(entity);

            Undo.RegisterCreatedObjectUndo(proxy, "Create EntityWrapper");

            return proxy;
        }
    }
}