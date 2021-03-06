using src.Runtime.Types;

namespace NECS.Runtime
{
    public unsafe struct Entity
    {
        /// <summary>
        /// This is the Globally unique ID of this Entity
        /// </summary>
        /// This ID will be only assigned to this Entity instance
        /// It can be used to look up entities, but it is not recommended as it is a slow process
        /// For Entity Lookup use the _runtimeIndex
        internal long _UUID;

        public long Uuid => _UUID;
        
        /// <summary>
        /// This is the UID that represents an invalid ID
        /// </summary>
        private const int INVALID_UID = -1;

        
        /// <summary>
        /// This is the ID of the entity usable for lookup
        /// </summary>
        /// This ID will be reused when the Entity is removed
        /// 
        /// To use this Entity for lookup use the LUT in the EntityManager
        internal int _runtimeIndex;


        internal long _woldID;
    }
}