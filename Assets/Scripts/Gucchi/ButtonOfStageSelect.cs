using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GucchiCS
{
    public class ButtonOfStageSelect : MonoBehaviour
    {
        // クリック時
        public void OnClick()
        {
            if (!StageSelectManager.Instance.IsChanging)
            {
                // ブロック数
                int numBlock = StageSelectManager.Instance.BlockNum;

                // ステージのブロックを回転
                StageSelectManager.Instance.ChangeLightAction(numBlock, true);
            }
        }

        // ボタンの回転
        public void ChangeLightRotate(bool buttonClick = false)
        {
            var scroll = Input.GetAxis("Mouse ScrollWheel");

            // 移動処理
            if (scroll > 0 || Input.GetKeyDown(KeyCode.W))
            {
                transform.DORotate(new Vector3(0f, 0f, 180f), 1f, RotateMode.WorldAxisAdd)
                    .SetRelative();
            }
            else if (scroll < 0 || Input.GetKeyDown(KeyCode.S) || buttonClick)
            {
                transform.DORotate(new Vector3(0f, 0f, -180f), 1f, RotateMode.WorldAxisAdd)
                    .SetRelative();
            }
        }
    }
}