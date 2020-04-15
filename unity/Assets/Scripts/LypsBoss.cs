﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LypsBoss : MonoBehaviour
{
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

    private float mDelta = 0.2f;          //animaton speed
    private float GetDelta() { return mDelta; }

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

        mCurrentParam = 0f;
    }

}