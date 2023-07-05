using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class AssetManager : MonoBehaviourPunCallbacks
{

	#region Singleton
	public static AssetManager _asset = null;

	private void Awake()
	{
		if (_asset == null)
		{
			_asset = this;
		}
	}
	#endregion

	/// <summary> ���� �� </summary>
	private PhotonView _pv;

	/// <summary> Ʈ�� �̵��� ��ġ </summary>
	private Vector3 _targetPos;

	/// <summary> �����̳� ���ڿ� �ű� �ڿ��� </summary>
	private int[] _Assets = new int[12];

	private void Start()
	{
		_pv = gameObject.GetComponent<PhotonView>();
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
	public Vector3 GetTargetPosition(int _factoryNum) { return Managers.system.factoryScript[(Define.AssetData)_factoryNum].transform.position; }

	#region Syncron
	/// <summary> Ʈ���� �̵��� ���� ��ġ ����ȭ ���� </summary>
	/// <param name="_pos">������ position</param>
	public void SyncTargetPosition(Vector3 _pos) { _pv.RPC("SetTargetPosition", RpcTarget.All, _pos); }

	public void SyncFactroyData(Define.AssetData _factoryType)
	{ 
		_pv.RPC("InputFactoryAssets", RpcTarget.All, _factoryType, _Assets);
		_Assets = new int[12];
	}
	#endregion

	#region PunRPC
	/// <summary> �Էµ� �������� ����� �ڿ��� �̵� </summary>
	/// <param name="_factoryType">�ڿ��� �̵���ų ����</param>
	/// <param name="_Assets">����� �ڿ�</param>
	[PunRPC]
	private void InputFactoryAssets(Define.AssetData _factoryType, int[] _Assets)
	{
		foreach (Define.AssetData _assetData in _Assets)
		{
			Managers.system.InputFactoryItem(_factoryType, _assetData, _Assets[(int)_assetData]);
		}
	}
	#endregion

}
