using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class WorldSpaceCanvasScaler : MonoBehaviour
{
    public Camera mainCamera;
    public float distanceFromCamera = 5f; // Canvas'ın kameradan uzaklığı

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        AdjustCanvasWidth();
    }

    void AdjustCanvasWidth()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();

        // Ekranın genişliği oranı (dünya birimi cinsinden)
        float screenHeight = Screen.height;
        float screenWidth = Screen.width;
        float aspectRatio = (float)screenWidth / screenHeight;

        // Yükseklik sabit olsun, genişliği oranla ayarla
        float canvasHeight = rectTransform.sizeDelta.y;
        float canvasWidth = canvasHeight * aspectRatio;

        rectTransform.sizeDelta = new Vector2(canvasWidth, canvasHeight);

        // Canvas'ı doğru mesafeye yerleştir
        Vector3 canvasPosition = mainCamera.transform.position + mainCamera.transform.forward * distanceFromCamera;
        transform.position = canvasPosition;
        transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);
    }
}
