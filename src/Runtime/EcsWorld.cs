using Unity.Collections;

namespace NECS
{
    public struct EcsWorld
    {
        private NativeList<Entity> _entities;

        
        
        private int _nextID;

        public void Init()
        {
            _entities = new NativeList<Entity>(Allocator.Persistent);
        }

        public Entity AddNewEntity()
        {
            var e = new Entity(_nextID);
            _nextID++;

            _entities.Add(e);
            
            return e;
        }
    }
}