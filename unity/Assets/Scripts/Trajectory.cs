/************************************************************************************
Copyright(c) 2020 Bob Gouzinis (BIG BAD BOB GAMES)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
**************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    public Material mLineMaterial;

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

        SetupParticle(part);

        mDelta = mBoss.GetTrajectoryDelta();
        mPhase = mBoss.GetPhase();
        mRadius = mBoss.GetTrajectoryRadius();

        CreateMyLineRenderer();
    }

    private void SetupParticle(GameObject part)
    {
        mParticle = part;
        TrajectoryParticle tPart = part.AddComponent<TrajectoryParticle>();
        tPart.Setup(this);
    }

    public void SetTrajectoryDraw(bool b)
    {
        LineRenderer lineRen = gameObject.GetComponent<LineRenderer>();
        if (lineRen == null)
            return;
        lineRen.enabled = b;
    }


    //Create a LineRenderer component to visualise the circular trajectory
    private void CreateMyLineRenderer()
    {
        LineRenderer lren = GetComponent<LineRenderer>();
        if (lren != null)
            Destroy(lren);

        if (mBoss.GetNumTrajectories() > 1000)
            return;


        gameObject.AddComponent<LineRenderer>();
        lren = gameObject.GetComponent<LineRenderer>();

        Vector3[] pts = CreatePoints();
        lren.positionCount = pts.Length;
        lren.SetPositions(pts);
        lren.useWorldSpace = false;
        lren.loop = true;

        if (mLineMaterial != null)
            lren.material = mLineMaterial;
    }

    private Vector3[] CreatePoints()
    {
        int npoints = 20;
        Vector3[] pts = new Vector3[npoints];

        float delta = 2f * Mathf.PI / npoints;
        for (int i = 0; i < npoints; ++i)
        {
            float t = i * delta;
            Vector3 p = GetLocalPointFromCircleParam(mRadius, t);
            pts[i] = p;
        }
        return pts;
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


    public Vector3 GetCurrentPoint()
    {
        float param = mBoss.GetParam();
        float phaseDiff = mPhase * mDelta * (float)mN;
        float t = param + phaseDiff;
        Vector3 p = GetLocalPointFromCircleParam(mRadius, t);
        p = p + transform.position;
        return p;
    }

    private void UpdateDot()
    {
        if (mParticle == null || !mParticle.activeSelf)
            return;

        //Vector3 p = GetCurrentPoint();
        //mParticle.transform.position = p;
    }

    public void SwitchParticleOnOff(bool b)
    {
        if (mParticle == null)
            return;
        mParticle.SetActive(b);
    }

    public void SwitchAttachParticleOnOff(bool b)
    {
        if (mParticle == null)
            return;
        TrajectoryParticle tpe = mParticle.GetComponent<TrajectoryParticle>();
        if (tpe == null)
            return;
        tpe.SetFollowing(b);
    }

}