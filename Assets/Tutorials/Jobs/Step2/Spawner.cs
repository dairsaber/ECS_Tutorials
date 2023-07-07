using Tutorials.Jobs.Step1;
using UnityEngine;

namespace Tutorials.Jobs.Step2
{
    public class Spawner : MonoBehaviour
    {
        public static Transform[] TargetTransforms;
        
        // Cache Seeker Transforms.
        public static Transform[] SeekerTransforms;
        
        public GameObject SeekerPrefab;
        public GameObject TargetPrefab;
        public int NumSeekers;
        public int NumTargets;
        public Vector2 Bounds;

        private void Start()
        {
            Random.InitState(123);

            SeekerTransforms = new Transform[NumSeekers];
            for (int i = 0; i < NumSeekers; i++)
            {
                var go = Instantiate(SeekerPrefab);
                var seeker = go.GetComponent<Seeker>();
                var dir = Random.insideUnitCircle;
                seeker.Direction = new Vector3(dir.x, 0, dir.y);
                SeekerTransforms[i] = go.transform;
                go.transform.localPosition = new Vector3(
                    Random.Range(0, Bounds.x), 0, Random.Range(0, Bounds.y)
                );
            }
            
            
            TargetTransforms = new Transform[NumSeekers];
            for (int i = 0; i < NumTargets; i++)
            {
                var go = Instantiate(TargetPrefab);
                var seeker = go.GetComponent<Target>();
                var dir = Random.insideUnitCircle;
                seeker.Direction = new Vector3(dir.x, 0, dir.y);
                TargetTransforms[i] = go.transform;
                go.transform.localPosition = new Vector3(
                    Random.Range(0, Bounds.x), 0, Random.Range(0, Bounds.y)
                );
            }

        }
    }
}