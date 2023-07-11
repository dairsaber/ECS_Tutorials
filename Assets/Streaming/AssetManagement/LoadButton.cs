using System;
using UnityEngine;

namespace Streaming.AssetManagement
{
    public class LoadButton : MonoBehaviour
    {
        public bool Loaded;
        public bool Toggle;


        private void Start()
        {
            Loaded = true;
        }

        private void OnGUI()
        {
            if (Loaded)
            {
                if (GUI.Button(new Rect(10, 10, 150, 80), "Unload Assets"))
                {
                    Toggle = true;
                }
            }
            else
            {
                if (GUI.Button(new Rect(10, 10, 150, 80), "Load Assets"))

                {
                    Toggle = true;
                }
            }
        }
    }
}