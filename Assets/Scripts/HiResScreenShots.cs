using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class HiResScreenShots : MonoBehaviour {

    Camera camera;
    //SigilManager sm; 

    public int resWidth = 2550;
    public int resHeight = 3300;

    public bool takeHiResShot = false;

    private void Start()
    {
        camera = GetComponent<Camera>();
        //sm = GameObject.FindObjectOfType<SigilManager>();

        string dir = Application.persistentDataPath + "/Sigils";

        if (!Directory.Exists(dir))
        {
            System.IO.Directory.CreateDirectory(dir);
        }
    }

    public static string ScreenShotName(string sigilName)//int width, int height)
    {
        string dir = Application.persistentDataPath + "/Sigils";

        if (!Directory.Exists(dir))
        {
            System.IO.Directory.CreateDirectory(dir);
        }

        return string.Format("{0}/sigil_{1}.png",
                                dir,
                                sigilName);
    }

    public void TakeHiResShot()
    {
        takeHiResShot = true;
    }

    void LateUpdate()
    {
        takeHiResShot |= Input.GetKeyDown("k");
        if (takeHiResShot)
        {
            RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
            camera.targetTexture = rt;
            Texture2D screenShot = new Texture2D(resWidth/2, resHeight, TextureFormat.RGB24, false);
            camera.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(resWidth/2, 0, resWidth, resHeight), 0, 0);
            camera.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);
            byte[] bytes = screenShot.EncodeToPNG();
            string filename = ScreenShotName(SigilManager.screenShotName);
            System.IO.File.WriteAllBytes(filename, bytes);
            Debug.Log(string.Format("Took screenshot to: {0}", filename));
            takeHiResShot = false;
        }
    }
}
