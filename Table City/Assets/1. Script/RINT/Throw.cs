using UnityEngine;

public class Throw : MonoBehaviour
{
    public Transform m_Target;
    public float m_Speed = 10;
    public float m_HeightArc = 1;
    private Vector3 m_StartPosition;
    private bool m_IsStart;

    void Start()
    {
        m_StartPosition = transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_IsStart = true;
        }

        if (m_IsStart)
        {
            float x0 = m_StartPosition.x;
            float x1 = m_Target.position.x;
            float distance = x1 - x0;
            float nextX = Mathf.MoveTowards(transform.position.x, x1, m_Speed * Time.deltaTime);
            float baseY = Mathf.Lerp(m_StartPosition.y, m_Target.position.y, (nextX - x0) / distance);
            float arc = m_HeightArc * (nextX - x0) * (nextX - x1) / (-0.25f * distance * distance);
            float zOffset = (m_Target.position.z - m_StartPosition.z) * (nextX - x0) / distance;
            Vector3 nextPosition = new Vector3(nextX, baseY + arc, m_StartPosition.z + zOffset);

            transform.rotation = LookAt3D(nextPosition - transform.position);
            transform.position = nextPosition;

            if (nextPosition == m_Target.position)
                Arrived();
        }
    }

    void Arrived()
    {
        //Debug.Log("µµÂø");
    }

    Quaternion LookAt3D(Vector3 forward)
    {
        return Quaternion.LookRotation(forward);
    }
}