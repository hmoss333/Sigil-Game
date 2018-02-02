using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class HiResScreenShots : MonoBehaviour {

    Camera camera;
    SigilManager sm; 

    public int resWidth = 2550;
    public int resHeight = 3300;

    public bool takeHiResShot = false;

    private void Start()
    {
        camera = GetComponent<Camera>();
        sm = GameObject.FindObjectOfType<SigilManager>();
    }

    public static string ScreenShotName(int width, int height)
    {
        return string.Format("{0}/Screenshots/sigil_{1}.png",
                             Application.dataPath,
                             //width, height,
                             SigilManager.screenShotName);
                             //System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    public void TakeHiResShot()
    {
        takeHiResShot = true;
    }

    void LateUpdate()
    {
        //takeHiResShot |= Input.GetKeyDown("k");
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
            string filename = ScreenShotName(resWidth, resHeight);
            System.IO.File.WriteAllBytes(filename, bytes);
            Debug.Log(string.Format("Took screenshot to: {0}", filename));
            takeHiResShot = false;
        }
    }
}
