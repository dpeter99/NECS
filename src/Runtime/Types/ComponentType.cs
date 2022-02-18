using System;
using System.Collections.Generic;
using System.Threading;

namespace src.Runtime.Types
{
    public class EcsComponentType {
        // ReSharper disable StaticMemberInGenericType
        public static readonly int TypeIndex;
        public static readonly Type Type;
        // ReSharper restore StaticMemberInGenericType
    }
    
    /// <summary>
    /// Global descriptor of used component type.
    /// </summary>
    /// <typeparam name="T">Component type.</typeparam>
    public class EcsComponentType<T> : EcsComponentType where T : unmanaged {
        // ReSharper disable StaticMemberInGenericType
        public static readonly int TypeIndex;
        public static readonly Type Type;
        // ReSharper restore StaticMemberInGenericType

        static EcsComponentType () {
            TypeIndex = Interlocked.Increment (ref EcsComponentTypePool.ComponentTypesCount);
            Type = typeof (T);

        }
    }

    public sealed class EcsComponentTypePool {
        /// <summary>
        /// Global component type counter.
        /// First component will be "1" for correct filters updating (add component on positive and remove on negative).
        /// </summary>
        internal static int ComponentTypesCount;

        internal List<EcsComponentType> _types = new();

    }
}

//This files contains code form the following repo: https://github.com/Leopotam/ecs
//These code segments are used under the MIT license.
