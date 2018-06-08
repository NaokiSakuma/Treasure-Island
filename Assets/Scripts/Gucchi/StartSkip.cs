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
            // SEを鳴らす
            AudioManager.Instance.PlaySE(AUDIO.SE_BUTTON);

            _initializer.GetComponent<StageInitializer>().Skip();
        }
    }
}