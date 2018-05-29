using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace GucchiCS
{
    public class ResetManager : MonoBehaviour
    {
        enum OBJECT
        {
            PLAYER,
            LIGHT,
            DUMMY_LIGHT,
            ROTATE_OBJECT
        }

        // プレイヤー
        [SerializeField]
        Transform _player = null;

        // ライト
        [SerializeField]
        Light _light = null;

        // 見かけ上のライト
        [SerializeField]
        GameObject _dummyLight = null;

        // オブジェクト
        [SerializeField]
        List<GameObject> _rotateObjects = new List<GameObject>();

        // 初期座標・回転角
        List<Tuple<Vector3, Quaternion>> _temp = new List<Tuple<Vector3, Quaternion>>();

        // Use this for initialization
        void Awake()
        {
            // プレイヤーの初期地点・初期回転角を保存
            _temp.Add(Tuple.Create(_player.position, _player.rotation));

            // ライトの初期地点・初期回転角を保存
            _temp.Add(Tuple.Create(_light.transform.position, _light.transform.rotation));

            // 見かけ上のライトの初期地点・初期回転角を保存
            _temp.Add(Tuple.Create(_dummyLight.transform.position, _dummyLight.transform.rotation));

            // 各オブジェクトの初期地点・初期回転角を保存
            foreach (GameObject obj in _rotateObjects)
            {
                _temp.Add(Tuple.Create(obj.transform.position, obj.transform.rotation));
            }

            // モード切り替え中、ポーズ中はボタンを押させなくする（アクティブなしにする）
            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    if (!StageManager.Instance.IsPlay   ||
                        ModeChanger.Instance.IsChanging ||
                        ModeChanger.Instance.IsRotate   ||
                        Pausable.Instance.pausing)
                    {
                        transform.GetComponent<Button>().interactable = false;
                        transform.GetComponentInChildren<Text>().color = Color.gray;
                        return;
                    }

                    transform.GetComponent<Button>().interactable = true;
                    transform.GetComponentInChildren<Text>().color = Color.red;
                });
        }

        // リセット
        public void ResetObjects()
        {
            // プレイヤー
            _player.position = _temp[(int)OBJECT.PLAYER].Item1;
            _player.rotation = _temp[(int)OBJECT.PLAYER].Item2;

            // ライト
            _light.transform.position = _temp[(int)OBJECT.LIGHT].Item1;
            _light.transform.rotation = _temp[(int)OBJECT.LIGHT].Item2;

            // 見かけ上のライト
            _dummyLight.transform.position = _temp[(int)OBJECT.DUMMY_LIGHT].Item1;
            _dummyLight.transform.rotation = _temp[(int)OBJECT.DUMMY_LIGHT].Item2;

            // 各オブジェクト
            int i = (int)OBJECT.ROTATE_OBJECT;
            foreach (GameObject obj in _rotateObjects)
            {
                obj.transform.position = _temp[i].Item1;
                obj.transform.rotation = _temp[i].Item2;
                i++;
            }

            // オブジェクトの選択
            ModeChanger.Instance.SelectedObject = null;
        }
    }
}
