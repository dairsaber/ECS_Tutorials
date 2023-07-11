using Unity.Entities;
using UnityEngine;

namespace Tutorials.Tanks.Step2
{
    public class TurretAuthoring : MonoBehaviour
    {
        // Bakers convert authoring MonoBehaviours into entities and components.
        public GameObject CannonBallPrefab;
        public Transform CannonBallSpawn;  
        
        
         class Baker:Baker<TurretAuthoring>
         {
             public override void Bake(TurretAuthoring authoring)
             {
                 var entity = GetEntity(TransformUsageFlags.Dynamic);
                 AddComponent(entity, new Turret
                 {
                     CannonBallPrefab = GetEntity(authoring.CannonBallPrefab,TransformUsageFlags.Dynamic),
                     CannonBallSpawn = GetEntity(authoring.CannonBallSpawn,TransformUsageFlags.Dynamic)
                 });
                 
                 AddComponent(entity,new Shooting());
             }
         }
    }

    public struct Turret : IComponentData
    {
        public Entity CannonBallPrefab;
        public Entity CannonBallSpawn;
    }

    // This component will be used in Step 8.
    // This is a tag component that is also an "enableable component".
    // Such components can be toggled on and off without removing the component from the entity,
    // which would be less efficient and wouldn't retain the component's value.
    // An Enableable component is initially enabled.
    public struct Shooting : IComponentData, IEnableableComponent
    {
        
    }
}
