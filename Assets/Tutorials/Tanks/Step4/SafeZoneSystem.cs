using Tutorials.Tanks.Step2;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Tutorials.Tanks.Step4
{
    // Prevent the tanks from firing when within a "safe zone" circle area around the origin.

    // 因为这边在安全区域内要阻止发射 则需要再shootingSystem之前
    [UpdateBefore(typeof(TurretShootingSystem))]
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial struct SafeZoneSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Config>();
            state.RequireForUpdate<Execute.SafeZone>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var radius = SystemAPI.GetSingleton<Config>().SafeZoneRadius;
            // Debug rendering (the white circle).
            const float debugRenderStepInDegree = 20;

            for (float angle = 0; angle < 360; angle += debugRenderStepInDegree)
            {
                var a = float3.zero;
                var b = float3.zero;
                math.sincos(math.radians(angle), out a.x, out a.z);
                math.sincos(math.radians(angle + debugRenderStepInDegree), out b.x, out b.z);
                Debug.DrawLine(a * radius, b * radius);
            }
            // Debug rendering (the white circle) End

            var safeZoneJob = new SafeZoneJob
            {
                SquareRadius = radius * radius
            };
            safeZoneJob.ScheduleParallel();
        }
    }


    [WithAll(typeof(Turret))]
    [WithOptions(EntityQueryOptions.IgnoreComponentEnabledState)]
    [BurstCompile]
    public partial struct SafeZoneJob : IJobEntity
    {
        public float SquareRadius;

        void Execute(in LocalToWorld transformMatrix, EnabledRefRW<Shooting> shootingState)
        {
            shootingState.ValueRW = math.lengthsq(transformMatrix.Position) > SquareRadius;
        }
    }
}