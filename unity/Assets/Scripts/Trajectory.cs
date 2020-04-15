using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    private GameObject mParticle;
    public GameObject GetParticle() { return mParticle; }
    private LypsBoss mBoss;

    private int mN = -1;     //which trajectory am I?
    private float mRadius = -1f;
    private float mDelta = 1f;
    private float mPhase = -1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mN < 0)
        {
            Debug.LogError("Trajectory with negative Nth");
            return;
        }

        UpdateDot();
    }

    public void Setup(int nth, LypsBoss boss, GameObject part)
    {
        mN = nth;
        mBoss = boss;
        mParticle = part;

        mDelta = mBoss.GetTrajectoryDelta();
        mPhase = mBoss.GetPhase();
        mRadius = mBoss.GetTrajectoryRadius();
    }


    //bgTODO refactor this into a utils class; same in LypsBoss
    // Local point wth is centre origin, forward is +Z
    // t is in radians [0,2PI)
    static Vector3 GetLocalPointFromCircleParam(float radius, float t)
    {
        float x = radius * Mathf.Cos(t);
        float y = radius * Mathf.Sin(t);
        float z = 0f;

        Vector3 p = new Vector3(x, y, z);
        return p;
    }


    private void UpdateDot()
    {
        float param = mBoss.GetParam();
        float phaseDiff = mPhase * mDelta * (float)mN;
        float t = param + phaseDiff;
        Vector3 p = GetLocalPointFromCircleParam(mRadius, t);
        p = p + transform.position;
        
        if (mParticle != null)
        {
            mParticle.transform.position = p;
        }

    }
}
