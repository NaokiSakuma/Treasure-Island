using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class ArrowAnimation : MonoBehaviour {

	[SerializeField]
	MoveMap _moveMap;

	[SerializeField]
	private Image[] _arrows;

	[SerializeField, Range(0.0f, 10.0f)]
	public float _scalingSpeed = 1.0f;

	[SerializeField, Range(1.0f, 2.0f)]
	public float _scaleLimit = 2.0f;

	// Use this for initialization
	void Start () {
		var mm = _moveMap;

		_arrows = GetComponentsInChildren<Image>();

		// くっそ汚いのでうまいことまとめる方法知ってたらおしえてクレメンス
		// === 左の矢印 ===
		// 拡大
		this.ObserveEveryValueChanged(x => mm.MoveDirection.x)
		    .Where(x => x < 0.0f)
		    .Where(x => _arrows[0].transform.localScale.x < _scaleLimit)
			.Subscribe(_ => {
				_arrows[0].transform.localScale += Vector3.one * _scalingSpeed * Time.deltaTime;
				if (_arrows[0].transform.localScale.x > _scaleLimit) {
					_arrows[0].transform.localScale = Vector3.one * _scaleLimit;
				}
			});

		// 縮小
		this.UpdateAsObservable()
		    .Where(x => mm.MoveDirection.x >= 0.0f)
			.Where(x => _arrows[0].transform.localScale.x > 1.0f)
			.Subscribe(_ => {
				_arrows[0].transform.localScale -= Vector3.one * _scalingSpeed * Time.deltaTime;
				if (_arrows[0].transform.localScale.x < 1.0f) {
					_arrows[0].transform.localScale = Vector3.one;
				}
			});

		// === 右の矢印 ===
		// 拡大
		this.ObserveEveryValueChanged(x => mm.MoveDirection.x)
			.Where(x => x > 0.0f)
			.Where(x => _arrows[1].transform.localScale.x < _scaleLimit)
			.Subscribe(_ => {
				_arrows[1].transform.localScale += Vector3.one * _scalingSpeed * Time.deltaTime;
				if (_arrows[1].transform.localScale.x > _scaleLimit) {
					_arrows[1].transform.localScale = Vector3.one * _scaleLimit;
				}
			});
		// 縮小
		this.UpdateAsObservable()
			.Where(x => mm.MoveDirection.x <= 0.0f)
			.Where(x => _arrows[1].transform.localScale.x > 1.0f)
			.Subscribe(_ => {
				_arrows[1].transform.localScale -= Vector3.one * _scalingSpeed * Time.deltaTime;
				if (_arrows[1].transform.localScale.x < 1.0f) {
					_arrows[1].transform.localScale = Vector3.one;
				}
			});

		// === 上の矢印 ===
		// 拡大
		this.ObserveEveryValueChanged(y => mm.MoveDirection.z)
			.Where(z => z > 0.0f)
			.Where(_ => _arrows[2].transform.localScale.y < _scaleLimit)
			.Subscribe(_ => {
				_arrows[2].transform.localScale += Vector3.one * _scalingSpeed * Time.deltaTime;
				if (_arrows[2].transform.localScale.y > _scaleLimit) {
					_arrows[2].transform.localScale = Vector3.one * _scaleLimit;
				}
			});
		// 縮小
		this.UpdateAsObservable()
			.Where(_ => mm.MoveDirection.z <= 0.0f)
			.Where(_ => _arrows[2].transform.localScale.y > 1.0f)
			.Subscribe(_ => {
				_arrows[2].transform.localScale -= Vector3.one * _scalingSpeed * Time.deltaTime;
				if (_arrows[2].transform.localScale.y < 1.0f) {
					_arrows[2].transform.localScale = Vector3.one;
				}
			});

		// === 下の矢印 ===
		// 拡大
		this.ObserveEveryValueChanged(y => mm.MoveDirection.z)
			.Where(z => z < 0.0f)
			.Where(_ => _arrows[3].transform.localScale.y < _scaleLimit)
			.Subscribe(_ => {
				_arrows[3].transform.localScale += Vector3.one * _scalingSpeed * Time.deltaTime;
				if (_arrows[3].transform.localScale.y > _scaleLimit) {
					_arrows[3].transform.localScale = Vector3.one * _scaleLimit;
				}
			});
		// 縮小
		this.UpdateAsObservable()
			.Where(_ => mm.MoveDirection.z >= 0.0f)
			.Where(_ => _arrows[3].transform.localScale.y > 1.0f)
			.Subscribe(_ => {
				_arrows[3].transform.localScale -= Vector3.one * _scalingSpeed * Time.deltaTime;
				if (_arrows[3].transform.localScale.y < 1.0f) {
					_arrows[3].transform.localScale = Vector3.one;
				}
			});
	}
}
