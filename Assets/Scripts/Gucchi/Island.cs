/*
 * @Date    18/03/20
 * @Create  Yuta Higuchi
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GucchiCS
{
    // 島
    public class Island : MonoBehaviour, IGround
    {
        // 島に存在するユニットの種類
        enum ISLAND_OCCUPATION : int
        {
            UNIT,
            ENEMY,
            NULL
        }

        // 占領状況
        [SerializeField]
        ISLAND_OCCUPATION _islandState = ISLAND_OCCUPATION.NULL;
        ISLAND_OCCUPATION _islandStateBefore = ISLAND_OCCUPATION.NULL;

        // マテリアル
		public List<Material> _islandMaterial;

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

        // 島にいるユニットリスト
        List<Unit> _unitList;

        // 島にいる敵リスト
        List<Konji.LandingEnemy> _enemyList;

        void Awake()
        {
            _unitList = new List<Unit>();
            _enemyList = new List<Konji.LandingEnemy>();

            // サイズの設定（コード的にやばいので余裕があるときになおす）
            _islandSizeDic[ISLAND_SIZE.SMALL] = _islandSizeData[(int)ISLAND_SIZE.SMALL];
            _islandSizeDic[ISLAND_SIZE.MIDDLE] = _islandSizeData[(int)ISLAND_SIZE.MIDDLE];
            _islandSizeDic[ISLAND_SIZE.LARGE] = _islandSizeData[(int)ISLAND_SIZE.LARGE];

            // 指定されたスケールに変える
            float islandSize = _islandSizeDic[_islandSize];
            transform.localScale = new Vector3(islandSize, 3f, islandSize);

            // マテリアル設定
            transform.GetComponent<Renderer>().material = _islandMaterial[(int)_islandState];

            // 最大占領値設定
            _maxOccupation = _occupationList[(int)_islandSize];

            // すでに占領状態であれば占領値を最大にしておく
            if (_islandState != ISLAND_OCCUPATION.NULL)
            {
                _occupation = _maxOccupation;
            }

            // 占領値を表すテキストを設定
            transform.GetComponentInChildren<TextMesh>().text = "0 / " + _maxOccupation.ToString();

            // 占領状況設定
            _islandStateBefore = _islandState;

            // 遺物設置場所を設定
            transform.GetComponent<RelicSetter>().SetRelicSetter(this, _islandSize);
        }

        // 更新処理
        void Update()
        {
            // 占領値を表すテキストを設定
            transform.GetComponentInChildren<TextMesh>().text = Occupation.ToString() + " / " + _maxOccupation.ToString();
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
                //StartCoroutine(OccupationTimer(ISLAND_OCCUPATION.NULL));
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
                    // 占領者の変更
                    _islandState = occupation;

                    // 占領値を0にする
                    _occupation = 0;
                }

                _occupation++;

                StartCoroutine(OccupationTimer(occupation));
            }

            // 占領完了
            if (_occupation >= _maxOccupation)
            {
                // 占領値を最大の状態にする（念のため）
                _occupation = _maxOccupation;

                // マテリアル設定
                transform.GetComponent<Renderer>().material = _islandMaterial[(int)occupation];

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
            }

            return nearIslands;
        }

        // 現在の島の占領値
        public int Occupation
        {
            get
            {
                return _occupation;
            }
        }

        // 味方ユニットのリスト
        public List<Unit> UnitList
        {
            get
            {
                return _unitList;
            }
        }

        // 敵リスト
        public List<Konji.LandingEnemy> EnemyList
        {
            get
            {
                return _enemyList;
            }
        }
    }
}