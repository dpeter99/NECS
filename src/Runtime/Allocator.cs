using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;


namespace NECS.Runtime
{
    public unsafe class Allocator<Type> where Type : unmanaged
    {
        /// <summary>
        /// This represents a single element of the memory chunks and is used for accessing the given element as either a pointer to the next free slot or as the Entity
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        protected struct Element
        {
            /// <summary>
            /// If the element is free this represents the next free in the free list
            /// </summary>
            public int next;

            public Type element;
        };

        /// <summary>
        /// The maximum number of Entities in a MemoryChunk
        /// </summary>
        /// This is basically the size of the memory array that gets allocated.
        private const int MAX_OBJECTS_IN_CHUNK = 2048;

        /// <summary>
        /// The size of a single Entity
        /// </summary>
        private static int ELEMENT_SIZE = UnsafeUtility.SizeOf(typeof(Type));

        /// <summary>
        /// The size of the Memoty Chunks in bytes
        /// </summary>
        private static int ALLOC_SIZE = ELEMENT_SIZE * MAX_OBJECTS_IN_CHUNK;

        /*
        /// <summary>
        /// The size of a single Entity
        /// </summary>
        public const int ELEMENT_SIZE = (sizeof(Element));

        /// <summary>
        /// The size of the Memoty Chunks in bytes
        /// </summary>
        public const int ALLOC_SIZE = ELEMENT_SIZE * MAX_OBJECTS_IN_CHUNK;
        */

        interface IMemoryChunk
        {
            public bool Alloc(ref Type data);
        }
        
        #region Temp
        
#if flase
        
        internal class MemoryChunk_ManualAlloc
        {
            void* chunkStart;
            void* chunkEnd;

            int count;
            const bool FreeFlag = true;
            bool[] metadata = new bool[MAX_OBJECTS_IN_CHUNK];

            //Points to the next free element in the pool
            void* nextFree;

            MemoryChunk_ManualAlloc()
            {
                chunkStart = UnsafeUtility.Malloc(ALLOC_SIZE,0,Allocator.Persistent);

                //memset(chunkStart, -1, ALLOC_SIZE);

                chunkEnd = &chunkStart[MAX_OBJECTS_IN_CHUNK];

                for (int i = 1; i < MAX_OBJECTS_IN_CHUNK; i++)
                {
                    var e = UnsafeUtility.ReadArrayElement<Element>(chunkStart, i - 1);
                        e.next = UnsafeUtility.AddressOf<>(ref chunkStart[i]);
                    UnsafeUtility.WriteArrayElement(chunkStart,i-1,e);
                    metadata[i] = FreeFlag;
                }
                
                UnsafeUtility.WriteArrayElement(chunkStart,MAX_OBJECTS_IN_CHUNK - 1,new Element(){next = null});
                
                //chunkStart[MAX_OBJECTS_IN_CHUNK - 1]->next = IntPtr.Zero;
                nextFree = chunkStart;
            }

            void* allocate_ptr()
            {
                if (nextFree == null)
                    return null;
                count++;
                var res = nextFree;
                nextFree = UnsafeUtility.AsRef<Element>(nextFree).next;

                int i = ((int)res - (int)chunkStart);
                metadata[i] = !FreeFlag;

                return res;
            }
            
            Type allocate()
            {
                var ptr = allocate_ptr();

                
                
                return UnsafeUtility.AsRef<Element>(ptr).element;
            }

            void free(void* ptr)
            {
                /*
                count--;
                auto element = ((Element*)ptr);
                element->next = nextFree;
                nextFree = element;

                int i = ((Element*)ptr - (Element*)chunkStart);
                metadata[i] = FreeFlag;
                */
            }

        }
        
#endif
        #endregion

        public class MemoryChunk : IMemoryChunk
        {
            private Element[] _data = new Element[MAX_OBJECTS_IN_CHUNK];

            private int _nextFree = 0;
            private bool[] _freeTable = new bool[MAX_OBJECTS_IN_CHUNK];
            
            public MemoryChunk()
            {
                
                for (int i = 1; i < MAX_OBJECTS_IN_CHUNK; i++)
                {
                    _data[i - 1] = new Element() {next = i};
                    _freeTable[i - 1] = true;
                }

                _data[MAX_OBJECTS_IN_CHUNK - 1] = new Element() {next = -1};

            }
            
            public bool Alloc(ref Type data)
            {
                if (_nextFree == -1)
                {
                    data = default;
                    return false;
                }
                
                _freeTable[_nextFree] = false;

                data = ref _data[_nextFree].element;

                _nextFree = _data[_nextFree].next;
                
                return true;
            }
            
            void Free(ref Type data)
            {
                /*
                var ptr = ((IntPtr)UnsafeUtility.AddressOf(ref data));
                var chunkStart = ((IntPtr)UnsafeUtility.AddressOf(ref _data[0]));
                
                int i = ((int)ptr - (int)chunkStart);
                
                var element = i;
                _data[i].next = _nextFree;
                _nextFree = element;
                
                _freeTable[i] = true;
                */
            }
        }
        
        
        private List<MemoryChunk> _chunks;

        
        

        
    }
}