﻿using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace GucchiCS
{
    public class ResetManager : MonoBehaviour
    {
        enum OBJECT
        {
            PLAYER,
            ROTATE_OBJECT
        }

        // プレイヤー
        [SerializeField]
        Transform _player = null;

        // 回転可能オブジェクト
        List<GameObject> _rotateObjects = new List<GameObject>();

        // 初期座標・回転角
        List<Tuple<Vector3, Quaternion>> _temp = new List<Tuple<Vector3, Quaternion>>();

        // Use this for initialization
        void Awake()
        {
            // プレイヤーの初期地点・初期回転角を保存
            _temp.Add(Tuple.Create(_player.position, _player.rotation));

            // 各オブジェクトの初期地点・初期回転角を保存
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("RotateObject"))
            {
                _rotateObjects.Add(obj);
                _temp.Add(Tuple.Create(obj.transform.position, obj.transform.rotation));
            }
        }

        // リセット
        public void ResetObjects()
        {
            // プレイヤーの行動を非アクティブにする（死亡防止）
            _player.GetComponent<Konji.PlayerMove>().enabled = false;
            _player.GetComponent<Konji.PlayerControl>().enabled = false;

            // プレイヤー
            _player.GetComponent<Konji.PlayerMove>().Reset(_temp[(int)OBJECT.PLAYER].Item1, _temp[(int)OBJECT.PLAYER].Item2);

            // 各オブジェクト
            for (int i = (int)OBJECT.ROTATE_OBJECT; i <= _rotateObjects.Count; i++)
            {
                _rotateObjects[i - 1].transform.position = _temp[i].Item1;
                _rotateObjects[i - 1].transform.rotation = _temp[i].Item2;
            }

            // プレイヤーの行動のアクティブを戻す
            _player.GetComponent<Konji.PlayerMove>().enabled = true;
            _player.GetComponent<Konji.PlayerControl>().enabled = true;

            // オブジェクトの選択
            ModeChanger.Instance.SelectedObject = null;
        }
    }
}
