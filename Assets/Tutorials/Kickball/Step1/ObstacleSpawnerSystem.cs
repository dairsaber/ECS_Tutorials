using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Tutorials.Kickball.Step1
{
    public partial struct ObstacleSpawnerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Config>();
            state.RequireForUpdate<Common.ObstacleSpawner>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;
            var config = SystemAPI.GetSingleton<Config>();

            var rand = new Random(123);
            var scale = config.ObstacleRadius * 2;

            for (int column = 0; column < config.NumColumns; column++)
            {
                for (int row = 0; row < config.NumRows; row++)
                {
                    var obstacle = state.EntityManager.Instantiate(config.ObstaclePrefab);

                    state.EntityManager.SetComponentData(obstacle, new LocalTransform
                    {
                        Position = new float3
                        {
                            x = (column * config.ObstacleGridCellSize) + rand.NextFloat(config.ObstacleOffset),
                            y = 0,
                            z = (row * config.ObstacleGridCellSize) + rand.NextFloat(config.ObstacleOffset),
                        },
                        Scale = scale,
                        Rotation = quaternion.identity
                    });
                }
            }
        }
    }
}