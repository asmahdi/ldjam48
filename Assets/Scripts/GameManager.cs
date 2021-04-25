using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    [Header("Target Settings")]
    public bool autoGenerate;
    public GameObject target;
    public GameObject captureObject;
    public GameObject captureText;

    [Header("Map Settings")]
    public GameObject periscopeMap;
    public float lookSpeed, divingSpeed;
    public Slider horizontalControl, verticalControl;
    public TMP_Text lookAngle, depth;
    public Animator pingRadar;
    public GameObject[] pingAudios;
    public TMP_Text noteAngleText, noteDepthText;

    [Header("Interface Settings")]
    public SpriteRenderer fuel;
    public SpriteRenderer oxygen, food;
    public float maxOffset;
    public GameObject endLevel;

    [Header("Level Settings")]
    public float fuelUseRate;
    public float oxygenUseRate;
    public float foodUseRate;
    public Vector3 targetPosition;

    [Header("Repair Setting")]
    public GameObject[] cracks;
    public GameObject[] patches;
 




    private Vector3 targetA_Position;
    private Vector3 mapDimention = new Vector3(360,500,100);
    private float fuelAmount, oxygenAmount, foodAmount;

    private bool captured;


    private void Start()
    {
        if (autoGenerate)
        {
            targetA_Position = new Vector3(Random.Range(0, mapDimention.x), Random.Range(-mapDimention.y, 0));
            Instantiate(target, targetA_Position, Quaternion.identity, periscopeMap.transform);
        }
        else
        {
            target.transform.position = new Vector3(-targetPosition.x -3 , -targetPosition.y + 4, targetPosition.z);
        }

        fuelAmount = oxygenAmount = foodAmount = 1;
        WriteNote();



        InvokeRepeating("NeedRepair", 10f, 20f);
    }

    private void Update()
    {
        MoveHorizontally();
        MoveVerically();
        ControlPing();
        DecreaseResources();

        if (CheckIfLevelEnd())
        {
            endLevel.SetActive(true);
        }

        
    }

    public void MoveHorizontally()
    {
        if (periscopeMap.transform.position.x > 360)
        {
            periscopeMap.transform.position = new Vector3(0, periscopeMap.transform.position.y, periscopeMap.transform.position.z);
        }

        if (periscopeMap.transform.position.x < 0)
        {
            periscopeMap.transform.position = new Vector3(360, periscopeMap.transform.position.y, periscopeMap.transform.position.z);
        }

        periscopeMap.transform.Translate(-lookSpeed * Mathf.Pow(horizontalControl.value,3) * Time.deltaTime, 0, 0);

        lookAngle.text = (periscopeMap.transform.position.x ).ToString("F1");
    }

    public void MoveVerically()
    {
        if ( Mathf.CeilToInt(periscopeMap.transform.position.y)  > 0 && periscopeMap.transform.position.y < 500)
        {
            periscopeMap.transform.Translate(0, -verticalControl.value * divingSpeed * Time.deltaTime, 0);
            depth.text = (periscopeMap.transform.position.y).ToString("F1");
        }

        if ( periscopeMap.transform.position.y < 0 )
        {
            periscopeMap.transform.Translate(0, 1f, 0);
        }

        
    }


    public void CaputureTarget()
    {


        if (Vector3.Distance(Vector3.zero, target.transform.position) < 3)
        {

            captured = true;

            Instantiate(captureObject, target.transform, false);

            Invoke("DissolveTarget", 2.0f);   
            
        }
    }



    private void ControlPing()
    {
        float distance;
        distance =  Mathf.Abs(target.transform.position.y);
        print(Vector3.Distance(Vector3.zero, target.transform.position));

        

        if (distance > 100 )
        {
            pingRadar.SetFloat("ping_speed", .5f);

            pingAudios[0].SetActive(true);
            pingAudios[1].SetActive(false);
            pingAudios[2].SetActive(false);
            pingAudios[3].SetActive(false);

        }
        else if (distance  > 50)
        {
            pingRadar.SetFloat("ping_speed", 1);

            pingAudios[1].SetActive(true);
            pingAudios[0].SetActive(false);
            pingAudios[2].SetActive(false);
            pingAudios[3].SetActive(false);

        }
        else if (distance > 10)
        {
            pingRadar.SetFloat("ping_speed", 2);

            pingAudios[2].SetActive(true);
            pingAudios[1].SetActive(false);
            pingAudios[0].SetActive(false);
            pingAudios[3].SetActive(false);

        }
        else if (distance > 4)
        {
            pingRadar.SetFloat("ping_speed", 3);

            pingAudios[3].SetActive(true);
            pingAudios[1].SetActive(false);
            pingAudios[2].SetActive(false);
            pingAudios[0].SetActive(false);

        }



    }




    private void DecreaseResources()
    {
        fuelAmount -= fuelUseRate/1000 * Time.deltaTime;
        fuel.size = new Vector2(fuel.size.x, Mathf.Lerp(0, maxOffset, fuelAmount));

        oxygenAmount -= oxygenUseRate/1000 * Time.deltaTime;
        oxygen.size = new Vector2(fuel.size.x, Mathf.Lerp(0, maxOffset, oxygenAmount));

        foodAmount -= foodUseRate/1000 * Time.deltaTime;
        food.size = new Vector2(fuel.size.x, Mathf.Lerp(0, maxOffset, foodAmount));
    }



    public void WriteNote()
    {
        float startAngle, endAngle;
        startAngle = targetPosition.x - 50;
        endAngle = targetPosition.x + 50;

        startAngle = CorrectAngle(startAngle);
        endAngle = CorrectAngle(endAngle);

        noteAngleText.text = startAngle.ToString() + " to "+ endAngle.ToString() + "DEGREE";
        noteDepthText.text = (targetPosition.y - 70).ToString() + " to " + (targetPosition.y + 70).ToString() + "M";

    }


    private float CorrectAngle(float angle)
    {
        if (angle < 0 )
        {
            return 360 + angle;
        }
        if (angle > 360)
        {
            return angle - 360;
        }
        else
        {
            return angle;
        }
    }

    private void DissolveTarget()
    {
        target.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        captureText.SetActive(true);
    }



    private void DistributeCracks()
    {
        foreach (GameObject crack in cracks)
        {
            crack.transform.position = new Vector3(Random.Range(-6, 6), Random.Range(-3, 3), 0);
        }
    }


    private bool CheckIfLevelEnd()
    {
        if (captured && periscopeMap.transform.position.y > 3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    

 
}
