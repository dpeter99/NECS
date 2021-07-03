using NECS.Runtime;
using NUnit.Framework;
using src.Runtime;
using Unity.Collections;


namespace NECS.Tests
{
    
    public unsafe class MemoryChunkTests
    {
        public struct TestThingy
        {
            public char name;
        }
        

        [Test]
        public void TestAllocator()
        {
            Allocator<TestThingy> allocator = new Allocator<TestThingy>();


            Allocator<TestThingy>.MemoryChunk_ManualAlloc chunk = new Allocator<TestThingy>.MemoryChunk_ManualAlloc();
            
            var res = chunk.allocate();

            Assert.False(res == null);
            
            res->name = 'T';

            Assert.AreEqual(res->name, 'T');
            Assert.AreEqual(res->name, chunk.chunkStart->element.name);
        }
        
    }
}