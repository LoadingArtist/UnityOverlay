using System;
using System.Drawing;
using System.Runtime.InteropServices;
using UnityEngine;

public class TransparentWindow : MonoBehaviour
{
    [SerializeField]
    private Material mat;

    private struct MARGINS
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }

    //Windows APIs
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

    [DllImport("Dwmapi.dll")]
    private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);
    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);


    // Definitions of window styles
    const int GWL_STYLE = -16;
    const uint WS_POPUP = 0x80000000;
    const uint WS_VISIBLE = 0x10000000;

    void Start()
    {
#if !UNITY_EDITOR
        var margins = new MARGINS() { cxLeftWidth = -1 };
        var hwnd = GetActiveWindow();
        SetWindowLong(hwnd, GWL_STYLE, WS_POPUP | WS_VISIBLE);
        DwmExtendFrameIntoClientArea(hwnd, ref margins);
#endif
    }

    void OnRenderImage(RenderTexture from, RenderTexture to)
    {
        Graphics.Blit(from, to, mat);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowWindow(GetActiveWindow(), 2);
        }
    }

}