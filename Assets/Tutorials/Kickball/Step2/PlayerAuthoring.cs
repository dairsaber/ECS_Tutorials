using System;
using Unity.Entities;
using UnityEngine;

namespace Tutorials.Kickball.Step2
{
    public class PlayerAuthoring : MonoBehaviour
    {
        class Baker : Baker<PlayerAuthoring>
        {
            public override void Bake(PlayerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent<Player>(entity);
                AddComponent<Carry>(entity);
                SetComponentEnabled<Carry>(entity,false);
            }
        }
    }
    
    
    public struct Player:IComponentData{}

    public struct Carry : IComponentData, IEnableableComponent
    {
        public Entity Target;
    }
}