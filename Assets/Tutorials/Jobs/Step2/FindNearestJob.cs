using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Tutorials.Jobs.Step2
{
    // Include the BurstCompile attribute to Burst compile the job.
    [BurstCompile]
    public struct FindNearestJob : IJob
    {
        // All of the data which a job will access should 
        // be included in its fields. In this case, the job needs
        // three arrays of float3.

        // Array and collection fields that are only read in
        // the job should be marked with the ReadOnly attribute.
        // Although not strictly necessary in this case, marking data  
        // as ReadOnly may allow the job scheduler to safely run 
        // more jobs concurrently with each other.
        // (See the "Intro to jobs" for more detail.)
        [ReadOnly] public NativeArray<float3> TargetPositions;
        [ReadOnly] public NativeArray<float3> SeekerPositions;
        
        // For SeekerPositions[i], we will assign the nearest 
        // target position to NearestTargetPositions[i].
        public NativeArray<float3> NearestTargetPositions;
        
        public void Execute()
        {
            for (int i = 0; i < SeekerPositions.Length; i++)
            {
                var seekerPos = SeekerPositions[i];
                var nearestDistSq = float.MaxValue;

                for (int j = 0; j < TargetPositions.Length; j++)
                {
                    var targetPos = TargetPositions[j];
                    var distSq = math.distancesq(seekerPos, targetPos);
                    if (distSq < nearestDistSq)
                    {
                        nearestDistSq = distSq;
                        NearestTargetPositions[i] = targetPos;
                    }
                }
            }
        }
    }
}