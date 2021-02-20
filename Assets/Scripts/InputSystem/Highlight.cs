using UnityEngine;

namespace Assets.Scripts.InputSystem
{
    public class Highlight  : MonoBehaviour
    {
        public Color ColorHiglightA = Color.blue;
        public Color[] ColorFromArrayy = new Color[] { Color.yellow, Color.blue, Color.magenta, Color.cyan, Color.green, Color.red, Color.white };
        public float mult = 2;
        public float widthH = 0.01f;
        public bool isOff = true;
        public bool OnlyOnce = true;
        public float mnozitelOutlineVR = 0.1f;

        private Shader shaderOutline;
        private MeshRenderer[] allMeshes;
        private GameObject[] highlightArray;

        private void Start()
        {
            allMeshes = GetComponents<MeshRenderer>();
            if (allMeshes == null) GetComponentsInChildren<MeshRenderer>();
            if (allMeshes != null) { highlightArray = new GameObject[allMeshes.Length]; }
            shaderOutline = Shader.Find("Outlined/SilhouetteOnly");
        }
    }
}
