using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Konji
{
    public class WaveEmitter : MonoBehaviour
    {
        // Waveプレハブを格納する
        public GameObject[] _waves;

        // 現在のWave
        public int _currentWave;

        //クールタイム
        public int _coolTime = 10;
        private int _coolTimer = 0;

        void Start()
        {
            StartCoroutine("WaveStart");
        }

        IEnumerator WaveStart()
        {
            // Waveが存在しなければコルーチンを終了する
            if (_waves.Length == 0)
            {
                yield break;
            }

            while (true)
            {
                // Waveを作成する
                GameObject wave = Instantiate(_waves[_currentWave], transform.position, Quaternion.identity);

                // WaveをEmitterの子要素にする
                wave.transform.parent = transform;

                // Waveの子要素のEnemyが全て削除されるまで待機する
                while (wave.transform.childCount != 0)
                {
                    yield return new WaitForEndOfFrame();
                }

                // Waveの削除
                Destroy(wave);

                //ウェーブが終わったらステージ？クリア
                if(_waves.Length <= ++_currentWave)
                {

                }

                //クールタイムへ
                yield return WaitWave();
            }
        }

        //クールタイム
        IEnumerator WaitWave()
        {
            //タイマー初期化
            _coolTimer = 0;

            while(true)
            {
                //タイマー
                while(_coolTimer < _coolTime)
                {
                    _coolTimer++;
                    yield return new WaitForSeconds(1);
                }

                //コルーチンのループがやばい可能性あり
                yield return WaveStart();
            }
        }
    }
}