using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderTest : MonoBehaviour
{
    public Shader clickDistortionShader;
    private Material clickDistortionMaterial;

    public float distortionDuration = 1f;
    private float effectStartTime = -1f;
    private Vector2 clickPosition = Vector2.zero;

    void Start()
    {
        if (clickDistortionShader != null)
        {
            clickDistortionMaterial = new Material(clickDistortionShader);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            effectStartTime = Time.time;

            // 클릭한 화면 좌표 가져오기
            clickPosition = Input.mousePosition;

            // 화면 좌표를 UV 좌표로 변환
            clickPosition.x /= Screen.width;
            clickPosition.y /= Screen.height;

            Debug.Log("Clicked!");
        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (clickDistortionMaterial != null && effectStartTime >= 0)
        {
            float elapsedTime = Time.time - effectStartTime;

            // 쉐이더로 클릭 좌표와 효과 전달
            clickDistortionMaterial.SetVector("_DistortionCenter", new Vector4(clickPosition.x, clickPosition.y, 0, 0));
            clickDistortionMaterial.SetFloat("_DistortionRadius", 0.02f);
            clickDistortionMaterial.SetFloat("_DistortionStrength", Mathf.Lerp(0.2f, 0f, elapsedTime / distortionDuration));

            // 쉐이더를 적용
            Graphics.Blit(src, dest, clickDistortionMaterial);

            // 효과 시간이 지나면 초기화
            if (elapsedTime > distortionDuration)
            {
                effectStartTime = -1f;
            }
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}
