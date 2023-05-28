using System;
using UnityEngine;

namespace Entities.Game.UI
{
    public class UIController : MonoBehaviour
    {
        public string DebugLabel = "lololol";
        private void OnGUI()
        {
            GUILayout.Label(DebugLabel);
        }
    }
}