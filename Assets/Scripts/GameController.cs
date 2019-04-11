using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public PlayerMobility player;
    public VCR vcr;
    bool replay = false;

    private static Vector3 SetZAxisToZero(Vector3 v)
    {
        return v + new Vector3(0, 0, -v.z);
    }

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMobility>();
        vcr = GameObject.Find("VCR").GetComponent<VCR>();

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space")) {
            //vcr.PrintMousePositions();
            vcr.StartReplay();
            replay = true;
        }

        Vector3 mousePosition;
        float inputWS;
        float inputAD;
        bool attack = false;

        if (replay) {
            if (vcr.IsNotLast()) {
                mousePosition = vcr.GetNextMousePosition();
                inputWS = vcr.GetNextInputWS();
                inputAD = vcr.GetNextInputAD();
                attack = vcr.GetNextAttack();
                vcr.IncrementNext();
            } else {
                vcr.ResetReplay();
                replay = false;
                if (Input.GetMouseButtonDown(0))
                {
                    attack = true;
                }
                inputWS = Input.GetAxis("Vertical");
                inputAD = Input.GetAxis("Horizontal");
                mousePosition = SetZAxisToZero(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
        } else {
            if (Input.GetMouseButtonDown(0))
            {
                attack = true;
            }
            inputWS = Input.GetAxis("Vertical");
            inputAD = Input.GetAxis("Horizontal");
            mousePosition = SetZAxisToZero(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        
        player.UpdateTurn(mousePosition, inputWS, inputAD, attack);
        if (!replay) {
            vcr.SaveEverything(mousePosition, inputWS, inputAD, attack);
        }   
    }
}
