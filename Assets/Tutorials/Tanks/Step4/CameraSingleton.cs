using System;
using UnityEngine;

namespace Tutorials.Tanks.Step4
{
    public class CameraSingleton : MonoBehaviour
    {
        public static Camera Instance;

        private void Awake()
        {
            Instance = GetComponent<Camera>();
        }
    }
}