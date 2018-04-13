using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class HiResScreenShots : MonoBehaviour {

    Camera camera; 

    public int resWidth = 2550;
    public int resHeight = 3300;

    public bool takeHiResShot = false;

    private void Start()
    {
        camera = GetComponent<Camera>();

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

        return string.Format("{0}/{1}.png",
                                dir,
                                sigilName);
    }

    public void TakeHiResShot()
    {
        takeHiResShot = true;
    }

    private const string MediaStoreImagesMediaClass = "android.provider.MediaStore$Images$Media";
    private static AndroidJavaObject _activity;

    public static AndroidJavaObject Activity
        {
            get
            {
                if (_activity == null)
                {
                    var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                    _activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                }
                return _activity;
            }
        }

    public static string SaveImageToGallery(Texture2D texture2D, string title, string description)
    {
        using (var mediaClass = new AndroidJavaClass(MediaStoreImagesMediaClass))
        {
            using (var cr = Activity.Call<AndroidJavaObject>("getContentResolver"))
            {
                var image = Texture2DToAndroidBitmap(texture2D);
                var imageUrl = mediaClass.CallStatic<string>("insertImage", cr, image, title, description);
                return imageUrl;
            }
        }
    }

    public static AndroidJavaObject Texture2DToAndroidBitmap(Texture2D texture2D)
    {
        byte[] encoded = texture2D.EncodeToPNG();
        using (var bf = new AndroidJavaClass("android.graphics.BitmapFactory"))
        {
            return bf.CallStatic<AndroidJavaObject>("decodeByteArray", encoded, 0, encoded.Length);
        }
    }

    void LateUpdate()
    {
        takeHiResShot |= Input.GetKeyDown("k");
        if (takeHiResShot)
        {
            RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
            camera.targetTexture = rt;
            Texture2D screenShot = new Texture2D(resWidth/2, resHeight, TextureFormat.RGBA32, false);
            camera.cullingMask &= ~(1 << LayerMask.NameToLayer("UI")); //Hide UI layer
            camera.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(resWidth/2, 0, resWidth, resHeight), 0, 0);
            camera.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);
            byte[] bytes = screenShot.EncodeToPNG();
            string filename = ScreenShotName(SigilManager.screenShotName);
            System.IO.File.WriteAllBytes(filename, bytes);
//#if UNITY_ANDROID
            //SaveImageToGallery(screenShot, SigilManager.screenShotName, SigilManager.screenShotName);
//#endif
            Debug.Log(string.Format("Took screenshot to: {0}", filename));
            camera.cullingMask |= 1 << LayerMask.NameToLayer("UI"); //Show UI layer
            takeHiResShot = false;
        }
    }
}
