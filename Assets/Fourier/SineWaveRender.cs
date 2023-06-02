using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineWaveRender : MonoBehaviour {

    GameObject[] points;
    [SerializeField] int pointCount = 2000;

    void Start() {
        points = new GameObject[pointCount];
        for (int i = 0; i < pointCount; i++) {
            points[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            points[i].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }
    }

    void Update() {
        float amplitude = 10f;
        float frequency = 1f;
        float phase = 0f;
        Vector3 offset = new Vector3(-10, 0, 0);
        for (int i = 0; i < pointCount; i++) {
            var point = points[i];
            float x = i / 100f;
            float y = 叠加出锯齿波(x, amplitude, frequency, phase);
            Vector3 pos = new Vector3(x, y, 0) + offset;
            point.transform.position = pos;
        }
    }

    float 叠加出方波(float x, float amplitude, float frequency, float phase) {
        float y = 0;
        for (int j = 0; j < 1000; j++) {
            y += SineWave(x, amplitude / ((1 + 2 * j) * Mathf.PI), (1 + 2 * j) * frequency, phase);
        }
        return y;
    }

    float 叠加出锯齿波(float x, float amplitude, float frequency, float phase) {
        float y = 0;
        for (int j = 0; j < 1000; j++) {
            int sign = j % 2 == 0 ? 1 : -1;
            y += SineWave(x, sign * amplitude / ((j + 1) * Mathf.PI), (j + 1) * frequency, phase);
        }
        return y;
    }        

    float SineWave(float x, float amplitude, float frequency, float phase) {
        return amplitude * Mathf.Sin(2 * Mathf.PI * frequency * x + phase);
    }

}
