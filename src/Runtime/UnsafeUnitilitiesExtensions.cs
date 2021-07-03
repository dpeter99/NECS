using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace src.Runtime
{
    
    public static class UnsafeUnitilitiesExtensions
    {
        /*

        public unsafe IntPtr GetArrayPointer<T>(void* array, int index) where T : struct
        {
            return ((IntPtr) array + (int)((long) index * (long) UnsafeUtility.SizeOf(typeof(T))));
        }
        
        
        public static unsafe ref T ArrayElementAsRef<T>(void* ptr, int index) where T : struct => (T&) ((IntPtr) ptr + (IntPtr) ((long) index * (long) sizeof (T)));
        */
    }
}