using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NECS;
using NECS.Runtime;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace NECS.Runtime
{
    public unsafe class EntityManager
    {
        /// <summary>
        /// A list of allocators mapped to the type they are responsible for.
        /// </summary>
        internal Dictionary<Type, IComponentContainer> _containerRegistries = new Dictionary<Type, IComponentContainer>();


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
		 */
        int AssignIndexToEntity(Entity* component)
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

            /*
            for (; i < this->m_EntityLUT.size(); ++i)
            {
                if (this->m_EntityLUT[i] == nullptr)
                {
                    this->m_EntityLUT[i] = component;
                    return i;
                }
            }
            */

            // increase component LUT size

            var ptr = NativeListUnsafeUtility.GetUnsafePtr(_entityLUT);
            
            _entityLUT[i] = (IntPtr)component;
            return i;
        }
        
        
        /**
		 * \brief Instantiates a new entity
		 * \tparam T Type of the Entity
		 * \tparam ARGS Constructor parameters of the Entity
		 * \param args Constructor parameters of the Entity
		 * \return
		 */
        public rtm_ptr CreateEntity()
        {
            //The type ID of the thing we are trying to add
            var CTID = typeof(Entity);

            // acquire memory for new entity object of type Type
            void* pObjectMemory = GetComponentContainer<Entity>().CreateObject();

            //Assign the index and the UID to the object
            int runtimeIndex = AssignIndexToEntity((Entity*) pObjectMemory);
            ((Entity*) pObjectMemory)->_runtimeIndex = runtimeIndex;
            ((Entity*) pObjectMemory)->_runtimeUID = nextUID;
            nextUID++;

            // create Entity in place
            //Entity* component = new(pObjectMemory)T(std::forward<ARGS>(args)...);

            return new rtm_ptr((Entity*) pObjectMemory);
        }

        public IEnumerable<Entity> GetEntities()
        {
            return (IEnumerable<Entity>) _containerRegistries[typeof(Entity)];
        }
        
        ~EntityManager()
        {
            _entityLUT.Dispose();
            LUTFragmFree.Dispose();
        }
    }

    public unsafe class rtm_ptr
    {
        Entity* _ptr;

        long _uid;
        
        public rtm_ptr(Entity* ptr)
        {
            _ptr = ptr;
            _uid = _ptr->_runtimeUID;
        }
    }
}