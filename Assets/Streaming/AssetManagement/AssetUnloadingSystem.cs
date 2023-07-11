using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Content;
using Unity.Scenes;
using UnityEngine;

namespace Streaming.AssetManagement
{
    public partial struct AssetUnloadingSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var query = SystemAPI.QueryBuilder().WithAll<References, Loading, RequestUnload>().Build();
            var referencesArray = query.ToComponentDataArray<References>(Allocator.Temp);
            // 托管类型的用ToComponent比较好
            var loadingArray = query.ToComponentDataArray<Loading>();


            for (int i = 0; i < referencesArray.Length; i++)
            {
                var refs = referencesArray[i];
                var loading = loadingArray[i];


                // Unload Entity Scene
                if (loading.EntityScene != Entity.Null)
                {
                    SceneSystem.UnloadScene(state.WorldUnmanaged, loading.EntityScene,
                        SceneSystem.UnloadParameters.DestroyMetaEntities);
                }


                // Unload Entity Prefab
                if (loading.EntityPrefabInstance != Entity.Null)
                {
                    state.EntityManager.DestroyEntity(loading.EntityPrefabInstance);
                }

                if (loading.EntityPrefab != Entity.Null)
                {
                    state.EntityManager.DestroyEntity(loading.EntityPrefab);
                }

                // Unload GameObject Scene
                if (loading.GameObjectScene.IsValid())
                {
                    refs.GameObjectSceneReference.Unload(ref loading.GameObjectScene);
                }

                // Unload GameObject Prefab
                if (loading.GameObjectPrefabInstance != null)
                {
                    Object.Destroy(loading.GameObjectPrefabInstance);
                }

                if (refs.GameObjectPrefabRefernce.LoadingStatus != ObjectLoadingStatus.None)
                {
                    refs.GameObjectPrefabRefernce.Release();
                }

                // Unload Mesh
                if (loading.MeshGameObjectInstance)
                {
                    var renderer = loading.MeshGameObjectInstance.GetComponent<MeshRenderer>();
                    var material = renderer.sharedMaterial;
                    Object.Destroy(material);
                    Object.Destroy(loading.MeshGameObjectInstance);
                }

                if (refs.MeshReference.LoadingStatus != ObjectLoadingStatus.None)
                {
                    refs.MeshReference.Release();
                }

                // Unload Material
                if (loading.MaterialGameObjectInstance)
                {
                    Object.Destroy(loading.MaterialGameObjectInstance);
                }

                if (refs.MaterialReference.LoadingStatus != ObjectLoadingStatus.None)
                {
                    refs.MaterialReference.Release();
                }


                // Unload Texture
                if (loading.TextureGameObjectInstance)
                {
                    var renderer = loading.TextureGameObjectInstance.GetComponent<MeshRenderer>();
                    var material = renderer.sharedMaterial;
                    Object.Destroy(material);
                    Object.Destroy(loading.TextureGameObjectInstance);
                }

                if (refs.TextureReference.LoadingStatus != ObjectLoadingStatus.None)
                {
                    refs.TextureReference.Release();
                }
            }
            // 上面有structural change 所以这边重新查询一遍,不能用上面的query
            // Remove Loading
            var loadingStateQuery = SystemAPI.QueryBuilder().WithAll<References, Loading, RequestUnload>().Build();
            state.EntityManager.RemoveComponent<Loading>(loadingStateQuery);
        }
    }
}