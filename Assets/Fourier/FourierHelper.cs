using System;
using UnityEngine;
using Complex = System.Numerics.Complex;

// 设 t 为时间，F 为频率，p 为相位，A 为振幅
// 设 w = 2*pi*F
// 设 N 为采样点数

// 欧拉公式: e^(i*x) = cos(x) + i*sin(x)

// 实数正弦波公式: y = A * sin(2*pi*F*t + p) = A * sin(w*t + p)

// 复数正弦波公式: y = A * e^(i*(2*pi*F*t + p)) = A * e^(i*(w*t + p))

// 傅里叶级数公式: y = A1 * sin(w1*t + p1) + A2 * sin(w2*t + p2) + ... + An * sin(wn*t + pn)

// 时域转频域公式: y = A * e^(i*(w*t + p)) = A * (cos(w*t + p) + i*sin(w*t + p))

// 频域转时域公式: y = A * (cos(w*t + p) + i*sin(w*t + p)) = A * e^(i*(w*t + p))

public static class FourierHelper {

    const float PI = Mathf.PI;
    const float PI_2 = Mathf.PI * 2;
    const float PI_2_NEG = -PI_2;

    // 正弦波
    public static float Y_ByRealWave(float t, float A, float F, float p) {
        float w = PI_2 * F;
        return A * Mathf.Sin(w * t + p);
    }

    // 时域转频域
    public static Complex[] DFT(float[] values) {
        int N = values.Length;
        Complex[] spectrum = new Complex[N];
        for (int k = 0; k < N; k++) {
            // 第 k 个正弦波
            Complex wave = 0;
            float w = PI_2_NEG * k / N;
            for (int t = 0; t < N; t++) {
                float A = values[t];
                wave += A * Complex.Exp(new Complex(0, w * t));
            }
            spectrum[k] = wave;
        }
        return spectrum;
    }

    // 频域转时域
    public static float[] DFT_Inverse_Real(Complex[] spectrum) {
        int N = spectrum.Length;
        float[] values = new float[N];
        for (int k = 0; k < N; k++) {
            Complex sum = 0;
            float w = PI_2 * k / N;
            for (int t = 0; t < N; t++) {
                Complex A = spectrum[t];
                sum += A * Complex.Exp(new Complex(0, w * t));
            }
            values[k] = (float)sum.Real / N;
        }
        return values;
    }

}
