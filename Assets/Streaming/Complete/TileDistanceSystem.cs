using Streaming.Common;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Streaming.Complete
{
    public partial struct TileDistanceSystem : ISystem
    {
        private EntityQuery tilesQuery;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            tilesQuery = SystemAPI.QueryBuilder().WithAll<TileInfo, DistanceToRelevant>().Build();
            state.RequireForUpdate(tilesQuery);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var tiles = tilesQuery.ToComponentDataArray<TileInfo>(Allocator.Temp);
            var distanceSq = new NativeArray<float>(tiles.Length, Allocator.Temp);
            
            // Calculate the distance from the tile to the closest Relevant entity
            {
                for (int index = 0; index < tiles.Length; index++)
                {
                    distanceSq[index] = float.MaxValue;
                }

                foreach (var transform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<RelevantEntity>())
                {
                    for (int i = 0; i < tiles.Length; i++)
                    {
                        var pos = new float3(tiles[i].Position.x, 0f, tiles[i].Position.y);
                        var distance = transform.ValueRO.Position - pos;
                        distance.y = 0;
                        // 这边relevantEntity可能有多个取距离最小的
                        distanceSq[i] = math.min(math.lengthsq(distance), distanceSq[i]);
                    }
                }
            }
            
            // Copy the distances 下面这句操作不是很懂
            // Reinterpret 将distanceSq数组重新诠释为DistanceToRelevant的数组
            // CopyFromComponentDataArray 将数组中的数据再存储回根这个类型相关的实体中的Component中去 这边就是 DistanceToRelevant啦
            // 哎搞这么复杂的API干嘛
            tilesQuery.CopyFromComponentDataArray(distanceSq.Reinterpret<DistanceToRelevant>());
        }
    }
}