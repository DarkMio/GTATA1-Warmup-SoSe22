using UnityEngine;

namespace Scripts
{
    /// <summary>
    /// The background is generated in a shader - the shader is fed from here.
    /// </summary>
    public class GenerativeBackground : MonoBehaviour
    {
        private static readonly int Offset = Shader.PropertyToID("_Offset");
        [SerializeField] private Camera targetCamera;
        [SerializeField] private MeshRenderer targetRenderer;
        [SerializeField] private PlayerShip ship;

        private Vector3 offset;
        private RenderTexture renderTexture;
        private Vector2Int screenSize;

        private void Start()
        {
            RebuildStarfield();
        }

        private void Update()
        {
            // every time the screen changes, it's necessary to rebuild the render texture
            if (screenSize.x != Screen.width || screenSize.y != Screen.height)
            {
                RebuildStarfield();
            }
            // add offset based on the velocity (not on world-space coordinates, which are modulo'd
            offset += ship.movementObject.CurrentVelocity;
            // set the value in the shader
            targetRenderer.material.SetVector(Offset, offset / 100);
        }

        /// <summary>
        /// Rebuilds components and sets them correctly for the generative background to work correctly
        /// </summary>
        private void RebuildStarfield()
        {
            // if we have already one, then we likely want to get rid of it.
            if (renderTexture != null)
            {
                renderTexture.Release();
            }

            // this is an object initializer and basically sets object members right after calling the constructor
            // it is, too, syntactic sugar.
            renderTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.Default, 0)
            {
                name = "Generative Background Texture"
            };

            // the material can handle everything on its own, it reads the dimensions of the texture in the shader.
            targetRenderer.material.mainTexture = renderTexture;

            // set the texture filling the camera rectangle correctly
            var cameraHeight = targetCamera.orthographicSize * 2;
            var cameraWidth = cameraHeight * targetCamera.aspect;
            targetRenderer.transform.localScale = new Vector3(cameraWidth, cameraHeight, 1);
            screenSize = new Vector2Int(Screen.width, Screen.height);
        }
    }
}