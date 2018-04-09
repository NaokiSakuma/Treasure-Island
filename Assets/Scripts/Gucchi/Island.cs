/*
 * @Date    18/03/20
 * @Create  Yuta Higuchi
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GucchiCS
{
    // 島
    public class Island : MonoBehaviour, IGround
    {
        // 島に存在するユニットの種類
        public enum ISLAND_OCCUPATION : int
        {
            UNIT,
            ENEMY,
            BATTLE,
            NULL
        }

        // 占領状況
        [SerializeField]
        ISLAND_OCCUPATION _islandState = ISLAND_OCCUPATION.NULL;
        ISLAND_OCCUPATION _islandStateBefore = ISLAND_OCCUPATION.NULL;

        // サイズリスト
        public List<float> _islandSizeData = new List<float>() {
            35f,
            50f, 
            75f
        };

        // サイズプルダウン用
        public enum ISLAND_SIZE : int
        {
            SMALL,
            MIDDLE,
            LARGE
        }

        // サイズの管理
        Dictionary<ISLAND_SIZE, float> _islandSizeDic = new Dictionary<ISLAND_SIZE, float>() {
            { ISLAND_SIZE.SMALL,  0f },
            { ISLAND_SIZE.MIDDLE, 0f },
            { ISLAND_SIZE.LARGE,  0f }
        };

        // 島のサイズ
        [SerializeField]
        ISLAND_SIZE _islandSize = ISLAND_SIZE.SMALL;

        // 移動範囲
        [SerializeField]
        float _movingRange = 8f;

        // 占領値
        public List<int> _occupationList = new List<int>()
        {
            100,
            300,
            500
        };
        int _maxOccupation;

        // 現在の占領値
        int _occupation;

        // 占領タイマーの間隔
        [SerializeField]
        float _occupationTimerInterval = 0.01f;

        // 占領旗
        public OccupationFlag _flag;

        // 占領ゲージ
        public List<Slider> _occupationGage;

        // 占領されたときにイベントを発生させる場合に使うオブジェクト
        public List<GameObject> _event = null;

        // スコア
        [SerializeField]
        int _score = 1000;

        // 島にいるユニットリスト
        List<Unit> _unitList = new List<Unit>();

        // 島にいる敵リスト
        List<Konji.LandingEnemy> _enemyList = new List<Konji.LandingEnemy>();

        // 島に置いてある遺物リスト
        List<Konji.Relic> _relicList = new List<Konji.Relic>();

        void Awake()
        {
            // サイズの設定（コード的にやばいので余裕があるときになおす）
            _islandSizeDic[ISLAND_SIZE.SMALL] = _islandSizeData[(int)ISLAND_SIZE.SMALL];
            _islandSizeDic[ISLAND_SIZE.MIDDLE] = _islandSizeData[(int)ISLAND_SIZE.MIDDLE];
            _islandSizeDic[ISLAND_SIZE.LARGE] = _islandSizeData[(int)ISLAND_SIZE.LARGE];

            // 指定されたスケールに変える
            float islandSize = _islandSizeDic[_islandSize];
            transform.localScale = new Vector3(islandSize, 3f, islandSize);

            // キャンバス設定
            transform.GetComponentInChildren<Canvas>().transform.Translate(new Vector3(0, 25f, _islandSizeDic[_islandSize] / 2f));

            // 初期の占領状況に応じて占領旗の色変更と占領ゲージを設定
            if (_islandState != ISLAND_OCCUPATION.NULL)
            {
                // 占領旗
                OccupationFlag flag = Instantiate(_flag, new Vector3(transform.position.x, 1.5f, transform.position.z + _islandSizeDic[_islandSize] / 3f), Quaternion.identity);
                flag.transform.SetParent(this.transform, true);
                flag.ChangeMaterial(_islandState);

                // 占領ゲージ
                Slider gage = Instantiate(_occupationGage[(int)_islandState], Vector3.zero, Quaternion.identity);
                gage.transform.SetParent(transform.GetComponentInChildren<Canvas>().transform, false);
                gage.transform.localScale = new Vector3(0.5f, 1f, 0.5f);
                gage.GetComponent<Slider>().value = 0f;
            }

            // 最大占領値設定
            _maxOccupation = _occupationList[(int)_islandSize];

            // すでに占領状態であれば占領値を最大にしておく
            if (_islandState != ISLAND_OCCUPATION.NULL)
            {
                _occupation = _maxOccupation;
            }

            // 占領状況設定
            _islandStateBefore = _islandState;

            // 遺物設置場所を設定
            transform.GetComponent<RelicSetter>().SetRelicSetter(this, _islandSize);
        }

        // 更新処理
        void Update()
        {
            Slider gage = transform.GetComponentInChildren<Canvas>().GetComponentInChildren<Slider>();

            if (gage == null)
                return;

            // 戦闘中
            if (_islandState == ISLAND_OCCUPATION.BATTLE)
            {
                gage.value = _unitList.Count / (_unitList.Count + _enemyList.Count);
            }
            else if (_islandState != ISLAND_OCCUPATION.NULL)
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

            // 味方・敵ともに存在
            bool battle = _unitList.Count > 0 && _enemyList.Count > 0;

            // 味方が占領している場合
            if (superiorityUnit && _islandStateBefore != ISLAND_OCCUPATION.UNIT)
            {
                StartCoroutine(OccupationTimer(ISLAND_OCCUPATION.UNIT));
                return;
            }

            // 敵が占領している場合
            if (superiorityEnemy && _islandStateBefore != ISLAND_OCCUPATION.ENEMY)
            {
                StartCoroutine(OccupationTimer(ISLAND_OCCUPATION.ENEMY));
                return;
            }

            // ユニットがいない場合
            if (emptyIsland && _islandStateBefore != ISLAND_OCCUPATION.NULL)
            {
                StartCoroutine(OccupationTimer(ISLAND_OCCUPATION.NULL));
                return;
            }

            // 戦闘状態
            if (battle && _islandStateBefore != ISLAND_OCCUPATION.BATTLE)
            {
                // 占領状況の変更
                _islandState = ISLAND_OCCUPATION.BATTLE;

                // すでに占領ゲージが存在している場合は削除
                if (transform.GetComponentInChildren<Canvas>().GetComponentInChildren<Slider>() != null)
                {
                    Destroy(transform.GetComponentInChildren<Canvas>().GetComponentInChildren<Slider>());
                }

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
        IEnumerator OccupationTimer(ISLAND_OCCUPATION occupation)
        {
            yield return new WaitForSeconds(_occupationTimerInterval);

            // 占領準備中
            if (occupation != ISLAND_OCCUPATION.NULL)
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
                if (_islandState != ISLAND_OCCUPATION.NULL)
                {
                    OccupationFlag flag = transform.GetComponentInChildren<OccupationFlag>();

                    // 旗がすでに存在している場合
                    if (flag != null)
                    {
                        // マテリアルのみ変更
                        flag.ChangeMaterial(_islandState);
                    }
                    // 旗がない場合
                    else
                    {
                        // 旗の生成
                        flag = Instantiate(_flag, new Vector3(transform.position.x, 1.5f, transform.position.z + _islandSizeDic[_islandSize] / 3f), Quaternion.identity);
                        flag.transform.SetParent(this.transform, true);
                        flag.ChangeMaterial(_islandState);
                    }
                }

                // イベントがあれば発生させる
                if (_event.Count > 0)
                {
                    foreach (GameObject ev in _event)
                    {
                        ev.GetComponent<IslandOccupationEvent>().OccupationNotify();
                    }
                }

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

        // 遺物の登録（もっとうまい方法あれば変えたい）
        public void RegisterRelic(Konji.Relic relic)
        {
            _relicList.Add(relic);
        }

        // 遺物の解除（もっとうまい方法あれば変えたい）
        public void RemoveRelic(Konji.Relic relic)
        {
            _relicList.Remove(relic);
        }

        /* プロパティ */

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

        // 遺物リスト
        public List<Konji.Relic> RelicList
        {
            get { return _relicList; }
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