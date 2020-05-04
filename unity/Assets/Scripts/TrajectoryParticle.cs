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

public class TrajectoryParticle : MonoBehaviour
{
    private Trajectory mTrajectory;

    private bool mFollowing = true;
    public void SetFollowing(bool b)
    {
        mFollowing = b;
        ResetParticlePhysics(mFollowing);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mTrajectory == null)
            return;

        if (mFollowing)
            UpdateFollowing();
    }

    public void Setup(Trajectory traj)
    {
        mTrajectory = traj;
    }

    //If following, physics need to be off; and vice versa
    private void ResetParticlePhysics(bool following)
    {
        if (following)
            DisableParticlePhysics();
        else
            EnableParticlePhysics();
    }

    private void EnableParticlePhysics()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
            return;
        rb.isKinematic = false;
    }

    private void DisableParticlePhysics()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
            return;

        rb.isKinematic = true;
        //rb.useGravity = false;
    }

    private void UpdateFollowing()
    {
        Vector3 p = mTrajectory.GetCurrentPoint();
        Vector3 q = transform.position;
        transform.position = Vector3.Lerp(p, q, 0.9f);
    }
}
