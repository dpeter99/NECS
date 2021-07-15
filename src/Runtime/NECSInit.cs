using System;
using NECS;
using src.Runtime.Runtime;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
using UnityEngine.SubsystemsImplementation;
using System = NECS.Runtime.System;

namespace src.Runtime
{
    public class NECSInit
    {

        
        
        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        [RuntimeInitializeOnLoadMethod]
        public static void Init()
        {
            //SubsystemDescriptorWithProvider desc = new SubsystemDescriptorWithProvider<NECSSystem, SubsystemProvider>();
            
            Debug.Log("Init NECS");

            ECSManager.Init();
            
            SystemManager.Add(new NECS.Runtime.System());
            
            SetPlayerLoop();

            var GO = new GameObject("NECS_Runner");

        }
        
        public static void SetPlayerLoop()
        {
            var playerLoop = PlayerLoop.GetCurrentPlayerLoop();
            
            PlayerLoopSystem system = new PlayerLoopSystem()
            {
                type = typeof(NECSInit),
                updateDelegate = Run,
            };
            
            com.dpeter99.utils.PlayerLoopHelpers.AppendSystemToPlayerLoopList(system,ref playerLoop,typeof(Update));
            
            PlayerLoop.SetPlayerLoop(playerLoop);
        }


        public static void Run()
        {
            SystemManager.Update();
            
            
        }
    }
    
}