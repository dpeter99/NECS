using System;
using System.Collections;
using System.Collections.Generic;
using NECS.Runtime;
using src.Runtime;
using src.Runtime.Types;
using Unity.Collections;

namespace NECS
{
    public class EcsWorld : IDisposable
    {
        private readonly string _name;
        public string Name => _name;

        private readonly long _ID;
        
        public long ID => _ID;
        

        private EntityManager _manager;
        
        private int _nextID;

        public EcsWorld(string name)
        {
            _name = name;
            _ID = ECSWorldManager.Instance.GetNewWorldID();
            
            _manager = new EntityManager(this);
        }

        public IEnumerable<EntityComponentLinker> Entities => _manager.GetEntities();

        

        public void Init()
        {
            
        }

        public Entity AddNewEntity()
        {
            var e = _manager.CreateEntity();
            
            return e;
        }

        public T AddComponent<T>(Entity ent) where T : unmanaged, IComponent
        {
            return _manager.AddComponent<T>(ent);
        }

        public List<ComponentRef> GetComponents(Entity entity)
        {
            return _manager.GetComponentsForEntity(entity);
            //return null;
        }
        
        public void Dispose()
        {
            _manager?.Dispose();
        }
    }
}