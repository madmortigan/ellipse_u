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

public class LypsBoss : MonoBehaviour
{
    public Material mShapeLineMaterial;
    private bool mDrawShapeLine = true;
    public void SetDrawShapeLine(bool b)
    {
        mDrawShapeLine = b;
        UpdateShapeLineVisibility(b);        
    }

    private float mMajorRadius = 200f;
    private float mTrajectoryRadius = 100f;
    private int mNumberTrajectories = 4;
    private float mPhase = -1f;
    private bool SetParameters(float majorR, float trajR, int n, float ph,
        float speed)
    {
        mMajorRadius = majorR;
        mTrajectoryRadius = trajR;
        mNumberTrajectories = n;
        mPhase = ph;
        mDelta = speed;
        return CheckParameters();
    }
    private bool CheckParameters()
    {
        if (mMajorRadius <= 0f)
            return false;
        if (mNumberTrajectories < 1)
            return false;
        return true;
    }

    public float GetPhase() { return mPhase; }
    public float GetTrajectoryDelta()
    {
        float deltaT = 2f * Mathf.PI / (float)mNumberTrajectories;
        return deltaT;
    }
    public float GetTrajectoryRadius() { return mTrajectoryRadius; }

    public GameObject mTrajectoryPrefab;
    public GameObject mDebugAnchor1Prefab;

    private GameObject mParticlePrefab;
    public void SetParticlePrefab(GameObject partPref) { mParticlePrefab = partPref; }

    private Trajectory[] mTrajectories;


    private float mCurrentParam = -1f;    //animation param ticker
    public float GetParam() { return mCurrentParam; }
    public void SetParam(float p) { mCurrentParam = p; }

    private float mDelta = 0.2f;          //animaton speed
    private float GetDelta() { return mDelta; }

    public int GetNumTrajectories() { return mNumberTrajectories; }

    private bool mPlay = true;      //play/pause flag; true if we're playing. 
    public bool TogglePlayPause()
    {
        mPlay = !mPlay;
        return mPlay;
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        UpdateParam();
        UpdateShapeLine();
    }

    public void SetDrawTrajectories(bool b)
    {
        foreach (Trajectory trj in mTrajectories)
            trj.SetTrajectoryDraw(b);
    }

    private void UpdateShapeLineVisibility(bool b)
    {
        LineRenderer liner = gameObject.GetComponent<LineRenderer>();
        if (liner == null)
            return;
        liner.enabled = b;
    }



    void OnDestroy()
    {
        DestroyTrajectories();
    }

        private void DestroyTrajectories()
    {
        foreach (Trajectory traj in mTrajectories)
        {
            DestroyTrajectory(traj);
        }
    }

    private void DestroyTrajectory(Trajectory traj)
    {
        GameObject trajgo = traj.gameObject;
        GameObject part = traj.GetParticle();
        Destroy(part);
        Destroy(trajgo);
    }

    private void UpdateParam()
    {
        if (!mPlay)
            return;

        float dt = GetDelta();
        float udt = dt * Time.deltaTime;
        mCurrentParam += udt;
        if (mCurrentParam >= 2f * Mathf.PI)
            mCurrentParam = mCurrentParam - 2f * Mathf.PI;
    }
    

    private void UpdateShapeLine()
    {
        if (!mDrawShapeLine)
            return;
        LineRenderer liner = gameObject.GetComponent<LineRenderer>();
        if (liner == null)
            return;
        if (liner.positionCount != mTrajectories.Length)
            return;

        for (int i=0; i<liner.positionCount; ++i)
        {
            Vector3 p = mTrajectories[i].GetCurrentPoint();
            liner.SetPosition(i, p);    //not efficient to set positions individual!
        }
    }


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



    public void Setup(float majorRadius, float trajRadius, int ntraj, 
        float phase,float speed)
    {
        if (!SetParameters(majorRadius, trajRadius, ntraj, phase, speed))
        {
            Debug.LogError("LypsBoss: Failed to set parameters");
            return;
        }

        if (mTrajectoryPrefab == null)
        {
            Debug.LogError("LypsBoss: Trajectory Prefab is null");
            return;
        }

        mTrajectories = new Trajectory[mNumberTrajectories];

        float deltaT = GetTrajectoryDelta();
        for (int i=0; i<mNumberTrajectories; ++i)
        {
            float t = i * deltaT;
            Vector3 cp = GetLocalPointFromCircleParam(mMajorRadius, t);

            GameObject trajInst = GameObject.Instantiate(mTrajectoryPrefab, cp, Quaternion.identity, transform);
            Trajectory traj = trajInst.GetComponent<Trajectory>();
            if (traj == null)
            {
                Debug.LogError("LypsBoss: No Trajectory component");
                return;
            }

            GameObject partInst  = null;
            if (mParticlePrefab != null)
                partInst = GameObject.Instantiate(mParticlePrefab);
  
            traj.Setup(i, this, partInst);
            mTrajectories[i] = traj;
        }

        CreateShapeLineRenderer();

        mCurrentParam = 0f;
    }

    public void SwitchParticlesOnOff(bool b)
    {
        if (mTrajectories == null)
            return;
        foreach(Trajectory trj in mTrajectories)
        {
            trj.SwitchParticleOnOff(b);
        }
    }

    public void SwitchAttachParticlesOnOff(bool b)
    {
        if (mTrajectories == null)
            return;
        foreach (Trajectory trj in mTrajectories)
        {
            trj.SwitchAttachParticleOnOff(b);
        }
    }

    private void CreateShapeLineRenderer()
    {
        LineRenderer lren = GetComponent<LineRenderer>();
        if (lren != null)
            Destroy(lren);

        //if (mBoss.GetNumTrajectories() > 1000)
        //    return;

        gameObject.AddComponent<LineRenderer>();
        lren = gameObject.GetComponent<LineRenderer>();

        Vector3[] pts = CreatePoints();
        if (pts == null)
            return;
        lren.positionCount = pts.Length;
        lren.SetPositions(pts);
        lren.useWorldSpace = false;
        lren.loop = true;

        if (mShapeLineMaterial != null)
            lren.material = mShapeLineMaterial;
    }

    private Vector3[] CreatePoints()
    {
        if (mTrajectories == null)
            return null;
        Vector3[] pts = new Vector3[mTrajectories.Length];
        for(int i=0; i<mTrajectories.Length; ++i)
        {
            Vector3 p = mTrajectories[i].GetCurrentPoint();
            pts[i] = p;
        }
        return pts;
    }


    public void ColorSpread(Material mat)
    {
        if (mTrajectories == null || mTrajectories.Length < 1)
            return;

        mTrajectories[0].SetColourSpreading(mat);   //bgDEBUG rename!
    }

    public void ResetSpreading()
    {
        if (mTrajectories == null || mTrajectories.Length < 1)
            return;

        for (int i = 0; i < mTrajectories.Length; ++i)
        {
            mTrajectories[i].ResetSpreading();
        }

    }

}
