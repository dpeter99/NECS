using NECS.Runtime;
using NUnit.Framework;
using src.Runtime;
using Unity.Collections;

namespace NECS.Tests
{
    
    public class MemoryChunkTests
    {
        public class TestThingy
        {
            public string name;
        }
        

        [Test]
        public void TestAllocator()
        {
            Allocator<TestThingy> allocator = new Allocator<TestThingy>();


            Allocator<TestThingy>.MemoryChunk chunk = new Allocator<TestThingy>.MemoryChunk();

            TestThingy a = new TestThingy();
            bool res = chunk.Alloc(ref a);

            Assert.IsTrue(res);
            
            a.name = "Test";
            
            //Assert.Equals(a.name)
        }
        
    }
}