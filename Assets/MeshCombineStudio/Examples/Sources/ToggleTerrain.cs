using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeshCombineStudio
{
    public class ToggleTerrain : MonoBehaviour
    {
        public GameObject terrainGO;
        public MeshCombiner meshCombiner;

        GUIStyle textStyle;

        void Awake()
        {
            QualitySettings.shadowDistance = 2000;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (meshCombiner.combined)
                {
                    terrainGO.SetActive(!terrainGO.activeInHierarchy);
                }
            }
        }

        void OnGUI()
        {
            if (textStyle == null)
            {
                textStyle = new GUIStyle("label");
                textStyle.fontStyle = FontStyle.Bold;
                textStyle.fontSize = 16;
            }

            float height = 150;
#if !UNITY_EDITOR
        height = 65;
#endif

            GUI.Box(new Rect(5, 98, 305, height), GUIContent.none);

            textStyle.normal.textColor = terrainGO.activeInHierarchy ? Color.red : Color.green;

            if (meshCombiner.combined)
            {
                GUI.Label(new Rect(10, 100, 300, 60), "For seeing real performance difference the Unity terrain needs to be disabled. Toggle with 'Space' key.", textStyle);
            }

#if UNITY_EDITOR
            textStyle.normal.textColor = Color.red;
            GUI.Label(new Rect(10, 200, 300, 60), "Also test in a Build to see the real performance difference...", textStyle);
#endif
        }
    }
}