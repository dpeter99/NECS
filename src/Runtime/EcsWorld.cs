using System.Collections;
using System.Collections.Generic;
using NECS.Runtime;
using Unity.Collections;

namespace NECS
{
    public class EcsWorld
    {
        private EntityManager _manager = new EntityManager();
        
        
        private int _nextID;
        public IEnumerable<Entity> Entities => _manager.GetEntities();

        public void Init()
        {
            
        }

        public rtm_ptr AddNewEntity()
        {
            var e = _manager.CreateEntity();
            
            return e;
        }
    }
}