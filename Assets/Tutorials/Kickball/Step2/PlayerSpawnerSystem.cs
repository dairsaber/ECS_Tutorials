using Tutorials.Kickball.Step1;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Tutorials.Kickball.Step2
{
    [UpdateBefore(typeof(TransformSystemGroup))]
    [UpdateAfter(typeof(ObstacleSpawnerSystem))]
    public partial struct PlayerSpawnerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Common.PlayerSpawner>();
            state.RequireForUpdate<Config>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;
            var config = SystemAPI.GetSingleton<Config>();

            foreach (var obstacleTransform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<Obstacle>())
            {
                var player = state.EntityManager.Instantiate(config.PlayerPrefab);

                state.EntityManager.SetComponentData(player, new LocalTransform
                {
                    Position = new float3
                    {
                        x = obstacleTransform.ValueRO.Position.x + config.PlayerOffset,
                        y = 1,
                        z = obstacleTransform.ValueRO.Position.z + config.PlayerOffset
                    },
                    Scale = 1,
                    Rotation = quaternion.identity
                });
            }
        }
    }
}