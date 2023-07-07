using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Tutorials.Jobs.Step3
{
    [BurstCompile]
    public struct FindNearestJob:IJobParallelFor
    {
        [ReadOnly] public NativeArray<float3> TargetPositions;
        [ReadOnly] public NativeArray<float3> SeekerPositions;

        public NativeArray<float3> NearestTargetPositions;
        
        public void Execute(int index)
        {
            var seekerPos = SeekerPositions[index];
            var nearestDistSq = float.MaxValue;
            for (var i = 0; i < TargetPositions.Length; i++)
            {
                var targetPos = TargetPositions[i];
                var distSq = math.distancesq(seekerPos, targetPos);
                if (distSq < nearestDistSq)
                {
                    nearestDistSq = distSq;
                    NearestTargetPositions[index] = targetPos;
                }
            }
        }
    }
}