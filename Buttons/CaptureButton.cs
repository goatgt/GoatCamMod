using System;
using System.Collections;
using System.IO;
using BepInEx;
using UnityEngine;

namespace GoatCamMod;

public class CaptureButton : GorillaPressableButton
{
    public Camera        TargetCamera;
    public RenderTexture TargetTexture;

    private new MeshRenderer buttonRenderer;
    private     bool         isCapturing;

    public void Start()
    {
        gameObject.layer = 18;

        buttonRenderer = transform.Find("ButtonMesh")?.GetComponent<MeshRenderer>();
        if (buttonRenderer == null)
            buttonRenderer = GetComponent<MeshRenderer>();

        if (buttonRenderer != null)
        {
            if (buttonRenderer.material == null)
                buttonRenderer.material = new Material(Shader.Find("Standard"));

            unpressedMaterial = new Material(buttonRenderer.material) { color = Color.white, };
            pressedMaterial   = new Material(buttonRenderer.material) { color = Color.red, };

            SetUnpressedMaterial();
        }
        else
        {
            Debug.LogError("[GoatCamMod] ButtonMesh or MeshRenderer not found! Button will not work properly.");
        }

        StartCoroutine(AssignCameraNextFrame());
    }

    public override void ButtonActivation()
    {
        if (buttonRenderer == null || unpressedMaterial == null || pressedMaterial == null)
        {
            Debug.LogWarning("[GoatCamMod] Button materials not ready yet.");

            return;
        }

        if (!isCapturing)
            StartCoroutine(CountdownAndCapture());

        isOn = !isOn;

        UpdateColor();
    }

    private void EnsureCameraAndTexture()
    {
        if (TargetCamera == null)
        {
            GameObject camObj = GameObject.Find("GoatCameraModModelBetter(Clone)/Camera");
            if (camObj != null)
            {
                TargetCamera = camObj.GetComponent<Camera>();
            }
            else
            {
                Debug.LogError("[GoatCamMod] Camera object not found!");

                return;
            }
        }

        if (TargetCamera == null)
            return;

        if (TargetCamera.targetTexture == null)
        {
            TargetTexture = new RenderTexture(1024, 1024, 24, RenderTextureFormat.ARGB32)
            {
                    useMipMap        = false,
                    autoGenerateMips = false,
            };

            TargetTexture.Create();
            TargetCamera.targetTexture = TargetTexture;

            Debug.Log("[GoatCamMod] Created ARGB32 RenderTexture.");
        }
        else
        {
            TargetTexture = TargetCamera.targetTexture;
        }

        TargetCamera.allowHDR  = false;
        TargetCamera.allowMSAA = true;
    }

    private IEnumerator CountdownAndCapture()
    {
        isCapturing = true;

        EnsureCameraAndTexture();

        if (TargetCamera == null || TargetTexture == null)
        {
            Debug.LogError("[GoatCamMod] Camera or RenderTexture not assigned!");
            isCapturing = false;

            yield break;
        }

        Debug.Log("[GoatCamMod] 3...");

        yield return new WaitForSeconds(1f);
        Debug.Log("[GoatCamMod] 2...");

        yield return new WaitForSeconds(1f);
        Debug.Log("[GoatCamMod] 1...");

        yield return new WaitForSeconds(1f);

        Debug.Log("[GoatCamMod] Smile!");

        yield return StartCoroutine(CaptureScreenshotCoroutine());

        isCapturing = false;
    }

    private IEnumerator CaptureScreenshotCoroutine()
    {
        yield return new WaitForEndOfFrame();

        if (TargetCamera == null || TargetTexture == null)
            yield break;

        RenderTexture previous = RenderTexture.active;

        TargetCamera.Render();
        RenderTexture.active = TargetTexture;

        Texture2D tex = new(TargetTexture.width, TargetTexture.height, TextureFormat.RGB24, false, false);
        tex.ReadPixels(new Rect(0, 0, TargetTexture.width, TargetTexture.height), 0, 0);
        tex.Apply();

        // Gamma correction
        Color[] pixels = tex.GetPixels();
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i].r = Mathf.LinearToGammaSpace(pixels[i].r);
            pixels[i].g = Mathf.LinearToGammaSpace(pixels[i].g);
            pixels[i].b = Mathf.LinearToGammaSpace(pixels[i].b);
        }

        tex.SetPixels(pixels);
        tex.Apply();

        RenderTexture.active = previous;

        string folderPath = Path.Combine(Paths.PluginPath, "GoatCamModCaptures");
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        string fileName = "GoatCam_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        string fullPath = Path.Combine(folderPath, fileName);

        File.WriteAllBytes(fullPath, tex.EncodeToPNG());
        Destroy(tex);

        Debug.Log("[GoatCamMod] Screenshot saved to: " + fullPath);
    }

    private IEnumerator AssignCameraNextFrame()
    {
        yield return null;
        EnsureCameraAndTexture();
    }
}