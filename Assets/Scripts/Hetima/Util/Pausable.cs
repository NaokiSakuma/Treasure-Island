using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Rigidbodyの速度を保存しておくクラス
/// </summary>
public class RigidbodyVelocity{
	public Vector3 velocity;
	public Vector3 angularVeloccity;
	public RigidbodyVelocity (Rigidbody rigidbody){
		velocity = rigidbody.velocity;
		angularVeloccity = rigidbody.angularVelocity;
	}
}

public class Pausable : SingletonMonoBehaviour<Pausable>{
	/// <summary>
	/// 現在Pause中か？
	/// </summary>
	public bool pausing;

	/// <summary>
	/// 無視するGameObject
	/// </summary>
	public GameObject[] ignoreGameObjects;

	/// <summary>
	/// ポーズ状態が変更された瞬間を調べるため、前回のポーズ状況を記録しておく
	/// </summary>
	bool _prevPausing;

	/// <summary>
	/// Rigidbodyのポーズ前の速度の配列
	/// </summary>
	RigidbodyVelocity[] _rigidbodyVelocities;

	/// <summary>
	/// ポーズ中のRigidbodyの配列
	/// </summary>
	Rigidbody[] _pausingRigidbodies;

	/// <summary>
	/// ポーズ中のMonoBehaviourの配列
	/// </summary>
	MonoBehaviour[] _pausingMonoBehaviours;

	/// <summary>
	/// 更新処理
	/// </summary>
	void Update (){
		// ポーズ状態が変更されていたら、Pause/Resumeを呼び出す。
		if (_prevPausing != pausing){
			if (pausing){
				Pause();
			}
			else{
				Resume();
			}
			_prevPausing = pausing;
		}
	}

	/// <summary>
	/// 中断
	/// </summary>
	void Pause ()
	{
		// Rigidbodyの停止
		// 子要素から、スリープ中でなく、IgnoreGameObjectsに含まれていないRigidbodyを抽出
		Predicate<Rigidbody> rigidbodyPredicate =
			obj => !obj.IsSleeping() &&
				   Array.FindIndex(ignoreGameObjects, gameObject => gameObject == obj.gameObject) < 0;
		_pausingRigidbodies = Array.FindAll(transform.GetComponentsInChildren<Rigidbody>(), rigidbodyPredicate);
		_rigidbodyVelocities = new RigidbodyVelocity[_pausingRigidbodies.Length];
		for (int i = 0; i < _pausingRigidbodies.Length; i++){
			// 速度、角速度も保存しておく
			_rigidbodyVelocities[i] = new RigidbodyVelocity(_pausingRigidbodies[i]);
			_pausingRigidbodies[i].Sleep();
		}

		// MonoBehaviourの停止
		// 子要素から、有効かつこのインスタンスでないもの、IgnoreGameObjectsに含まれていないMonoBehaviourを抽出
		Predicate<MonoBehaviour> monoBehaviourPredicate =
			obj => obj.enabled &&
				   obj != this &&
				   Array.FindIndex(ignoreGameObjects, gameObject => gameObject == obj.gameObject) < 0;
		_pausingMonoBehaviours = Array.FindAll(transform.GetComponentsInChildren<MonoBehaviour>(), monoBehaviourPredicate);
		foreach (var monoBehaviour in _pausingMonoBehaviours){
			monoBehaviour.enabled = false;
		}

	}

	/// <summary>
	/// 再開
	/// </summary>
	void Resume ()
	{
		// Rigidbodyの再開
		for (int i = 0; i < _pausingRigidbodies.Length; i++){
			_pausingRigidbodies[i].WakeUp();
			_pausingRigidbodies[i].velocity = _rigidbodyVelocities[i].velocity;
			_pausingRigidbodies[i].angularVelocity = _rigidbodyVelocities[i].angularVeloccity;
		}

		// MonoBehaviourの再開
		foreach (var monoBehaviour in _pausingMonoBehaviours){
			monoBehaviour.enabled = true;
		}
	}
}