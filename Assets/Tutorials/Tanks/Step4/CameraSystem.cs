using Tutorials.Tanks.Step3;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Tutorials.Tanks.Step4
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial struct CameraSystem : ISystem
    {
        private Entity target;
        private Random random;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Execute.Camera>();
            random = new Random(123);
        }


        // Because this OnUpdate accesses managed objects, it cannot be Burst-compiled.
        // [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (target == Entity.Null || Input.GetKeyDown(KeyCode.Space))
            {
                var tankQuery = SystemAPI.QueryBuilder().WithAll<Tank>().Build();

                var tanks = tankQuery.ToEntityArray(Allocator.Temp);

                if (tanks.Length == 0) return;

                target = tanks[random.NextInt(tanks.Length)];
            }
            
            var cameraTransform = CameraSingleton.Instance.transform;
            var tankTransform = SystemAPI.GetComponent<LocalToWorld>(target);

            var position = tankTransform.Position;
            position -= 10.0f * tankTransform.Forward; // move the camera back from the tank
            position += new float3(0, 5f, 0); // raise the camera by an offset
            cameraTransform.position = position;

            cameraTransform.LookAt(tankTransform.Position);
        }
    }
}