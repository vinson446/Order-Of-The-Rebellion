using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [Header("Debugger")]
    [SerializeField] float currentCamMoveSpeed;
    public bool isDefault;
    [SerializeField] bool focus1 = true;

    [Header("Cam Speed Settings")]
    [SerializeField] float initCamMoveSpeed;
    [SerializeField] float acceleration;

    [Header("Clamp Settings")]
    [SerializeField] float maxCamMoveSpeed;
    [SerializeField] float[] xLimits;
    [SerializeField] float[] zLimits;

    [Header("Inspector References")]
    [SerializeField] CinemachineVirtualCamera defaultCineCam;
    [SerializeField] CinemachineVirtualCamera focusCineCam1;
    [SerializeField] CinemachineVirtualCamera focusCineCam2;
    [SerializeField] CinemachineVirtualCamera tmpCam;

    CombatManager combatManager;

    private void Start()
    {
        TurnOnDefaultCam();

        combatManager = FindObjectOfType<CombatManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (defaultCineCam.gameObject.activeInHierarchy)
            DefaultCamMovement();
    }

    void DefaultCamMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // accelerate/decelerate camera
        if (x == 0 && z == 0)
        {
            currentCamMoveSpeed = initCamMoveSpeed;
        }
        else 
        {
            currentCamMoveSpeed += initCamMoveSpeed * acceleration * Time.deltaTime;
        }
        currentCamMoveSpeed = Mathf.Clamp(currentCamMoveSpeed, 0, maxCamMoveSpeed);
        defaultCineCam.transform.localPosition += new Vector3(x * currentCamMoveSpeed, 0, z * currentCamMoveSpeed);

        ClampCam();
    }

    void ClampCam()
    {
        Vector3 localPos = defaultCineCam.transform.position;
        defaultCineCam.transform.localPosition = new Vector3(Mathf.Clamp(localPos.x, xLimits[0], xLimits[1]), localPos.y,
            Mathf.Clamp(localPos.z, zLimits[0], zLimits[1]));
    }

    // station default cam at last unit that was looked at
    public void TurnOnDefaultCam()
    {
        ResetDefaultCam();

        if (focus1 && focusCineCam1.LookAt != null)
        {
            Vector3 tmpPos = defaultCineCam.transform.localPosition;
            tmpPos.x = focusCineCam1.transform.localPosition.x;
            tmpPos.z = focusCineCam1.transform.localPosition.z;
            defaultCineCam.transform.localPosition = tmpPos;
        }
        else if (!focus1 && focusCineCam2.LookAt != null)
        {
            Vector3 tmpPos = defaultCineCam.transform.localPosition;
            tmpPos.x = focusCineCam2.transform.localPosition.x;
            tmpPos.z = focusCineCam2.transform.localPosition.z;
            defaultCineCam.transform.localPosition = tmpPos;
        }

        defaultCineCam.gameObject.SetActive(true);
        focusCineCam1.gameObject.SetActive(false);
        focusCineCam2.gameObject.SetActive(false);
    }


    public void TurnOnFocusCam(Transform unit)
    {
        ResetFocusCam();

        if (focus1)
        {
            focusCineCam2.LookAt = unit;
            focusCineCam2.Follow = unit;

            focus1 = false;

            defaultCineCam.gameObject.SetActive(false);
            focusCineCam1.gameObject.SetActive(false);
            focusCineCam2.gameObject.SetActive(true);
        }
        else
        {
            focusCineCam1.LookAt = unit;
            focusCineCam1.Follow = unit;

            focus1 = true;

            defaultCineCam.gameObject.SetActive(false);
            focusCineCam1.gameObject.SetActive(true);
            focusCineCam2.gameObject.SetActive(false);
        }
    }

    // set default cam to look at next available ally unit
    public void DefaultCamLookAtNextUnit()
    {
        ResetDefaultCam();

        defaultCineCam.Follow = FindClosestAvailableAllyUnit();
        defaultCineCam.LookAt = FindClosestAvailableAllyUnit();

        defaultCineCam.gameObject.SetActive(true);
        focusCineCam1.gameObject.SetActive(false);
        focusCineCam2.gameObject.SetActive(false);
    }

    Transform FindClosestAvailableAllyUnit()
    {
        float closestDistance = Mathf.Infinity;
        int closestIndex = -1;

        for (int i = 0; i < combatManager.allyTeam.Count; i++)
        {
            if (Vector3.Distance(combatManager.lastUnit.transform.position, combatManager.allyTeam[i].transform.position) < closestDistance)
            {
                closestDistance = Vector3.Distance(combatManager.lastUnit.transform.position, combatManager.allyTeam[i].transform.position);
                closestIndex = i;
            }
        }

        return combatManager.allyTeam[closestIndex].transform.parent.transform;
    }

    void ResetFocusCam()
    {
        focusCineCam1.LookAt = null;
        focusCineCam1.Follow = null;
        focusCineCam2.LookAt = null;
        focusCineCam2.Follow = null;
    }

    void ResetDefaultCam()
    {
        defaultCineCam.Follow = null;
        defaultCineCam.LookAt = null;
    }
}
