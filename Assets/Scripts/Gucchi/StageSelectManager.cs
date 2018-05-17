﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace GucchiCS
{
    public class StageSelectManager : SingletonMonoBehaviour<StageSelectManager>
    {
        // ブロックリスト
        [SerializeField]
        List<SelectLight> _blockList = new List<SelectLight>();

        // 位置の間隔
        [SerializeField]
        float _posInterval = 20f;

        // ライト変更中かどうか
        bool _isChanging = false;

        // Use this for initialization
        void Start()
        {
            // ブロック数
            int numBlock = _blockList.Count;

            // 配置
            if (numBlock > 0)
            {
                for (int i = 0; i < numBlock; i++)
                {
                    _blockList[i].BlockID = i;

                    // 位置設定
                    float angle = 360f / numBlock * i;
                    foreach (Transform child in _blockList[i].transform)
                    {
                        child.position = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle) * _posInterval, Mathf.Sin(Mathf.Deg2Rad * angle) * _posInterval, -10f);
                        child.LookAt(child.root);
                        child.rotation = Quaternion.Euler(-angle, -angle, 0f);
                    }
                }
            }

            // ホイール操作（手前）でライト変更
            this.UpdateAsObservable()
                .Where(_ => Input.GetAxis("Mouse ScrollWheel") < 0)
                .Where(_ => !_isChanging)
                .Subscribe(_ => ChangeLightAction(numBlock));

            // ホイール操作（奥）でライト変更
            this.UpdateAsObservable()
                .Where(_ => Input.GetAxis("Mouse ScrollWheel") > 0)
                .Where(_ => !_isChanging)
                .Subscribe(_ => ChangeLightAction(numBlock));

            // 回転完了で再び動かせる
            this.ObserveEveryValueChanged(_ => _isChanging)
                .Where(_ => _isChanging)
                .Delay(System.TimeSpan.FromSeconds(1f))
                .Subscribe(_ => { _isChanging = false; });
        }

        // ライト回転
        void ChangeLightAction(int numBlock)
        {
            _isChanging = true;

            // すべてのブロックを移動
            foreach (SelectLight block in _blockList)
            {
                block.ChangeLightAction(numBlock);
            }
        }
    }
}