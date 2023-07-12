using Streaming.Common;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Streaming.Complete
{
    public class PlayerAuthoring : MonoBehaviour
    {
        public float MovementSpeedMetersPerSecond = 5.0f;
        public Vector3 CameraOffset;

        class Baker : Baker<PlayerAuthoring>
        {
            public override void Bake(PlayerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity,new Player
                {
                    Speed = authoring.MovementSpeedMetersPerSecond,
                    CameraOffset = authoring.CameraOffset
                });
                
                // Sections near the relevant entities are loaded at the highest LOD.
                AddComponent<RelevantEntity>(entity);
            }
        }
    }

    public struct Player : IComponentData
    {
        public float Speed;
        public float3 CameraOffset;
    }
}