﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace GucchiCS
{
    public class SelectLight : MonoBehaviour
    {
        // ブロックID
        int _blockID = 0;

        // 扉
        [SerializeField]
        Transform _doors = null;

        // ライト移動処理
        public void ChangeLightAction(int numBlock, bool buttonClick = false)
        {
            var scroll = Input.GetAxis("Mouse ScrollWheel");

            // 移動処理
            if (scroll > 0 || Input.GetKeyDown(KeyCode.W))
            {
                transform.DORotate(new Vector3(0f, 0f, (360f / numBlock)), 1f, RotateMode.WorldAxisAdd)
                    .SetRelative()
                    .OnComplete(() => StageSelectManager.Instance.AnimationCompleteNotify());
            }
            else if (scroll < 0 || Input.GetKeyDown(KeyCode.S) || buttonClick)
            {
                transform.DORotate(new Vector3(0f, 0f, -(360f / numBlock)), 1f, RotateMode.WorldAxisAdd)
                    .SetRelative()
                    .OnComplete(() => StageSelectManager.Instance.AnimationCompleteNotify());
            }
        }

        // 初期選択用強制設定（ステージから戻ってきたとき）
        public void SetBeforeLightPos(int count, int numBlock)
        {
            // 指定回数分移動
            for (int i = 0; i < count; i++)
            {
                transform.DORotate(new Vector3(0f, 0f, -(360f / numBlock)), 1f, RotateMode.WorldAxisAdd)
                    .SetRelative()
                    .OnComplete(() => StageSelectManager.Instance.AnimationCompleteNotify());
            }
        }

        // ID設定
        public int BlockID
        {
            get { return _blockID; }
            set
            {
                _blockID = value;
                SetStageNo();
            }
        }

        // ステージ番号設定
        void SetStageNo()
        {
            int i = 0;
            foreach (Transform door in _doors.transform)
            {
                door.GetComponent<Door>().DoorID = i + _blockID * 10;
                foreach (Transform child in door)
                {
                    if (child.GetComponent<TextMesh>() != null)
                    {
                        child.GetComponent<TextMesh>().text = ((i + 1) + _blockID * 10).ToString();
                    }
                }
                i++;
            }
        }
    }
}