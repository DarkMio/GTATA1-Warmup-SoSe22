using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts
{
    public class SceneSelector : MonoBehaviour
    {
        public void SelectScene(int scene)
        {
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }
    }
}
