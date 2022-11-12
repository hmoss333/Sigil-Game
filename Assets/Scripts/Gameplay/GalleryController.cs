using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GalleryController : MonoBehaviour
{
    Dictionary<string, Texture> storedSigils;
    public Dropdown sigilList;
    [SerializeField] RawImage sigilImage;
    [SerializeField] ParticleSystem chargeEffect, burnEffect;
    [SerializeField] float burnTimer;
    bool charging, burning;
    [SerializeField] GameObject pulseEffect, chargeText;
    [SerializeField] GameObject burnWarning;


    // Start is called before the first frame update
    void Start()
    {
        sigilImage.gameObject.SetActive(false);
        pulseEffect.SetActive(false);

        storedSigils = new Dictionary<string, Texture>();
        StartCoroutine(GetFiles());
    }

    // Update is called once per frame
    void Update()
    {
        charging = Input.GetKey(KeyCode.Mouse0) ? true : false;
        chargeText.SetActive(sigilImage.gameObject.activeSelf);

        //Feels like this can be simplified
        if (sigilImage.gameObject.activeSelf)
        {
            if (charging && !chargeEffect.isPlaying && !burning)
            {
                chargeEffect.Play();
                pulseEffect.SetActive(true);
            }
            else if ((!charging && chargeEffect.isPlaying) || burning)
            {
                chargeEffect.Stop();
                pulseEffect.SetActive(false);
            }
        }
    }



    //==Burn UI Logic==//
    public void Burn()
    {
        if (sigilImage.gameObject.activeSelf && !burning)
        {
            burnWarning.SetActive(true);
        }
    }

    public void BurnYes()
    {
        burnWarning.SetActive(false);
        burning = true;
        StartCoroutine(BurnRoutine(burnTimer));
    }

    public void BurnNo()
    {
        burnWarning.SetActive(false);
    }

    IEnumerator BurnRoutine(float burnTime)
    {
        burnEffect.Play();

        yield return new WaitForSeconds(burnTime);

        burnEffect.Stop();
        RemoveSavedImage();
        burning = false;
    }



    //==Image Storage/Recall==//
    IEnumerator GetFiles()
    {
        yield return new WaitForSeconds(0.15f);
        storedSigils.Clear();
        sigilList.ClearOptions();

        List<Dropdown.OptionData> tempOptionData = new List<Dropdown.OptionData>();

        string info = Application.persistentDataPath + "/Sigils/";
        string[] fileInfo = Directory.GetFiles(info, "*.png");
        for (int i = 0; i < fileInfo.Length; i++)
        {
            string url = "file://" + fileInfo[i];

            WWW www = new WWW(url);

            // Wait for download to complete
            yield return www;

            string fileName = Path.GetFileNameWithoutExtension(fileInfo[i]);

            if (!storedSigils.ContainsKey(fileName))
                storedSigils.Add(fileName, www.texture);

            Dropdown.OptionData newData = new Dropdown.OptionData();
            newData.text = fileName;
            //newData.image = Sprite.Create(www.texture, new Rect(0,0, www.texture.width, www.texture.height), new Vector2(0,0)); //not needed. storedSigils already has this reference
            tempOptionData.Add(newData);
        }

        foreach (Dropdown.OptionData optionData in tempOptionData)
        {
            sigilList.options.Add(optionData);
        }

        sigilList.value = 0;
        sigilList.RefreshShownValue();
    }

    public void LoadSavedImage()
    {
        if (storedSigils.Count > 0)
        {
            //PlayAudio(selectImageSound);

            int tempValue = sigilList.value;
            sigilImage.texture = storedSigils[sigilList.options[tempValue].text];
            sigilImage.gameObject.SetActive(true);
        }
    }

    public void RemoveSavedImage()
    {
        //PlayAudio(deleteSound);
        if (storedSigils.Count > 0)
        {
            int tempValue = sigilList.value;
            File.Delete(Application.persistentDataPath + "/Sigils/" + sigilList.options[tempValue].text + ".png");
            storedSigils.Remove(sigilList.options[tempValue].text);
            sigilList.options.RemoveAt(tempValue);
            sigilList.Hide();
            sigilList.RefreshShownValue();
            sigilImage.gameObject.SetActive(false);
        }
    }
}
