using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Complex = System.Numerics.Complex;

public class SineWaveRender : MonoBehaviour {

    [SerializeField] Transform cameraStand;
    GameObject[] valuePoints;
    [SerializeField] int pointCount = 1000;

    [SerializeField] float amplitude = 10;
    [SerializeField] float frequency = 10;
    [SerializeField] int type = 0;
    float curFrequency = 0;
    float curAmplitude = 0;
    int curType = 0;

    void Start() {

        Application.targetFrameRate = 60;

        valuePoints = new GameObject[pointCount];
        for (int i = 0; i < pointCount; i++) {
            valuePoints[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            valuePoints[i].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }

    }

    void Update() {

        if (Input.GetMouseButton(1)) {
            float mouseMove = Input.GetAxis("Mouse X");
            cameraStand.Rotate(0, mouseMove, 0);
        }

        if (curAmplitude != amplitude || curFrequency != frequency || curType != type) {
            curAmplitude = amplitude;
            curFrequency = frequency;
            curType = type;

            Vector3 offset = new Vector3(-10, 0, 0);

            // 生产一段正弦波的采样点
            float[] values = Bake_Values();

            // 时域转频域
            Complex[] spectrum = FourierHelper.DFT(values);

            // 频域转时域
            float[] values2 = FourierHelper.DFT_Inverse_Real(spectrum);

            if (type == 0) {
                // 原始时域
                for (int i = 0; i < pointCount; i++) {
                    valuePoints[i].transform.position = new Vector3(i / 60f, values[i], 0) + offset;
                }
            } else if (type == 1) {
                // 时域转频域
                for (int i = 0; i < pointCount; i++) {
                    var wave = spectrum[i];
                    float amplitude = (float)wave.Magnitude;
                    valuePoints[i].transform.position = new Vector3(i / 60f, amplitude, 0) + offset;
                }
            } else if (type == 2) {
                // 频域转时域
                for (int i = 0; i < pointCount; i++) {
                    valuePoints[i].transform.position = new Vector3(i / 60f, values2[i], 0) + offset;
                }
            }

        }

    }

    float[] Bake_Values() {
        var values = new float[pointCount];
        for (int i = 0; i < pointCount; i++) {
            float t = i / 60f;
            float y = FourierHelper.Y_ByRealWave(t, amplitude, frequency, 0);
            // float y = amplitude * Mathf.Sin(frequency * t);
            values[i] = y;
        }
        return values;
    }

}
