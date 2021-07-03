using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;


[assembly: InternalsVisibleTo("NECS.Tests")]
namespace NECS.Runtime
{
    public unsafe class Allocator<Type> where Type : unmanaged
    {
        /// <summary>
        /// This represents a single element of the memory chunks and is used for accessing the given element as either a pointer to the next free slot or as the Entity
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct Element
        {
            /// <summary>
            /// If the element is free this represents the next free in the free list
            /// </summary>
            public Element* next;

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
        private static int ELEMENT_SIZE = sizeof(Element);

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
        
        public class MemoryChunk_ManualAlloc
        {
            internal Element* chunkStart;
            Element* chunkEnd;

            int count;
            const bool FreeFlag = true;
            bool[] metadata = new bool[MAX_OBJECTS_IN_CHUNK];

            //Points to the next free element in the pool
            Element* nextFree;

            public MemoryChunk_ManualAlloc()
            {
                chunkStart = (Element*)UnsafeUtility.Malloc(ALLOC_SIZE,0,Allocator.Persistent);

                //memset(chunkStart, -1, ALLOC_SIZE);

                chunkEnd = &chunkStart[MAX_OBJECTS_IN_CHUNK];

                for (int i = 1; i < MAX_OBJECTS_IN_CHUNK; i++)
                {
                    chunkStart[i - 1].next = &chunkStart[i];
                    metadata[i] = FreeFlag;
                }
                
                chunkStart[MAX_OBJECTS_IN_CHUNK - 1].next = null;
                nextFree = chunkStart;
            }

            public Type* allocate()
            {
                if (nextFree == null)
                    return null;
                count++;
                var res = nextFree;
                nextFree = nextFree->next;

                long i = ((Element*)res - (Element*)chunkStart);
                metadata[i] = !FreeFlag;

                return (Type*)res;
            }

            void free(void* ptr)
            {
                count--;
                var element = ((Element*)ptr);
                element->next = nextFree;
                nextFree = element;

                long i = ((Element*)ptr - (Element*)chunkStart);
                metadata[i] = FreeFlag;
            }

        }
        
        #endregion

        /*
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
                
                var ptr = ((IntPtr)UnsafeUtility.AddressOf(ref data));
                var chunkStart = ((IntPtr)UnsafeUtility.AddressOf(ref _data[0]));
                
                int i = ((int)ptr - (int)chunkStart);
                
                var element = i;
                _data[i].next = _nextFree;
                _nextFree = element;
                
                _freeTable[i] = true;
                
            }
        }
        */
        
        
        private List<MemoryChunk_ManualAlloc> _chunks;

        
        

        
    }
}