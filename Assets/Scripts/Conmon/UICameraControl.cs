using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class UICameraControl : MonoBehaviour
{
    public static float Size;
    private void OnEnable()
    {
        float designWidth = 1080f;//�����зֱ��ʵĿ��
        float designHeight = 1920f;//�����зֱ��ʵĸ߶�
        float designOrthographicSize = designHeight / 100f/2f;//����ʱ����������Ĵ�С��3.2*100*2=640����100����ΪUnity�е�pixels per unit��100����2����Ϊ�����ó���Ļ��һ��
        float designScale = designWidth / designHeight;
        float scaleRate = (float)Screen.width / (float)Screen.height;
        if (scaleRate < designScale)//�ж�������Ƶı�����ʵ�ʱ����Ƿ�һ�£����������õĴ����������Ӧ���ã�С�Ļ������Զ�����Ӧ
        {
            float scale = scaleRate / designScale;
            designOrthographicSize = designOrthographicSize / scale;
            GetComponent<Camera>().orthographicSize = designOrthographicSize;
        }
        else
        {
            float scale = scaleRate / designScale;
            designOrthographicSize = designOrthographicSize / scale;
            GetComponent<Camera>().orthographicSize = designOrthographicSize;
        }
      
        Size = scaleRate * designOrthographicSize * 2;
    }
}
