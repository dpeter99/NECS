namespace NECS.Runtime
{
    public unsafe interface IComponentContainer
    {
        void* CreateObject();
        
        void DestroyObject(void* o);
    }
}