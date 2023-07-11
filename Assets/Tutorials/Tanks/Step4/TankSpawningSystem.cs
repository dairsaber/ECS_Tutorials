using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Tutorials.Tanks.Step4
{
    public partial struct TankSpawningSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Config>();
            state.RequireForUpdate<Execute.TankSpawning>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;

            var config = SystemAPI.GetSingleton<Config>();
            var random = new Random(123);
            var query = SystemAPI.QueryBuilder().WithAll<URPMaterialPropertyBaseColor>().Build();

            // An EntityQueryMask provides an efficient test of whether a specific entity would
            // be selected by an EntityQuery.
            var queryMask = query.GetEntityQueryMask();

            var ecb = new EntityCommandBuffer(Allocator.Temp);
            var tanks = new NativeArray<Entity>(config.TankCount, Allocator.Temp);
            ecb.Instantiate(config.TankPrefab, tanks);

            foreach (var tank in tanks)
            {
                // Every root entity instantiated from a prefab has a LinkedEntityGroup component, which
                // is a list of all the entities that make up the prefab hierarchy.
                
                // 因为这边要设置颜色 所以这边拿到了query 的掩码也就是带有URPMaterialPropertyBaseColor的查询掩码 不符合这个的实体都被过滤掉不会被加上颜色component
                // 相较于SetComponent这个SetComponentForLinkedEntityGroup多了一层根据query过滤的功能.
                // 如果tank身上没有URPMaterialPropertyBaseColor这个主键的话则不会被加上颜色
                // tank 及其子有这个URPMaterialPropertyBaseColor的都会被设置颜色
                ecb.SetComponentForLinkedEntityGroup(tank, queryMask,
                    new URPMaterialPropertyBaseColor { Value = RandomColor(ref random) });
            }
           
            // 自定义的ecb这边必须要手动Playback一下
            // 系统自带的创建的则不需要
            ecb.Playback(state.EntityManager);
        }

        // 随机颜色
        static float4 RandomColor(ref Random random)
        {
            // 0.618034005f is inverse of the golden ratio
            var hue = (random.NextFloat() + 0.618034005f) % 1;
            return (Vector4)Color.HSVToRGB(hue, 1.0f, 1.0f);
        }
    }
}