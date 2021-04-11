using UnityEngine;

namespace Scripts
{
    public static class ExtensionAPI
    {
        /// <summary>
        /// Extension method that does a modulo-like operation onto view frustum coordinates, repositioning an object
        /// within the orthographic frustum rectangle
        /// </summary>
        public static void RepositionSpriteRendererInFrustum(this SpriteRenderer renderer, Camera camera)
        {
            var transform = renderer.transform;
            var cameraRectangle = OrthographicBounds(camera);

            // horizontal axis handling
            if (transform.position.x > 0)
            {
                var bound = renderer.bounds.min.x;
                if (bound > cameraRectangle.max.x)
                {
                    var position = transform.position;
                    position.x -= cameraRectangle.size.x + renderer.bounds.size.x;
                    transform.position = position;
                }
            }
            else if (transform.position.x < 0)
            {
                var bound = renderer.bounds.max.x;
                if (bound < cameraRectangle.min.x)
                {
                    var position = transform.position;
                    position.x += cameraRectangle.size.x + renderer.bounds.size.x;
                    transform.position = position;
                }
            }

            // vertical axis handling
            if (transform.position.y > 0)
            {
                var bound = renderer.bounds.min.y;
                if (bound > cameraRectangle.max.y)
                {
                    var position = transform.position;
                    position.y -= cameraRectangle.size.y + renderer.bounds.size.y;
                    transform.position = position;
                }
            }
            else if (transform.position.y < 0)
            {
                var bound = renderer.bounds.max.y;
                if (bound < cameraRectangle.min.y)
                {
                    var position = transform.position;
                    position.y += cameraRectangle.size.y + renderer.bounds.size.y;
                    transform.position = position;
                }
            }
        }

        /// <summary>
        /// Extract the orthographic boundaries from a given Camera
        /// </summary>
        public static Bounds OrthographicBounds(this Camera camera)
        {
            var screenAspect = Screen.width / (float) Screen.height;
            var cameraHeight = camera.orthographicSize * 2;
            var bounds = new Bounds(
                camera.transform.position,
                new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
            return bounds;
        }
    }
}