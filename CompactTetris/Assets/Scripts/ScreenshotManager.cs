using UnityEngine;
using System.Collections;
using System.IO;

public class ScreenshotManager : MonoBehaviour
{
    public PlayerModel playerModel;

    void Start()
    {
        playerModel = GameManager.Instance.PlayerModel;
    }

    public void DoCapture()
    {
        StartCoroutine(CaptureScreenshot());
    }

    IEnumerator CaptureScreenshot()
    {
        // 스크린샷 캡처
        yield return new WaitForEndOfFrame();

        Debug.Log("Capture Start?");

        Texture2D screenshot = new Texture2D(Screen.width / 3, Screen.height, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(Screen.width / 3, 0, Screen.width / 3, Screen.height), 0, 0);

        screenshot.Apply();

        // 텍스처를 PNG로 인코딩
        byte[] imageData = screenshot.EncodeToPNG();
        // Base64로 인코딩
        string base64Image = System.Convert.ToBase64String(imageData);

        Destroy(screenshot);
        
        playerModel.GameSceneImg = base64Image;
        Debug.Log("Capture Update?");
    }

    public Sprite decriptCapture(string img)
    {
        byte[] imageData = System.Convert.FromBase64String(img);

        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(imageData);

        Rect rect = new Rect(0, 0, texture.width, texture.height);
        Sprite sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));

        return sprite;
    }

    
}
