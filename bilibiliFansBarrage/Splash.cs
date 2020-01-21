using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace bilibiliFansBarrage
{
    public class Splash
    {
        private static object _obj = new object();

        private static Form _SplashForm = null;

        private static Thread _SplashThread = null;

        private delegate void ChangeFormTextdelegate(string s);

        public static void Show(Type splashFormType)
        {
            if (_SplashThread != null)
                return;
            if (splashFormType == null)
            {
                throw (new Exception());
            }

            _SplashThread = new Thread(new ThreadStart(delegate ()
            {
                CreateInstance(splashFormType);
                Application.Run(_SplashForm);
            }));

            _SplashThread.IsBackground = true;
            _SplashThread.SetApartmentState(ApartmentState.STA);
            _SplashThread.Start();
        }

        public static void ChangeTitle(string status)
        {
            ChangeFormTextdelegate de = new ChangeFormTextdelegate(ChangeText);
            _SplashForm.Invoke(de, status);
        }



        public static void Close()
        {
            if (_SplashThread == null || _SplashForm == null) return;

            try
            {
                _SplashForm.Invoke(new MethodInvoker(_SplashForm.Close));
            }
            catch (Exception)
            {
            }
            _SplashThread = null;
            _SplashForm = null;
        }

        private static void ChangeText(string title)
        {
            _SplashForm.Text = title.ToString();
        }



        private static void CreateInstance(Type FormType)
        {
            if (_SplashForm == null)
            {
                lock (_obj)
                {
                    object obj = FormType.InvokeMember(null,
                                        BindingFlags.DeclaredOnly |
                                        BindingFlags.Public | BindingFlags.NonPublic |
                                        BindingFlags.Instance | BindingFlags.CreateInstance, null, null, null);
                    _SplashForm = obj as Form;
                    _SplashForm.TopMost = true;
                    _SplashForm.ShowInTaskbar = false;
                    _SplashForm.BringToFront();
                    _SplashForm.StartPosition = FormStartPosition.CenterScreen;
                    if (_SplashForm == null)
                    {
                        throw (new Exception());
                    }
                }
            }
        }
    }
}
