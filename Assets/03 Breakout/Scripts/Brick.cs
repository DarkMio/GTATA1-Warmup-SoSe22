using UnityEngine;

namespace Scripts
{
    /// <summary>
    /// Brick component spawns an upgrade if it is marked as upgrade
    /// </summary>
    public class Brick : MonoBehaviour
    {
        [SerializeField] private BrickType brickType;
        [SerializeField] private Upgrade upgradePrefab;

        private void OnDestroy()
        {
            if (brickType == BrickType.Upgrade)
            {
                var upgrade = Instantiate(upgradePrefab, transform.parent);
                upgrade.transform.position = transform.position;
            }
        }
    }
}