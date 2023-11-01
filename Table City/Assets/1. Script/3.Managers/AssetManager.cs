using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class AssetManager : PunManagerBase
{
	private PhotonView _pv;

	/// <summary> Ʈ�� �̵��� ��ġ </summary>
	private Vector3 _targetPos;

	/// <summary> �����̳� ���ڿ� �ű� �ڿ��� </summary>
	private int[] _Assets = new int[12];

    public override void Init()
    {
        _pv = Managers.Network.GetPhotonView();
    }

    /// <summary> ���� ������ �޾ƿ��� </summary>
    /// <returns>������ position</returns>
    public Vector3 GetTargetPosition() { return _targetPos; }

	/// <summary> �ű� �ڿ� ���� </summary>
	/// <param name="_assetType">�ڿ� Ÿ��</param>
	/// <param name="_count">�ڿ� ����</param>
	public void SetAssetData(Define.AssetData _assetType, int _count) => _Assets[(int)_assetType] += _count;

	/// <summary> Ʈ���� �̵��� ���� ��ġ ��ȯ </summary>
	/// <param name="_factoryNum">���� ��ȣ</param>
	/// <returns>���� Position</returns>
	public Vector3 GetTargetPosition(int _factoryNum) { return Managers.Game.factoryScript[(Define.AssetData)_factoryNum].transform.position; }

	#region Syncron
	/// <summary> Ʈ���� �̵��� ���� ��ġ ����ȭ ���� </summary>
	/// <param name="_pos">������ position</param>
	public void SyncTargetPosition(Vector3 _pos) { _pv.RPC("SetTargetPosition", RpcTarget.All, _pos); }

	/// <summary> ���忡 �� �ڿ� ����ȭ </summary>
	/// <param name="_factoryType">�����͸� ����ȭ �� ����</param>
	public void SyncFactroyData(Define.AssetData _factoryType)
	{ 
		_pv.RPC("InputFactoryAssetData", RpcTarget.All, _factoryType, _Assets);
		_Assets = new int[12];
	}

	/// <summary> ���忡�� ������ �ڿ� ����ȭ  </summary>
	/// <param name="_data">���� ������</param>
	/// <param name="_createCount">���忡�� ������ ����</param>
	public void SyncFactroyCreateAsset(Define.AssetData _data, int _createCount)
	{
		_pv.RPC("OutputFactoryAssetData", RpcTarget.All, _data, _createCount);
	}
	#endregion

	#region PunRPC
	/// <summary> �Էµ� �������� ����� �ڿ��� �̵� </summary>
	/// <param name="_factoryType">�ڿ��� �̵���ų ����</param>
	/// <param name="_Assets">����� �ڿ�</param>
	[PunRPC]
	private void InputFactoryAssetData(Define.AssetData _factoryType, int[] _Assets)
	{
		Managers.Game.PlayInputFactoryItem(_factoryType, _Assets);
	}

	/// <summary> ���忡�� ������ �ڿ��� ���� </summary>
	/// <param name="_data">���� ������</param>
	/// <param name="_createCount">���忡�� ������ �ڿ� ����</param>
	[PunRPC]
	private void OutputFactoryAssetData(Define.AssetData _data, int _createCount)
	{
		Managers.Game.asset[(int)_data] += _createCount;
	}
	#endregion

}
