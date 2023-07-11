using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Tutorials.Tanks.Step2
{
    public partial struct TurretRotationSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Execute.TurretRotation>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var spin = quaternion.RotateY(SystemAPI.Time.DeltaTime * math.PI);

            foreach (var transform in SystemAPI.Query<RefRW<LocalTransform>>().WithAll<Turret>())
            {
                // 这边了解一下四元数矩阵的乘法,两个四元素相乘还是四元数,且相乘的顺序不能调换 Q1*Q2 代表先应用Q2再应用Q1;
                transform.ValueRW.Rotation = math.mul(spin, transform.ValueRO.Rotation);
            }

        }
    }
}