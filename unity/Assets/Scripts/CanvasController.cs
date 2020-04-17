using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    private TMPro.TMP_InputField mTMPInputFieldNCircles;
    private TMPro.TMP_InputField mTMPInputFieldPhase;
    private TMPro.TMP_InputField mTMPInputFieldSize;


    private EllipseGame mGame;
    public void SetGame(EllipseGame g)
    {
        mGame = g;
        UpdateFields();
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("CanvasController");
        SetupFields();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetupFields()
    {
        SetupInputFields();
    }

    private void SetupInputFields()
    {
        TMPro.TMP_InputField[] inputFields = transform.GetComponentsInChildren<TMPro.TMP_InputField>();
        foreach (TMPro.TMP_InputField tmp_if in inputFields)
        {
            if (tmp_if.name == "InputFieldNCircles")
                mTMPInputFieldNCircles = tmp_if;
            else if (tmp_if.name == "InputFieldPhase")
                mTMPInputFieldPhase = tmp_if;
            else if (tmp_if.name == "InputFieldSize")
                mTMPInputFieldSize = tmp_if;
        }

        if (mTMPInputFieldNCircles == null)
            Debug.LogWarning("TMP input field NCircles NOT FOUND!!!");
        if (mTMPInputFieldPhase == null)
            Debug.LogWarning("TMP input field Phase NOT FOUND!!!");
        if (mTMPInputFieldSize == null)
            Debug.LogWarning("TMP input field Size NOT FOUND!!!");
    }


    private void UpdateFields()
    {
        UpdateInputFieldNCircles();
        UpdateInputFieldPhase();
        UpdateInputFieldSize();
    }

    private void UpdateInputFieldNCircles()
    {
        if (mTMPInputFieldNCircles == null)
            return;
        int val = mGame.mNumberTrajectories;
        mTMPInputFieldNCircles.text = val.ToString();
    }

    private void UpdateInputFieldPhase()
    {
        if (mTMPInputFieldPhase == null)
            return;
        float val = mGame.mPhase;
        mTMPInputFieldPhase.text = val.ToString();
    }

    private void UpdateInputFieldSize()
    {
        if (mTMPInputFieldSize == null)
            return;
        float val = mGame.mTrajectoryRadius;
        mTMPInputFieldSize.text = val.ToString();
    }


    // event handlers


    // N Circles / trajectories

    public void NCirclesOnEndEditHandler(string str)
    {
        string inStr = mTMPInputFieldNCircles.text;
        if (int.TryParse(inStr, out int outVal))
            mGame.SetNTrajectories(outVal);
    }

    public void NCirclesButtonUpHandler()
    {
        string inStr = mTMPInputFieldNCircles.text;
        if (int.TryParse(inStr, out int outVal))
        {
            int newVal = ++outVal;
            mTMPInputFieldNCircles.text = newVal.ToString();

            string dum = "";
            NCirclesOnEndEditHandler(dum);
        }
    }

    public void NCirclesButtonDownHandler()
    {
        string inStr = mTMPInputFieldNCircles.text;
        if (int.TryParse(inStr, out int outVal))
        {
            int newVal = --outVal;
            mTMPInputFieldNCircles.text = newVal.ToString();

            string dum = "";
            NCirclesOnEndEditHandler(dum);
        }
    }



    // Phase

    public void PhaseOnEndEditHandler(string str)
    {
        string inStr = mTMPInputFieldPhase.text;
        if (float.TryParse(inStr, out float outVal))
            mGame.SetPhase(outVal);
    }

    public void PhaseButtonUpHandler()
    {
        string inStr = mTMPInputFieldPhase.text;
        if (float.TryParse(inStr, out float outVal))
        {
            float newVal = ++outVal;
            mTMPInputFieldPhase.text = newVal.ToString();

            string dum = "";
            PhaseOnEndEditHandler(dum);
        }
    }

    public void PhaseButtonDownHandler()
    {
        string inStr = mTMPInputFieldPhase.text;
        if (float.TryParse(inStr, out float outVal))
        {
            float newVal = --outVal;
            mTMPInputFieldPhase.text = newVal.ToString();

            string dum = "";
            PhaseOnEndEditHandler(dum);
        }
    }



    // Size

    public void SizeOnEndEditHandler(string str)
    {
        string inStr = mTMPInputFieldSize.text;
        if (float.TryParse(inStr, out float outVal))
            mGame.SetSize(outVal);
    }

    public void SizeButtonUpHandler()
    {
        string inStr = mTMPInputFieldSize.text;
        if (float.TryParse(inStr, out float outVal))
        {
            float newVal = ++outVal;
            mTMPInputFieldSize.text = newVal.ToString();

            string dum = "";
            SizeOnEndEditHandler(dum);
        }
    }

    public void SizeButtonDownHandler()
    {
        string inStr = mTMPInputFieldSize.text;
        if (float.TryParse(inStr, out float outVal))
        {
            float newVal = --outVal;
            mTMPInputFieldSize.text = newVal.ToString();

            string dum = "";
            SizeOnEndEditHandler(dum);
        }
    }

    public void NextParticleButtonHandler()
    {
        mGame.SetNextParticle();
    }

    public void ToggleDrawTrajectoriesButtonHandler()
    {
        mGame.ToggleDrawTrajectories();
    }


    // Right Panel

    // Quit Button
    
    public void PauseGoButtonHandler()
    {
        mGame.TogglePlayPause();
    }


    public void QuitButtonHandler()
    {
        mGame.QuitGame();
    }

}
