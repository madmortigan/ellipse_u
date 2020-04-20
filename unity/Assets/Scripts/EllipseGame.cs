using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum DrawLinesEnum
{
    None = 0,               //Keep as first
    Trajectories = 1,
    Shape = 2,
    All = 3                 //Keep as last
}

public class EllipseGame : MonoBehaviour
{
    public float mMajorRadius = 100;
    public float mTrajectoryRadius = 80;
    public int mNumberTrajectories = 4;
    public float mPhase = -1f;
    public float mSpeed = 1f;
    public GameObject mBossPrefab;
    public GameObject[] mParticlePrefab;
    public GameObject mCanvasGO;

    private LypsBoss mBoss;
    private CanvasController mCanvas;

    private int mCurrentParticleIndex = 0;
    private bool mParticlesOn = true;

    //private bool mDrawTrajectories = false;
    private DrawLinesEnum mDrawLinesMode = DrawLinesEnum.None;
    private bool GetDrawTrajectories()
    {
        bool doDraw = 
            mDrawLinesMode == DrawLinesEnum.All ||
            mDrawLinesMode == DrawLinesEnum.Trajectories;
        return doDraw;
    }
    private bool GetDrawShapeLine()
    {
        bool doDraw = 
            mDrawLinesMode == DrawLinesEnum.All ||
            mDrawLinesMode == DrawLinesEnum.Shape;
        return doDraw;
    }
    private void SetDrawLinesMode(DrawLinesEnum mode)
    {
        mDrawLinesMode = mode;
        mBoss.SetDrawShapeLine(GetDrawShapeLine());
        mBoss.SetDrawTrajectories(GetDrawTrajectories());
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("EllipseGame::Start");

        SetupCanvas();
        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetupCanvas()
    {
        //find and cache CanvasController
        if (mCanvasGO != null)
        {
            mCanvas = mCanvasGO.GetComponent<CanvasController>();
        }

        if (mCanvas != null)
            mCanvas.SetGame(this);
        else
            Debug.LogError("No Canvas found");

    }

    private void Setup()
    {
        if (mBossPrefab == null)
        {
            Debug.LogError("No LBoss prefab");
            return;
        }

        GameObject boss = GameObject.Instantiate(mBossPrefab);
        mBoss = boss.GetComponent<LypsBoss>();
        if (mBoss == null)
        {
            Debug.LogError("No LypsBoss component");
            return;
        }

        mBoss.SetParticlePrefab(mParticlePrefab[mCurrentParticleIndex]);
        mBoss.Setup(mMajorRadius, mTrajectoryRadius, mNumberTrajectories,
            mPhase, mSpeed);

        bool drawTrajectories = GetDrawTrajectories();
        mBoss.SetDrawTrajectories(drawTrajectories);

        bool drawShapeLine = GetDrawShapeLine();
        mBoss.SetDrawShapeLine(drawShapeLine);

        SetupCamera();
    }

    private void SetupCamera()
    {
        GameObject cgo = GameObject.FindGameObjectWithTag("MainCamera");
        if (cgo == null)
            return;

        Camera cam = cgo.GetComponent<Camera>();
        
        float fov = Mathf.Deg2Rad * cam.fieldOfView;
        float wid = 2f * (mMajorRadius + mTrajectoryRadius);

        // tan(fov) = wid/dist;
        float dist = 2f * wid / Mathf.Tan(fov);
        Vector3 p = mBoss.gameObject.transform.position;

        Vector3 dir = mBoss.gameObject.transform.forward;
        Vector3 pp = p - dist * dir;
        cam.transform.position = pp;
        cam.transform.LookAt(p);
    }

    private void Reset()
    {
        if (mBoss == null)
            return;
        float currParam = mBoss.GetParam();
        GameObject bossgo = mBoss.gameObject;
        Destroy(bossgo);
        mBoss = null;

        Setup();
        mBoss.SetParam(currParam);
        if (!mParticlesOn)
            SwitchParticlesOnOff(false);
    }

    private void SwitchParticlesOnOff(bool b)
    {
        mParticlesOn = b;
        if (mBoss == null)
            return;
        mBoss.SwitchParticlesOnOff(b);
    }

    public void SetNTrajectories(int n)
    {
        if (n < 1 || n>10000)
            return;

        mNumberTrajectories = n;
        Reset();
    }

    public void SetPhase(float p)
    {
        if (p < -9999f || p > 9999)
            return;
        mPhase = p;
        Reset();
    }

    public void SetSize(float s)
    {
        if (s < 0.1f || s > 1000f)  //bit arbitrary, bgtodo rationalise
            return;

        mTrajectoryRadius = s;
        Reset();
    }

    public void QuitGame()
    {
        Debug.Log("CALLING Applcation.QUIT");
        Application.Quit();
    }


    public void TogglePlayPause()
    {
        if (mBoss == null)
            return;
        mBoss.TogglePlayPause();
    }

    public void SetNextParticle()
    {
        if (mBoss == null)
            return;

        if (!mParticlesOn)
        {
            SwitchParticlesOnOff(true);
            return;
        }

        ++mCurrentParticleIndex;
        if (mCurrentParticleIndex >= mParticlePrefab.Length)
        {
            mCurrentParticleIndex = 0;
            mParticlesOn = false;
        }
        Reset();
    }

    public void SwitchDrawLines()
    {
        //SetDrawTrajectories(!mDrawTrajectories);
        int iMode = (int)mDrawLinesMode;
        ++iMode;
        int lastMode = (int)DrawLinesEnum.All;
        if (iMode > lastMode)
            iMode = 0;
        SetDrawLinesMode((DrawLinesEnum)iMode);
    }

}
