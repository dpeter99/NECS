using Unity.Jobs;
using UnityEngine;

namespace src.Runtime.Runtime
{
    public struct SystemRunnerJob : IJob
    {
        private readonly NECS.Runtime.System _system;

        public SystemRunnerJob(NECS.Runtime.System system)
        {
            _system = system;
        }
        
        public void Execute()
        {
            
        }
    }
}