using UnityEngine;
using Photon.Pun;

public class Throw : MonoBehaviour
{
    private Vector3 m_TargetPosition { get; set; }

    [SerializeField]
    private GameObject fx,model;
    [SerializeField]
    private float m_Speed = 10;
    [SerializeField]
    private float m_HeightArc = 1;
    private Vector3 m_StartPosition;

    [SerializeField]
    private Define.AssetData itemType;

    [SerializeField]
    private bool itemShot = false;

    private void Start()
    {
        m_StartPosition = transform.position;

        if (itemShot == true) 
            m_TargetPosition = Managers.system._workbenchPoints[(int)itemType];

        fx = transform.GetChild(1).gameObject;
        model = transform.GetChild(0).gameObject;

    }

    void Update()
    {
        float x0 = m_StartPosition.x;
        float x1 = m_TargetPosition.x;
        float distance = x1 - x0;
        float nextX = Mathf.MoveTowards(transform.position.x, x1, m_Speed * Time.deltaTime);
        float baseY = Mathf.Lerp(m_StartPosition.y, m_TargetPosition.y, (nextX - x0) / distance);
        float arc = m_HeightArc * (nextX - x0) * (nextX - x1) / (-0.25f * distance * distance);
        float zOffset = (m_TargetPosition.z - m_StartPosition.z) * (nextX - x0) / distance;
        Vector3 nextPosition = new Vector3(nextX, baseY + arc, m_StartPosition.z + zOffset);

        transform.rotation = LookAt3D(nextPosition - transform.position);
        transform.position = nextPosition;

        if (nextPosition == m_TargetPosition)
            Arrived();
    }

    /// <summary> 트럭이 이동할 목표지점 설정 </summary>
    /// <param name="_pos">목표 Position</param>
    public void SetTargetPosition(Vector3 _pos) => m_TargetPosition = _pos;

    void Arrived()
    {
        fx.SetActive(true);
        model.SetActive(false);
        Managers.system.ActionTimer(2,()=>
        {
            model.SetActive(true);
            fx.SetActive(false); 
            Managers.instantiate.AddPooling(gameObject); 
        });
    }

    Quaternion LookAt3D(Vector3 forward)
    {
        return Quaternion.LookRotation(forward);
    }
}