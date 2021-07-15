using NECS.Runtime;

namespace src.Runtime
{
    public class ComponentRef
    {
        public unsafe Component* data;

        public ref Component GetData()
        {
            unsafe
            {
                return ref (*data);
            }
        }

        public unsafe ComponentRef(Component* data)
        {
            this.data = data;
        }
        
        
    }
}