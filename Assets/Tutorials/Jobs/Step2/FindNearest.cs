using System;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Tutorials.Jobs.Step2
{
    public class FindNearest : MonoBehaviour
    {
        // The size of our arrays does not need to vary, so rather than create
        // new arrays every field, we'll create the arrays in Awake() and store them
        // in these fields.
        private NativeArray<float3> TargetPositions;
        private NativeArray<float3> SeekerPositions;
        private NativeArray<float3> NearestTargetPositions;


        private void Start()
        {
            var spawner = FindObjectOfType<Spawner>();
            // We use the Persistent allocator because these arrays must
            // exist for the run of the program.
            TargetPositions = new NativeArray<float3>(spawner.NumTargets, Allocator.Persistent);
            SeekerPositions = new NativeArray<float3>(spawner.NumSeekers, Allocator.Persistent);
            NearestTargetPositions = new NativeArray<float3>(spawner.NumSeekers, Allocator.Persistent);
        }
        
        // We are responsible for disposing of our allocations
        // when we no longer need them.
        public void OnDestroy()
        {
            TargetPositions.Dispose();
            SeekerPositions.Dispose();
            NearestTargetPositions.Dispose();
        }

        private void Update()
        {
            // Copy every target transform to a NativeArray.
            for (int i = 0; i < TargetPositions.Length; i++)
            {
                TargetPositions[i] = (float3)Spawner.TargetTransforms[i].localPosition;
            }
            
            // Copy every seeker transform to a NativeArray.
            for (int i = 0; i < SeekerPositions.Length; i++)
            {
                SeekerPositions[i] = (float3)Spawner.SeekerTransforms[i].localPosition;
            }

            var job = new FindNearestJob
            {
                TargetPositions = TargetPositions,
                SeekerPositions = SeekerPositions,
                NearestTargetPositions = NearestTargetPositions
            };

            // Schedule() puts the job instance on the job queue.
            var jobHandle = job.Schedule();
            
            // The Complete method will not return until the job represented by
            // the handle finishes execution. Effectively, the main thread waits
            // here until the job is done.
            jobHandle.Complete();
            
            // Draw a debug line from each seeker to its nearest target.
            for (int i = 0; i < SeekerPositions.Length; i++)
            {
                // float3 is implicitly converted to Vector3
                Debug.DrawLine(SeekerPositions[i], NearestTargetPositions[i]);
            }

        }
    }
}