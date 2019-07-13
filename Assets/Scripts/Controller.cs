using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using RG.OrbitalElements;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    public GameObject Tesla, Sun,dateNow,Earth,Venus,Mercury;
    public GameObject[] values;
    Vector3 PosF, cur;
    Vector3Double PosD;
    public LIST list;
    public GameObject[] speed;
    private int AdD = 10, x = 0;
    float RotationPercentage, rotationSpeed = 1;
    bool IsAble = true;
    void Start()
    {
        speed[0].GetComponent<Image>().color = Color.blue;
        DateTime dt = DateTime.FromOADate(double.Parse(list.dataArray[x].Dateutc, System.Globalization.CultureInfo.InvariantCulture));
        dt = TimeZoneInfo.ConvertTime(dt,TimeZoneInfo.Local).ToLocalTime();
        string time = dt.ToString();
        dateNow.GetComponent<TextMeshProUGUI>().text = time;
        PosD = RG.OrbitalElements.Calculations.CalculateOrbitalPosition(double.Parse(list.dataArray[x].Semimajoraxisau, System.Globalization.CultureInfo.InvariantCulture),
                                                                            double.Parse(list.dataArray[x].Eccentricity, System.Globalization.CultureInfo.InvariantCulture),
                                                                            double.Parse(list.dataArray[x].Inclinationdegrees, System.Globalization.CultureInfo.InvariantCulture),
                                                                            double.Parse(list.dataArray[x].Longitudeofascnodedegrees, System.Globalization.CultureInfo.InvariantCulture),
                                                                            double.Parse(list.dataArray[x].Argumentofperiapsisdegrees, System.Globalization.CultureInfo.InvariantCulture),
                                                                            double.Parse(list.dataArray[x].Trueanomalydegrees, System.Globalization.CultureInfo.InvariantCulture));
        Tesla.transform.position = new Vector3((float)PosD.x / AdD, (float)PosD.y / AdD, (float)PosD.z / AdD);
            values[0].GetComponent<TextMeshProUGUI>().text = list.dataArray[x].Semimajoraxisau;
            values[1].GetComponent<TextMeshProUGUI>().text = list.dataArray[x].Eccentricity;
            values[2].GetComponent<TextMeshProUGUI>().text = list.dataArray[x].Inclinationdegrees;
            values[3].GetComponent<TextMeshProUGUI>().text = list.dataArray[x].Longitudeofascnodedegrees;
            values[4].GetComponent<TextMeshProUGUI>().text = list.dataArray[x].Argumentofperiapsisdegrees;
            values[5].GetComponent<TextMeshProUGUI>().text = list.dataArray[x].Meananomalydegrees;
            values[6].GetComponent<TextMeshProUGUI>().text = list.dataArray[x].Trueanomalydegrees;
        x++;
        speed[0].GetComponent<Button>().onClick.AddListener(() => SetSpeed(speed[0],1));
        speed[1].GetComponent<Button>().onClick.AddListener(() => SetSpeed(speed[1], 2));
        speed[2].GetComponent<Button>().onClick.AddListener(() => SetSpeed(speed[2], 4));
    }
    void SetSpeed(GameObject target, int num)
    {
        for (int x = 0; x < 3; x++)
        {
            speed[x].GetComponent<Image>().color = Color.white;
            if (speed[x]==target) speed[x].GetComponent<Image>().color = Color.blue;
        }
        rotationSpeed = num;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
    void Update()
    {
        Earth.transform.position = RotatePointAroundPivot(Earth.transform.position+new Vector3(5,0,10f), Sun.transform.position, Quaternion.Euler(0,50f * 0.00033f*rotationSpeed, 0));
        Mercury.transform.position = RotatePointAroundPivot(Mercury.transform.position + new Vector3(0, 0, 5f), Sun.transform.position, Quaternion.Euler(0, 50f * 0.0018f * rotationSpeed, 0));
        Venus.transform.position = RotatePointAroundPivot(Venus.transform.position + new Vector3(5, 0, 5f), Sun.transform.position, Quaternion.Euler(0, 50f * 0.00125f * rotationSpeed, 0));
        if (IsAble)
        {
            cur = Tesla.transform.position;
            DateTime dt = DateTime.FromOADate(double.Parse(list.dataArray[x].Dateutc, System.Globalization.CultureInfo.InvariantCulture));
            dt = TimeZoneInfo.ConvertTime(dt, TimeZoneInfo.Local).ToLocalTime();
            string time = dt.ToString();
            dateNow.GetComponent<TextMeshProUGUI>().text = time;
            PosD = RG.OrbitalElements.Calculations.CalculateOrbitalPosition(double.Parse(list.dataArray[x].Semimajoraxisau, System.Globalization.CultureInfo.InvariantCulture),
                                                                            double.Parse(list.dataArray[x].Eccentricity, System.Globalization.CultureInfo.InvariantCulture),
                                                                            double.Parse(list.dataArray[x].Inclinationdegrees, System.Globalization.CultureInfo.InvariantCulture),
                                                                            double.Parse(list.dataArray[x].Longitudeofascnodedegrees, System.Globalization.CultureInfo.InvariantCulture),
                                                                            double.Parse(list.dataArray[x].Argumentofperiapsisdegrees, System.Globalization.CultureInfo.InvariantCulture),
                                                                            double.Parse(list.dataArray[x].Trueanomalydegrees, System.Globalization.CultureInfo.InvariantCulture));
            values[0].GetComponent<TextMeshProUGUI>().text = list.dataArray[x].Semimajoraxisau;
            values[1].GetComponent<TextMeshProUGUI>().text = list.dataArray[x].Eccentricity;
            values[2].GetComponent<TextMeshProUGUI>().text = list.dataArray[x].Inclinationdegrees;
            values[3].GetComponent<TextMeshProUGUI>().text = list.dataArray[x].Longitudeofascnodedegrees;
            values[4].GetComponent<TextMeshProUGUI>().text = list.dataArray[x].Argumentofperiapsisdegrees;
            values[5].GetComponent<TextMeshProUGUI>().text = list.dataArray[x].Meananomalydegrees;
            values[6].GetComponent<TextMeshProUGUI>().text = list.dataArray[x].Trueanomalydegrees;
            PosF = new Vector3((float)PosD.x / AdD, (float)PosD.y / AdD, (float)PosD.z / AdD);
            x++; IsAble = false;
            RotationPercentage = 1f;
        }
        else if (!IsAble)
        {
            RotationPercentage -= 0.01f * rotationSpeed;
            Tesla.transform.position = Vector3.Slerp(PosF, cur, RotationPercentage);
            if (RotationPercentage <= 0.45f)
            {
                IsAble = true;
            }
        }
        if (x == 606)
        {
            Debug.Log("We reached final destination!");
            x = 49;
        }
    }
    public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion angle)
    {
        return angle * (point - pivot) + pivot;
    }

}
