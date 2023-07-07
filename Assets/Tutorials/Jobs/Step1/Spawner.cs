using UnityEngine;

namespace Tutorials.Jobs.Step1
{
    public class Spawner : MonoBehaviour
    {
        // The set of targets is fixed, so rather than 
        // retrieve the targets every frame, we'll cache 
        // their transforms in this field.
        public static Transform[] TargetTransforms;

        public GameObject SeekerPrefab;
        public GameObject TargetPrefab;
        public int NumSeekers;
        public int NumberTargets;
        public Vector2 Bounds;

        private void Start()
        {
            Random.InitState(123);
            for (int i = 0; i < NumSeekers; i++)
            {
                var go = Instantiate(SeekerPrefab);
                var seeker = go.GetComponent<Seeker>();
                var dir = Random.insideUnitCircle;
                seeker.Direction = new Vector3(dir.x, 0, dir.y);
                go.transform.localPosition = new Vector3(
                    Random.Range(0, Bounds.x),
                    0,
                    Random.Range(0, Bounds.y)
                );
            }

            TargetTransforms = new Transform[NumberTargets];
            for (int i = 0; i < NumberTargets; i++)
            {
                var go = Instantiate(TargetPrefab);
                var target = go.GetComponent<Target>();
                var dir = Random.insideUnitCircle;
                target.Direction = new Vector3(dir.x, 0, dir.y);
                TargetTransforms[i] = go.transform;
                go.transform.localPosition = new Vector3(
                    Random.Range(0, Bounds.x),
                    0,
                    Random.Range(0, Bounds.y)
                );
            }
        }
    }
}