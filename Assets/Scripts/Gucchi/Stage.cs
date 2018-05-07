using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GucchiCS
{
    public class Stage : MonoBehaviour
    {
        // プレイヤー
        [SerializeField]
        Konji.PlayerControl _playerPrefab = null;
        Konji.PlayerControl _player = null;

        // プレイヤーの座標
        [SerializeField]
        Vector3 _playerPos = Vector3.zero;

        // クリアオブジェクト
        [SerializeField]
        Clear _clearPrefab = null;
        Clear _clear = null;

        // クリアオブジェクトの座標
        [SerializeField]
        Vector3 _clearObjPos = Vector3.zero;

        // Use this for initialization
        void Awake()
        {
            // プレイヤー生成
            _player = Instantiate(_playerPrefab, _playerPos, Quaternion.identity);

            // クリアオブジェクト生成
            _clear = Instantiate(_clearPrefab, _clearObjPos, Quaternion.identity);
            _clear.Player = _player.transform;
        }

        /* プロパティ */

        // プレイヤー
        public Konji.PlayerControl Player
        {
            get { return _player; }
        }
    }
}