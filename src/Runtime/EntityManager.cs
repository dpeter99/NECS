using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NECS;
using NECS.Runtime;
using src.Runtime;
using src.Runtime.Types;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace NECS.Runtime
{
    public unsafe class EntityManager : IDisposable
    {
        
        private EcsWorld _world;
        
        /// <summary>
        /// A list of allocators mapped to the type they are responsible for.
        /// </summary>
        internal Dictionary<Type, IComponentContainer> _containerRegistries = new Dictionary<Type, IComponentContainer>();

        //NativeHashMap<GUID,IComponentContainer> 

        NativeList<IntPtr> _entityLUT = new (ENTITY_LUT_GROW, Allocator.Persistent);
		
        //Extra number of spaces to allocate in the LUT
        const int ENTITY_LUT_GROW = 2048;
        int LUTNextFree = 0;
        bool LUTFragm = false;
        NativeList<int> LUTFragmFree = new NativeList<int>(ENTITY_LUT_GROW, Allocator.Persistent);
        
        /**
		 * \brief The next assignable Unique ID
		 */
        int nextUID = 0;


        public EntityManager(EcsWorld world)
        {
            this._world = world;
        }
        
        /**
		 * \brief Returns the correct container for the entity type
		 * \tparam T The type of the entity
		 * \return The entity container accosted with this type
		 */
        internal ComponentContainer<T> GetComponentContainer<T>() where T : unmanaged
        {
            var CID = typeof(T);

            var it = _containerRegistries.ContainsKey(CID);
            ComponentContainer<T> cc = null;

            if (!_containerRegistries.ContainsKey(CID))
            {
                cc = new ComponentContainer<T>();
                this._containerRegistries[CID] = cc;

                //T::RegisterDefaultUpdate<T>(*this);
            }
            else
                cc = (ComponentContainer<T>) _containerRegistries[CID];

            //TODO: find some assertion mode??
            //assert(cc != nullptr && "Failed to create ComponentContainer<Type>!");
            return cc;
        }

        /**
         * \brief Assigns the next free LUT index to this entity
         * \param component
         * \return
         * <param name="entityComponentLinker"></param>
         */
        int AssignIndexToEntity(EntityComponentLinker* entityComponentLinker)
        {
            int i = 0;
            if (LUTFragm)
            {
                i = LUTFragmFree.ElementAt(LUTFragmFree.Length);
                LUTFragmFree.RemoveAt(LUTFragmFree.Length);
                if (LUTFragmFree.IsEmpty)
                {
                    LUTFragm = false;
                }
            }
            else
            {
                i = LUTNextFree;
                LUTNextFree++;
                if (!(i < _entityLUT.Length))
                {
                    _entityLUT.ResizeUninitialized(_entityLUT.Capacity + ENTITY_LUT_GROW);
                    _entityLUT.Length = _entityLUT.Capacity + ENTITY_LUT_GROW;
                }
            }
            

            // increase component LUT size

            var ptr = NativeListUnsafeUtility.GetUnsafePtr(_entityLUT);
            
            _entityLUT[i] = (IntPtr)entityComponentLinker;
            return i;
        }
        
        /// <summary>
        /// Instantiates a new entity
        /// </summary>
        /// <returns>A new Entity reference</returns>
        public Entity CreateEntity()
        {
            EntityComponentLinker* pObjectMemory = (EntityComponentLinker*) GetComponentContainer<EntityComponentLinker>().CreateObject();
            
            int runtimeIndex = AssignIndexToEntity(pObjectMemory);
            
            Entity ent = new Entity()
            {
                _runtimeIndex = runtimeIndex,
                _UUID = nextUID,
                _woldID = _world.ID
            };

            pObjectMemory->entitiy = ent;
            pObjectMemory->components = new UnsafeList<IntPtr>(1, Allocator.Persistent);
            
            nextUID++;

            return ent;
        }
        
        public T AddComponent<T>(Entity entity) where T : unmanaged, IComponent
        {
            //The type ID of the thing we are trying to add
            var CTID = typeof(Entity).GUID;

            // acquire memory for new entity object of type Type
            void* pObjectMemory = GetComponentContainer<T>().CreateObject();

            ((EntityComponentLinker*)_entityLUT[entity._runtimeIndex])->components.Add((IntPtr) pObjectMemory);
            
            //Assign the index and the UID to the object
            /*int runtimeIndex = AssignIndexToEntity((Entity*) pObjectMemory);*/
            //((T*) pObjectMemory)->_runtimeIndex = runtimeIndex;
            //((T*) pObjectMemory)->_runtimeUID = nextUID;
            nextUID++;
            

            // create Entity in place
            //Entity* component = new(pObjectMemory)T(std::forward<ARGS>(args)...);

            //return new rtm_ptr((Entity*) pObjectMemory);
            return new T();
        }

        public IEnumerable<EntityComponentLinker> GetEntities()
        {
            return (IEnumerable<EntityComponentLinker>) _containerRegistries[typeof(EntityComponentLinker)];
        }

        public List<ComponentRef> GetComponentsForEntity(Entity entity)
        {
            EntityComponentLinker* links = (EntityComponentLinker*) _entityLUT[entity._runtimeIndex];

            var list = new List<ComponentRef>();
            
            for (int i = 0; i < links->components.length; i++)
            {
                list.Add(new ComponentRef((Component*) links->components[i]));
            }
            
            return list;
        }
        

        ~EntityManager()
        {
            Dispose(false);
        }
        
        private void ReleaseUnmanagedResources()
        {
            // TODO release unmanaged resources here
        }

        private void Dispose(bool disposing)
        {
            ReleaseUnmanagedResources();
            if (disposing)
            {
                _entityLUT.Dispose();
                LUTFragmFree.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}