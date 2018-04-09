/*
 * @Date    18/04/08
 * @Create  Yuta Higuchi
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GucchiCS
{
    public class Fishes : MonoBehaviour, IGround
    {
        // 占拠状況
        Island.ISLAND_OCCUPATION _islandState = Island.ISLAND_OCCUPATION.NULL;
        Island.ISLAND_OCCUPATION _islandStateBefore = Island.ISLAND_OCCUPATION.NULL;

        // 移動範囲
        [SerializeField]
        float _movingRange = 100f;

        // 占領値
        [SerializeField]
        int _maxOccupation = 100;

        // 現在の占領値
        int _occupation;

        // 占領タイマーの間隔
        [SerializeField]
        float _occupationTimerInterval = 0.01f;

        // 占領旗
        public OccupationFlag _flag;

        // 占領ゲージ
        public List<Slider> _occupationGage;

        // スコア
        [SerializeField]
        int _score = 500;

        // 資源
        [SerializeField]
        int _resource = 0;

        // 島にいるユニットリスト
        List<Unit> _unitList = new List<Unit>();

        // 島にいる敵リスト
        List<Konji.LandingEnemy> _enemyList = new List<Konji.LandingEnemy>();

        void Awake()
        {
            // キャンバス設定
            transform.GetComponentInChildren<Canvas>().transform.Translate(new Vector3(0, 25f, 15f));
        }

        void Update()
        {
            Slider gage = transform.GetComponentInChildren<Canvas>().GetComponentInChildren<Slider>();

            if (gage == null)
                return;

            if (_islandState != Island.ISLAND_OCCUPATION.NULL)
            {
                // 占領値ゲージ設定
                gage.value = _occupation / (float)_maxOccupation;
            }
        }

        // 占拠状況
        void CheckOccupationState()
        {
            // 味方が占領中
            bool superiorityUnit = _unitList.Count > 0 && _enemyList.Count <= 0;

            // 敵が占領中
            bool superiorityEnemy = _unitList.Count <= 0 && _enemyList.Count > 0;

            // ユニットなし
            bool emptyIsland = _unitList.Count <= 0 && _enemyList.Count <= 0;

            // 味方が占領している場合
            if (superiorityUnit && _islandStateBefore != Island.ISLAND_OCCUPATION.UNIT)
            {
                StartCoroutine(OccupationTimer(Island.ISLAND_OCCUPATION.UNIT));
                return;
            }

            // 敵が占領している場合
            if (superiorityEnemy && _islandStateBefore != Island.ISLAND_OCCUPATION.ENEMY)
            {
                StartCoroutine(OccupationTimer(Island.ISLAND_OCCUPATION.ENEMY));
                return;
            }

            // ユニットがいない場合
            if (emptyIsland && _islandStateBefore != Island.ISLAND_OCCUPATION.NULL)
            {
                StartCoroutine(OccupationTimer(Island.ISLAND_OCCUPATION.NULL));
                return;
            }

            // 戦闘状態
            if (_islandStateBefore != Island.ISLAND_OCCUPATION.BATTLE)
            {
                // 占領状況の変更
                _islandState = Island.ISLAND_OCCUPATION.BATTLE;

                // 戦闘ゲージ生成
                Slider gage = Instantiate(_occupationGage[(int)_islandState], Vector3.zero, Quaternion.identity);
                gage.transform.SetParent(transform.GetComponentInChildren<Canvas>().transform, false);
                gage.transform.localScale = new Vector3(0.5f, 1f, 0.5f);
                gage.GetComponent<Slider>().value = 0f;

                // 変更完了
                _islandStateBefore = _islandState;
            }
        }

        // 占領タイマー
        IEnumerator OccupationTimer(Island.ISLAND_OCCUPATION occupation)
        {
            yield return new WaitForSeconds(_occupationTimerInterval);

            // 占領準備中
            if (occupation != Island.ISLAND_OCCUPATION.NULL)
            {
                if (_islandState != occupation)
                {
                    // 占領状況の変更
                    _islandState = occupation;

                    // 占領値を0にする
                    _occupation = 0;

                    // すでに占領ゲージが存在している場合は削除
                    if (transform.GetComponentInChildren<Canvas>().GetComponentInChildren<Slider>() != null)
                    {
                        Destroy(transform.GetComponentInChildren<Canvas>().GetComponentInChildren<Slider>());
                    }

                    // 再生成
                    Slider gage = Instantiate(_occupationGage[(int)_islandState], Vector3.zero, Quaternion.identity);
                    gage.transform.SetParent(transform.GetComponentInChildren<Canvas>().transform, false);
                    gage.transform.localScale = new Vector3(0.5f, 1f, 0.5f);
                    gage.GetComponent<Slider>().value = 0f;
                }

                _occupation++;

                StartCoroutine(OccupationTimer(occupation));
            }

            // 占領完了
            if (_occupation >= _maxOccupation)
            {
                // 占領値を最大の状態にする（念のため）
                _occupation = _maxOccupation;

                // 占領状況に応じてマテリアルを変更
                if (_islandState != Island.ISLAND_OCCUPATION.NULL)
                {
                    OccupationFlag flag = transform.GetComponentInChildren<OccupationFlag>();

                    // 旗がすでに存在している場合
                    if (flag != null)
                    {
                        // マテリアルのみ変更
                        flag.ChangeMaterial(_islandState);
                    }
                    // 旗ない場合
                    else
                    {
                        // 旗の生成
                        flag = Instantiate(_flag, new Vector3(transform.position.x, 1.5f, transform.position.z + 10f), Quaternion.identity);
                        flag.transform.SetParent(this.transform, true);
                        flag.ChangeMaterial(_islandState);
                    }

                    // 味方の占領なら資源を取得
                    if (_islandState == Island.ISLAND_OCCUPATION.UNIT)
                    {
                        // 資源を取得
                        Konji.ResourceManager.Instance.Resource += _resource;
                        _resource = 0;
                    }
                }

                // 占領されたらイベント発生
                transform.GetComponent<FishesEvent>().OccupationNotify();

                // 占領者の変更
                _islandStateBefore = _islandState;
            }
        }

        // 味方または敵が島に上陸したときに呼ぶ処理
        public void LandingNotify(ICharacter character)
        {
            Debug.Log(character);

            // 占拠状況
            CheckOccupationState();
        }

        // 移動できる範囲内にある島を取得
        public List<IGround> GetNearIslands()
        {
            List<IGround> nearIslands = new List<IGround>();

            // 移動範囲内にあるColliderを取得
            Collider[] targets = Physics.OverlapSphere(transform.position, _movingRange);

            // 取得したColliderのうち島のみを取得
            foreach (Collider col in targets)
            {
                if (col.gameObject.GetComponent<Island>() != null)
                {
                    IGround ground = col.gameObject.GetComponent<Island>();

                    nearIslands.Add(ground);
                }
                else if (col.gameObject.GetComponent<Fishes>() != null)
                {
                    IGround ground = col.gameObject.GetComponent<Fishes>();

                    nearIslands.Add(ground);
                }
            }

            return nearIslands;
        }

        /* プロパティ */

        // 占領状況
        public Island.ISLAND_OCCUPATION IslandState
        {
            get { return _islandState; }
        }

        // 現在の島の占領値
        public int Occupation
        {
            get { return _occupation; }
        }

        // 味方ユニットのリスト
        public List<Unit> UnitList
        {
            get { return _unitList; }
        }

        // 敵リスト
        public List<Konji.LandingEnemy> EnemyList
        {
            get { return _enemyList; }
        }

        // スコア
        public int Score
        {
            get
            {
                return _score;
            }
        }
    }
}