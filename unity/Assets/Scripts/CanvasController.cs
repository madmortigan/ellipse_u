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

public class CanvasController : MonoBehaviour
{
    public GameObject mAboutCanvas;

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


    private void LaunchAboutBox()
    {
        if (mAboutCanvas == null)
            return;
        mAboutCanvas.SetActive(true);
    }

    private void CloseAboutBox()
    {
        if (mAboutCanvas == null)
            return;

        mAboutCanvas.SetActive(false);
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

    public void SwitchDrawLinesButtonHandler()
    {
        mGame.SwitchDrawLines();
    }


    // Right Panel

    // Quit Button
    
    public void PauseGoButtonHandler()
    {
        mGame.TogglePlayPause();
    }

    public void AttachParticlesButtonHandler()
    {
        mGame.SwitchAttachParticles();
    }


    public void QuitButtonHandler()
    {
        mGame.QuitGame();
    }

    public void AboutButtonHandler()
    {
        if (mAboutCanvas == null)
            return;
        if (mAboutCanvas.activeSelf)
            CloseAboutBox();
        else
            LaunchAboutBox();
    }

    //button in AboutBox canvas.
    public void CloseAboutButtonHandler()
    {
        CloseAboutBox();
    }

}
