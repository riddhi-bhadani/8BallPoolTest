using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueBall : MonoBehaviour
{
    private const float THROW_FORCE = 0.8f;

    private Rigidbody m_RigidBody = null;
    private float m_TimeStart, m_TimeEnd = 0;
    private float m_TimeInterval = 0;

    private Vector2 m_StartPos, m_EndPos, m_Direction = Vector2.zero;

    private bool m_IsDragStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Touch touch = new Touch(); ;

        if (Input.touchCount <= 0)
        {
            return;
        }

        touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            Ray raycast = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit, Mathf.Infinity))
            {
                // detect swipe is began on CueBall or not
                if (raycastHit.collider.CompareTag("CueBall"))
                {
                    m_IsDragStarted = true;
                    m_TimeStart = Time.time;
                    m_StartPos = touch.position;
                }
            }
        }

        if (touch.phase == TouchPhase.Ended && m_IsDragStarted)
        {
            m_TimeEnd = Time.time;
            m_EndPos = touch.position;

            m_TimeInterval = m_TimeEnd - m_TimeStart;

            m_Direction = m_StartPos - m_EndPos;

            if (m_TimeInterval <= 0.1f)
            {
                m_TimeInterval = 0.1f;
            }
            Vector2 l_force = -m_Direction / m_TimeInterval * THROW_FORCE;

            //apply force in swipe direction to the CueBall
            m_RigidBody.AddForce(new Vector3(l_force.x, 0, l_force.y));
            m_IsDragStarted = false;
        }
    }
}
