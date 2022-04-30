using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts{
    public class HealthPoint : MonoBehaviour
{
    [SerializeField]private Text _healthPoint;
    // Start is called before the first frame update
    public void onHit(int healthPoint){
        _healthPoint.text="Lifes: "+healthPoint.ToString();
    }

    // Update is called once per frame
    
}
}

