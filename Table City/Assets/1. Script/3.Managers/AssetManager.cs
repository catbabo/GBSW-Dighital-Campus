using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class AssetManager : PunManagerBase
{
	private PhotonView _pv;

	/// <summary> 트럭 이동할 위치 </summary>
	private Vector3 _targetPos;

	/// <summary> 공장이나 상자에 옮길 자원들 </summary>
	private int[] _Assets = new int[12];

    public override void Init()
    {
        _pv = Managers.Network.GetPhotonView();
    }

    /// <summary> 공장 포지션 받아오기 </summary>
    /// <returns>공장의 position</returns>
    public Vector3 GetTargetPosition() { return _targetPos; }

	/// <summary> 옮길 자원 저장 </summary>
	/// <param name="_assetType">자원 타입</param>
	/// <param name="_count">자원 개수</param>
	public void SetAssetData(Define.AssetData _assetType, int _count) => _Assets[(int)_assetType] += _count;

	/// <summary> 트럭이 이동할 공장 위치 반환 </summary>
	/// <param name="_factoryNum">공장 번호</param>
	/// <returns>공장 Position</returns>
	public Vector3 GetTargetPosition(int _factoryNum) { return Managers.Game.factoryScript[(Define.AssetData)_factoryNum].transform.position; }

	#region Syncron
	/// <summary> 트럭이 이동할 공장 위치 동기화 실행 </summary>
	/// <param name="_pos">공장의 position</param>
	public void SyncTargetPosition(Vector3 _pos) { _pv.RPC("SetTargetPosition", RpcTarget.All, _pos); }

	/// <summary> 공장에 들어간 자원 동기화 </summary>
	/// <param name="_factoryType">데이터를 동기화 할 공장</param>
	public void SyncFactroyData(Define.AssetData _factoryType)
	{ 
		_pv.RPC("InputFactoryAssetData", RpcTarget.All, _factoryType, _Assets);
		_Assets = new int[12];
	}

	/// <summary> 공장에서 생성한 자원 동기화  </summary>
	/// <param name="_data">공장 데이터</param>
	/// <param name="_createCount">공장에서 생성한 개수</param>
	public void SyncFactroyCreateAsset(Define.AssetData _data, int _createCount)
	{
		_pv.RPC("OutputFactoryAssetData", RpcTarget.All, _data, _createCount);
	}
	#endregion

	#region PunRPC
	/// <summary> 입력된 공장으로 저장된 자원들 이동 </summary>
	/// <param name="_factoryType">자원을 이동시킬 공장</param>
	/// <param name="_Assets">저장된 자원</param>
	[PunRPC]
	private void InputFactoryAssetData(Define.AssetData _factoryType, int[] _Assets)
	{
		Managers.Game.PlayInputFactoryItem(_factoryType, _Assets);
	}

	/// <summary> 공장에서 생성한 자원들 저장 </summary>
	/// <param name="_data">공장 데이터</param>
	/// <param name="_createCount">공장에서 생성한 자원 개수</param>
	[PunRPC]
	private void OutputFactoryAssetData(Define.AssetData _data, int _createCount)
	{
		Managers.Game.asset[(int)_data] += _createCount;
	}
	#endregion

}
