using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Entities.Serialization;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = Unity.Mathematics.Random;

namespace Streaming.Complete
{
    public class TileInfoAuthoring : MonoBehaviour
    {
        public int randomSeed;
        public float tileSize;
        public int2 minBoundary;
        public int2 maxBoundary;
        public List<TileTemplate> tileTemplates;

        class Baker : Baker<TileInfoAuthoring>
        {
            public override void Bake(TileInfoAuthoring authoring)
            {
                List<TileTemplate> tileTemplates = new List<TileTemplate>(authoring.tileTemplates.Count);
                List<EntitySceneReference> sceneReferences =
                    new List<EntitySceneReference>(authoring.tileTemplates.Count);

                foreach (var tileTemplate in tileTemplates)
                {
                    if (tileTemplate != null)
                    {
                        DependsOn(tileTemplate.tileScene);

                        if (tileTemplate.tileScene != null)
                        {
                            tileTemplates.Add(tileTemplate);
                            sceneReferences.Add(new EntitySceneReference(tileTemplate.tileScene));
                        }
                    }
                }

                int2 min = math.min(authoring.minBoundary, authoring.maxBoundary);
                int2 max = math.max(authoring.minBoundary, authoring.maxBoundary);

                var random = new Random((uint)authoring.randomSeed);
                var tileSize = new float2(authoring.tileSize, authoring.tileSize);

                // Choose the tiles for the world from the patterns
                for (int x = min.x; x < max.x; x++)
                {
                    for (int y = min.y; y < max.y; y++)
                    {
                        var selectTile = random.NextInt(0, tileTemplates.Count);
                        var tileEntity = CreateAdditionalEntity(TransformUsageFlags.None, false, $"Tile {x}_{y}");

                        // Store the information to instantiate the scene into the right tile position
                        var loadingDistance = tileTemplates[selectTile].loadingDistance;
                        var unloadingDistance = tileTemplates[selectTile].unloadingDistance;

                        AddComponent(tileEntity, new TileInfo
                        {
                            Scene = sceneReferences[selectTile],
                            Position = tileSize * new float2(x, y),
                            Rotation = random.NextFloat(-2, 2) * (math.PI / 2f),
                            LoadingDistanceSq = loadingDistance * loadingDistance,
                            UnloadingDistanceSq = unloadingDistance * unloadingDistance
                        });
                        
                        // This component will store the distance to the Relevant entities
                        AddComponent<DistanceToRelevant>(tileEntity);
                    }
                }
            }
        }
    }


    [Serializable]
    public class TileTemplate
    {
        public SceneAsset tileScene; // the scene to instantiate
        public float loadingDistance; // proximity distance within which to consider loading the scene
        public float unloadingDistance; // proximity distance within which to consider unloading the scene
    }

    public struct TileInfo : IComponentData
    {
        public EntitySceneReference Scene;
        public float2 Position;
        public float Rotation;
        public float LoadingDistanceSq;
        public float UnloadingDistanceSq;
    }

    public struct DistanceToRelevant : IComponentData
    {
        public float DistanceSq;
    }
}