using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GucchiCS
{
    public class StageInitializer : MonoBehaviour
    {
        // プレイヤー
        [SerializeField]
        Transform _player = null;

        // クリアオブジェクト
        [SerializeField]
        GameObject _clear = null;

        // ゴール地点
        [SerializeField]
        Vector3 _goalPos = new Vector3(0f, 0f, 8.9f);

        // Use this for initialization
        void Awake()
        {
            _player.localScale = Vector3.zero;

            // プレイヤーとゴールを出現させる
            GameObject clear = Instantiate(_clear, new Vector3(_player.position.x, _player.position.y, 8.9f), Quaternion.identity);
            Sequence seq = DOTween.Sequence()
                .OnStart(() =>
                {
                    clear.GetComponentInChildren<Clear>().Player = _player;
                })
                .Append(clear.transform.DOScale(Vector3.one, 1f))
                .Append(_player.DOScale(Vector3.one, 1f))
                .Append(clear.transform.DOScale(Vector3.zero, 1f))
                .AppendCallback(() => { clear.transform.position = _goalPos; })
                .Join(clear.transform.DOScale(Vector3.one, 1f))
                .AppendCallback(() => 
                {
                    GameManagerKakkoKari.Instance.IsPlay = true;
                });

            seq.Play();
        }
    }
}