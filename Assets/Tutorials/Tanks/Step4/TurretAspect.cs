using Tutorials.Tanks.Step2;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

namespace Tutorials.Tanks.Step4
{
    public readonly partial struct TurretAspect : IAspect
    {
        private readonly RefRO<Turret> m_Turret;
        private readonly RefRO<URPMaterialPropertyBaseColor> m_BaseColor;

        public Entity CannonBallSpawn => m_Turret.ValueRO.CannonBallSpawn;
        public Entity CannonBallPrefb => m_Turret.ValueRO.CannonBallPrefab;
        public float4 Color => m_BaseColor.ValueRO.Value;
    }
}