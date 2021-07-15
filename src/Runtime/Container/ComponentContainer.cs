using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace NECS.Runtime
{
    public unsafe class ComponentContainer<Type> : IComponentContainer, IEnumerable<Type>, IDisposable where Type : unmanaged
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
        private const int MAX_OBJECTS_IN_CHUNK = 32;

        /// <summary>
        /// The size of a single Entity
        /// </summary>
        private static int ELEMENT_SIZE = sizeof(Element);

        /// <summary>
        /// The size of the Memoty Chunks in bytes
        /// </summary>
        private static int ALLOC_SIZE = ELEMENT_SIZE * MAX_OBJECTS_IN_CHUNK;

        interface IMemoryChunk
        {
            public bool Alloc(ref Type data);
        }
        
        public class MemoryChunk: IDisposable
        {
            internal Element* chunkStart;
            internal Element* chunkEnd;

            internal int count;
            const bool FreeFlag = true;
            internal bool[] metadata = new bool[MAX_OBJECTS_IN_CHUNK];

            //Points to the next free element in the pool
            Element* nextFree;

            public MemoryChunk()
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

                return (&res->element);
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

            ~MemoryChunk()
            {
                ReleaseUnmanagedResources();
            }

            private void ReleaseUnmanagedResources()
            {
                UnsafeUtility.Free(chunkStart,Allocator.Persistent);
            }

            public void Dispose()
            {
                ReleaseUnmanagedResources();
                GC.SuppressFinalize(this);
            }
        }

        public class Iterator : IEnumerator<Type>
        {
            private List<MemoryChunk>.Enumerator _currentChunk;

            Element* _currentElement;

            private int index = 0;
            
            public Iterator(List<MemoryChunk>.Enumerator begin)
            {
                _currentChunk = begin;

                //_currentChunk.MoveNext();
                //_currentElement = (_currentChunk.Current).chunkStart;


            }
            
            public bool MoveNext()
            {
                if (_currentChunk.Current == null)
                {
                    _currentChunk.MoveNext();
                    _currentElement = (_currentChunk.Current).chunkStart;
                    return true;
                }
                
                // move to next object in current chunk
                _currentElement = &_currentElement[1];
                index++;
                
                while (index < MAX_OBJECTS_IN_CHUNK && ((_currentChunk.Current).metadata[index] == true)) {

                    _currentElement = &_currentElement[1];
                    index++;
                }
                
                // if we reached end of list, move to next chunk
                if (_currentElement == (_currentChunk.Current).chunkEnd)
                {
                    index = 0;
                    if (_currentChunk.MoveNext())
                    {
                        // set object iterator to begin of next chunk list
                        //assert((*m_CurrentChunk) != nullptr);
                        _currentElement = (_currentChunk.Current).chunkStart;
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            }

            public void Reset()
            {
                
            }

            public Type Current => _currentElement->element;

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                
            }
        }
        
        private List<MemoryChunk> _chunks = new List<MemoryChunk>();


        public void* CreateObject()
        {
            void* slot = null;

            // get next free slot
            foreach (var chunk in _chunks)
            {
                if (chunk.count > MAX_OBJECTS_IN_CHUNK)
                    continue;

                slot = chunk.allocate();
                if (slot != null)
                {
                    //chunk->objects.push_back((OBJECT_TYPE*)slot);
                    break;
                }
            }

            // all chunks are full... allocate a new one
            if (slot == null)
            {
                //Allocator* allocator = new Allocator(ALLOC_SIZE, Allocate(ALLOC_SIZE, this->m_AllocatorTag), sizeof(OBJECT_TYPE), alignof(OBJECT_TYPE));
                var newChunk = new MemoryChunk();

                // put new chunk in front
                _chunks.Add(newChunk);

                slot = newChunk.allocate();

                //TODO: Ammm this is for later for sure... (I hope)
                //assert(slot != nullptr && "Unable to create new object. Out of memory?!");
                
                //newChunk->objects.clear();
                //newChunk->objects.push_back((OBJECT_TYPE*)slot);
            }

            return slot;
        }

        public void DestroyObject(void* o)
        {
            throw new NotImplementedException();
        }

        IEnumerator<Type> IEnumerable<Type>.GetEnumerator()
        {
            return new Iterator(_chunks.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Iterator(_chunks.GetEnumerator());
        }

        public void Dispose()
        {

            for (int i = 0; i < _chunks.Count; i++)
            {
                _chunks[i].Dispose();
            }
        }
    }
}