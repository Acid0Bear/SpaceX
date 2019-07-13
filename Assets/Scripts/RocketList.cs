using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System;
using TMPro;

[Serializable]
public class LaunchObj
{
    [Serializable]
    public class Firststage
    {
        [Serializable]
        public class core
        {
            public string core_serial;
            public int flight;
        }
        public core[] cores;
    }
    [Serializable]
    public class SecondStage
    {
        [Serializable]
        public class Payloads
        {
            public string payload_type;
            public string payload_mass_kg;
            public string nationality;
        }
        public Payloads[] payloads;
    }
    [Serializable]
    public class Rocket
    {
        public string rocket_name;
        public string rocket_type;
        public Firststage first_stage;
        public SecondStage second_stage;
    }
    [Serializable]
    public class Links
    {
        public string mission_patch_small;
        public string[] flickr_images;
    }
    [Serializable]
    public class LaunchSite
    {
        public string site_name;
    }
    public string launch_year;
    public string mission_name;
    public bool upcoming;
    public Rocket rocket;
    public Links links;
    public LaunchSite launch_site;
}

[Serializable]
public class Launches
{
    public LaunchObj[] launches;
    public int GetLength() { return launches.Length; }
    public LaunchObj[] GetArray() {return launches;}
}


public class RocketList : MonoBehaviour
{
    Launches rocketlist;
    public Sprite NoPhoto;
    public Button ToMainMenu;
    public Button morePhotos;
    public GameObject payloads, type_kg, origin, rocket_name, rocket_type, serial, flights, home_port,missionName,year,TitleIMG,BigIMG;
    public GameObject[] photos;
    public GameObject ButtonSample, Content,Details;
    List<GameObject> buttons = new List<GameObject>();
    void Start()
    {
        StartCoroutine(GetList());
        var click = photos[0].GetComponent<Button>();
        click.onClick.AddListener(() => SetImg(photos[0]));
        click = photos[1].GetComponent<Button>();
        click.onClick.AddListener(() => SetImg(photos[1]));
        click = photos[2].GetComponent<Button>();
        click.onClick.AddListener(() => SetImg(photos[2]));
        click = photos[3].GetComponent<Button>();
        click.onClick.AddListener(() => SetImg(photos[3]));
        click = photos[4].GetComponent<Button>();
        click.onClick.AddListener(() => SetImg(photos[4]));
        click = photos[5].GetComponent<Button>();
        click.onClick.AddListener(() => SetImg(photos[5]));
        click = photos[6].GetComponent<Button>();
        click.onClick.AddListener(() => SetImg(photos[6]));
        click = photos[7].GetComponent<Button>();
        click.onClick.AddListener(() => SetImg(photos[7]));
        /*for (int f = 0; f < 8; f++)
        {
            var click = photos[f].GetComponent<Button>();
            click.onClick.AddListener(() => SetImg(photos[f]));
        }*/

    }

    void Update()
    {
        
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
    IEnumerator GetList()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://api.spacexdata.com/v3/launches");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            rocketlist = JsonUtility.FromJson<Launches>("{\"launches\":" + www.downloadHandler.text + "}");
        }
        SetButtons();
    }

    void SetButtons()
    {
        float y = -45.89f;
        for (int z = 0; z < rocketlist.GetLength(); z++)
        {
            GameObject button = Instantiate(ButtonSample, Content.transform);
            var components = button.GetComponentsInChildren<TextMeshProUGUI>();
            for (int a = 0; a < components.Length;a++)
            {
                if (components[a].text == "mission_name")
                    components[a].text = rocketlist.GetArray()[z].mission_name;
                else if (components[a].text == "%year")
                    components[a].text = rocketlist.GetArray()[z].launch_year;
                else if (components[a].text == "IsLaunched")
                    if (!rocketlist.GetArray()[z].upcoming)
                    {
                        components[a].text = "Launched!";
                        components[a].color = Color.red;
                    }
                    else
                    {
                        components[a].text = "Upcoming!";
                        components[a].color = Color.green;
                    }
                    

            }
            StartCoroutine(DownloadImage(rocketlist.GetArray()[z].links.mission_patch_small,button.GetComponentInChildren<RawImage>()));
            button.GetComponent<RectTransform>().localPosition = new Vector3(0, y, 0);
            y -= 45.89f * 2;
            Content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, -y);
            buttons.Add(button);
        }
        int x = 0;
        foreach (GameObject button in buttons)
        {
            var click = button.GetComponent<Button>();
            click.onClick.AddListener(() => FillDetails(buttons.IndexOf(button), rocketlist.GetArray()));
            x++;
        }
    }
    IEnumerator DownloadImage(string MediaUrl,RawImage img)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
            img.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
    }

    void SetImg(GameObject photo)
    {
        BigIMG.GetComponentInChildren<RawImage>().texture = photo.GetComponentInChildren<RawImage>().texture;
    }
    void FillDetails(int num, LaunchObj[] rocketlist)
    {
        TitleIMG.GetComponent<RawImage>().texture = NoPhoto.texture;
        Details.SetActive(true);
        missionName.GetComponent<TextMeshProUGUI>().text = rocketlist[num].mission_name;
        payloads.GetComponent<TextMeshProUGUI>().text = string.Format("Payloads - {0}", rocketlist[num].rocket.second_stage.payloads.Length);
        type_kg.GetComponent<TextMeshProUGUI>().text = string.Format("{0} - {1} kg", rocketlist[num].rocket.second_stage.payloads[0].payload_type, rocketlist[num].rocket.second_stage.payloads[0].payload_mass_kg);
        origin.GetComponent<TextMeshProUGUI>().text = string.Format("{0}", rocketlist[num].rocket.second_stage.payloads[0].nationality);
        rocket_name.GetComponent<TextMeshProUGUI>().text = rocketlist[num].rocket.rocket_name;
        rocket_type.GetComponent<TextMeshProUGUI>().text = string.Format("Rocket type - {0}", rocketlist[num].rocket.rocket_type);
        serial.GetComponent<TextMeshProUGUI>().text = string.Format("Serial - {0}", rocketlist[num].rocket.first_stage.cores[0].core_serial);
        flights.GetComponent<TextMeshProUGUI>().text = string.Format("Flights - {0}", rocketlist[num].rocket.first_stage.cores[0].flight.ToString());
        home_port.GetComponent<TextMeshProUGUI>().text = string.Format("Port - {0}", rocketlist[num].launch_site.site_name);
        year.GetComponent<TextMeshProUGUI>().text = rocketlist[num].launch_year.ToString();     
        int i;
        if (rocketlist[num].links.flickr_images.Length>=1) {
            StartCoroutine(DownloadImage(rocketlist[num].links.flickr_images[0], TitleIMG.GetComponent<RawImage>()));
            morePhotos.enabled = true;
            for (i = 0; i < rocketlist[num].links.flickr_images.Length&&i<8; i++)
            {
                photos[i].SetActive(true);
                StartCoroutine(DownloadImage(rocketlist[num].links.flickr_images[i], photos[i].GetComponentInChildren<RawImage>()));
            }
            if (i < 8)
                for (; i < 8; i++)
                {
                    photos[i].SetActive(false);
                }
        }
        else
            morePhotos.enabled = false;
    }
}
