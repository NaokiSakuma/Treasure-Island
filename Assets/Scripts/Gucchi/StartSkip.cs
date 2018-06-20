using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GucchiCS
{
    public class StartSkip : MonoBehaviour
    {
        // Initializer
        [SerializeField]
        GameObject _initializer = null;

        public void OnClick()
        {
            _initializer.GetComponent<StageInitializer>().Skip();
        }
    }
}