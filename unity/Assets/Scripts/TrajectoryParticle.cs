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

    private Collider mColliderPhys;         //non-trigger collider
    private void SwitchColliderPhys(bool b)
    {
        if (mColliderPhys!= null)
            mColliderPhys.enabled = b;
    }
    private Collider mColliderTrigger;     //trigger collider
    private void SwitchColliderTrigger(bool b)
    {
        if (mColliderTrigger != null)
            mColliderTrigger.enabled = b;
    }



    private bool mFollowing = true;
    public void SetFollowing(bool b)
    {
        mFollowing = b;
        ResetParticlePhysics(mFollowing);
    }


    Material mOriginalMaterial;
    void SetSpreadMaterial(Material mat)
    {
        if (mat == null)
            return;
        Renderer rend = GetComponent<Renderer>();
        if (rend == null)
            return;
        rend.material = mat;
    }

    public void SetColourSpreading(Material mat)
    {
        SetSpreadMaterial(mat);
    }
    public void ResetSpreading()
    {
        SetSpreadMaterial(mOriginalMaterial);    
    }


    // Start is called before the first frame update
    void Start()
    {
        StartSetupColliders();
        StartSetupMaterial();
    }

    private void StartSetupColliders()
    {
        Collider[] colls = GetComponents<Collider>();
        if (colls == null || colls.Length < 2)
            return;
        Physics.IgnoreCollision(colls[0], colls[1]);
        if (colls[0].isTrigger)
        {
            mColliderTrigger = colls[0];
            if (!colls[1].isTrigger)
                mColliderPhys = colls[1];
        }
        else if (colls[1].isTrigger)
        {
            mColliderTrigger = colls[1];
            if (!colls[0].isTrigger)
                mColliderPhys = colls[0];
        } 
    }

    private void StartSetupMaterial()
    {
        mOriginalMaterial = GetCurrentMaterial();
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
        transform.position = traj.GetCurrentPoint();
    }


    private Material GetCurrentMaterial()
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend == null)
            return null;
        Material mat = rend.material;
        if (mat == null)
            return null;
        return mat;
    }

    public Material GetSpreadMaterial()
    {
        Material curmat = GetCurrentMaterial();
        if (curmat == mOriginalMaterial)
            return null;
        return curmat;
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
        SwitchColliderPhys(true);
    }

    private void DisableParticlePhysics()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
            return;

        rb.isKinematic = true;
        SwitchColliderPhys(false);
    }

    private void UpdateFollowing()
    {
        Vector3 p = mTrajectory.GetCurrentPoint();
        Vector3 q = transform.position;
        transform.position = Vector3.Lerp(p, q, 0.9f);
    }


    private Material GetSpreadMaterial(GameObject go)
    {
        TrajectoryParticle tPart = go.GetComponent<TrajectoryParticle>();
        if (tPart == null)
            return null;
        return tPart.GetSpreadMaterial();
    }

    private void SpreadMaterial(GameObject other)
    {
        Material mat = GetSpreadMaterial(other);
        if (mat == null)
            return;
        SetSpreadMaterial(mat);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("On TRIGGER Enter:: SpreadColour");
        SpreadMaterial(other.gameObject);
    }



    private void OnCollisionEnter(Collision collision)
    {
    }

}
