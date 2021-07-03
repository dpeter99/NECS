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
            ComponentContainer<TestThingy> componentContainer = new ComponentContainer<TestThingy>();


            ComponentContainer<TestThingy>.MemoryChunk chunk = new ComponentContainer<TestThingy>.MemoryChunk();
            
            var res = chunk.allocate();

            Assert.False(res == null);
            
            res->name = 'T';

            Assert.AreEqual(res->name, 'T');
            Assert.AreEqual(res->name, chunk.chunkStart->element.name);
            
        }
        
    }
}