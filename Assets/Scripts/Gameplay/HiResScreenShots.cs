using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class HiResScreenShots : MonoBehaviour {

    Camera camera;
    public int resWidth;// = 2550;
    public int resHeight;// = 3300;

    string dir;

    private void Start()
    {
        camera = GetComponent<Camera>();
        resWidth = camera.pixelWidth;
        resHeight = camera.pixelHeight;

        dir = Application.persistentDataPath + "/Sigils/";

        if (!Directory.Exists(dir))
        {
            System.IO.Directory.CreateDirectory(dir);
        }
    }

    public void TakeHiResShot()
    {
        //Generate a new, temporary render texture
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        camera.targetTexture = rt;

        //Sssign the current camera view to the render texture
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGBA32, false);
        camera.cullingMask &= ~(1 << LayerMask.NameToLayer("UI")); //Hide UI layer
        camera.Render();
        RenderTexture.active = rt;

        //Save screenshot data of renderTexture using offsets
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);

        //Clear all temporary textures
        camera.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);

        //Create byte buffer for screenshot data
        byte[] bytes = screenShot.EncodeToPNG();

        //Save screenshot byte data to destination + file name
        string filename = SigilManager.screenShotName + ".png";
        string filePath = dir + filename;
        System.IO.File.WriteAllBytes(filePath, bytes);

        //Log confirmation and UI cleanup
        Debug.Log(string.Format("Took screenshot to: {0}", filePath));
        camera.cullingMask |= 1 << LayerMask.NameToLayer("UI"); //Show UI layer
    }
}
