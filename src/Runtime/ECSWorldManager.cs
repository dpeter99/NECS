using System.Collections.Generic;
using com.dpeter99.utils.Basic;
using NECS;

namespace src.Runtime
{
    public class ECSWorldManager : Singleton<ECSWorldManager>
    {
        private List<EcsWorld> _allWorlds = new List<EcsWorld>();
        
        public List<EcsWorld> AllWorlds
        {
            get => _allWorlds;
            set => _allWorlds = value;
        }

        private bool _worldDirty;
        private long _nextWorldID = 0;
        public bool worldDirty => _worldDirty;
        
        public void AddWorld(EcsWorld world)
        {
            _allWorlds.Add(world);
            _worldDirty = true;
        }

        public void Remove(EcsWorld world)
        {
            _allWorlds.Remove(world);
            _worldDirty = true;
        }

        public void ClearDirty()
        {
            _worldDirty = false;
        }

        public long GetNewWorldID()
        {
            return _nextWorldID++;
        }
    }
}