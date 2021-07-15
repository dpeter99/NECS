using System;
using NECS.Runtime;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;


namespace src.Runtime.Types
{
    public unsafe struct EntityComponentLinker
    {
        internal Entity entitiy;

        internal UnsafeList<IntPtr> components;
    }

    public struct ComponentLink
    {
        
    }
}