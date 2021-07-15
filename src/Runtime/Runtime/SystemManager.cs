using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace src.Runtime.Runtime
{
    public static class SystemManager
    {
        private static List<NECS.Runtime.System> systems = new List<NECS.Runtime.System>();

        public static void Add(NECS.Runtime.System system)
        {
            systems.Add(system);
        }

        public static void Update()
        {
            Debug.Log("Updating "+systems.Count+" systems");
            NativeArray<JobHandle> jobs = new NativeArray<JobHandle>(systems.Count, Allocator.Temp);

            for (var i = 0; i < systems.Count; i++)
            {
                var system = systems[i];
                SystemRunnerJob job = new SystemRunnerJob(system);
                jobs[i] = job.Schedule();
            }
            
            JobHandle.CompleteAll(jobs);

            jobs.Dispose();
        }
    }
}