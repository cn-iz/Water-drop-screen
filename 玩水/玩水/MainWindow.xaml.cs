using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace 玩水
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        WaterEffect water;
        int an = 0;
        public MainWindow()
        {
            InitializeComponent();

            this.WindowState = System.Windows.WindowState.Normal;
            this.WindowStyle = System.Windows.WindowStyle.None;
            this.ResizeMode = System.Windows.ResizeMode.NoResize;
            this.Topmost = true;

            this.Left = 0.0;
            this.Top = 0.0;
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
            back1.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            back1.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
            var w = new Getrelsize().Getsize();
            double iWidth = w.Width;
            //屏幕高
            double iHeight = w.Height;
            //按照屏幕宽高创建位图
            var img = new Bitmap((int)iWidth, (int)iHeight);
            //从一个继承自Image类的对象中创建Graphics对象
            Graphics gc = Graphics.FromImage(img);
            //抓屏并拷贝到myimage里
            gc.CopyFromScreen(new System.Drawing.Point(0, 0), new System.Drawing.Point(0, 0), new System.Drawing.Size((int)iWidth, (int)iHeight));
            //this.BackgroundImage = img;
            System.Windows.Media.Imaging.BitmapSource bi =
            System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
            img.GetHbitmap(),
            IntPtr.Zero,
            Int32Rect.Empty,
            BitmapSizeOptions.FromEmptyOptions());

            back1.Source = bi;
            water = new WaterEffect(320,250);
            back1.Effect = water;
            HookStart();
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            DropWater();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                DropWater();
        }
        private void DropWater()
        {
            var p = Mouse.GetPosition(back);
            p.X /= back.RenderSize.Width;
            p.Y /= back.RenderSize.Height;
            if (p.X >= 0 && p.X <= 1 && p.Y >= 0 && p.Y <= 1)
                water.Drop((float)p.X, (float)p.Y);
        }

        // 安装钩子  
        [DllImport("user32.dll")]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);
        // 卸载钩子  
        [DllImport("user32.dll")]
        public static extern bool UnhookWindowsHookEx(int idHook);
        // 继续下一个钩子  
        [DllImport("user32.dll")]
        public static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);
        //声明定义  
        public delegate int HookProc(int nCode, int wParam, IntPtr lParam);
        static int hKeyboardHook = 0;
        HookProc KeyboardHookProcedure;




        private void Form1_Load(object sender, EventArgs e)
        {
            HookStart();
        }


        // 安装钩子  
        public void HookStart()
        {
            if (hKeyboardHook == 0)
            {
                // 创建HookProc实例  
                KeyboardHookProcedure = new HookProc(KeyboardHookProc);
                //定义全局钩子  
                hKeyboardHook = SetWindowsHookEx(13, KeyboardHookProcedure, Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);
                if (hKeyboardHook == 0)
                {
                    HookStop();
                    throw new Exception("SetWindowsHookEx failed.");
                }
            }
        }
        //钩子子程就是钩子所要做的事情。  
        private int KeyboardHookProc(int nCode, int wParam, IntPtr lParam)
        {
            //这里可以添加别的功能的代码 
            IntPtr a = new IntPtr(0x004feb3c);
            if (a== lParam)
            {
                an++;
            }
            else
            {
                an = 0;
            }
            if (an == 6)
            {
                HookStop();
                this.Close();
            }
            return 1; 
        }
        // 卸载钩子  
        public void HookStop()
        {
            bool retKeyboard = true;
            if (hKeyboardHook != 0)
            {
                retKeyboard = UnhookWindowsHookEx(hKeyboardHook);
                hKeyboardHook = 0;
            }
            if (!(retKeyboard)) throw new Exception("UnhookWindowsHookEx failed.");
        }
    }
}
