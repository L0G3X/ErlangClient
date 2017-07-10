using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Login;
using WpfApplication1;

namespace Info
{
    /// <summary>
    /// Window1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Window1 : Window
    {
        //Initialize
        public Window1(string session)
        {            
            InitializeComponent();

            //Tab Indexes
            PointInputBox.TabIndex = 1;

            //Input Length;
            PointInputBox.MaxLength = 8;

            //session key
            sessionBox.Text = session;
        }

        //Logout
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            Global.SendData($"session_key={sessionBox.Text}", Global.Type.Logout);
            Close();
        }

        //Sending Point
        private void SendPoint_Click(object sender, RoutedEventArgs e)
        {
            PointBox.Text = Global.SendData($"session_key={sessionBox.Text}&point={PointInputBox.Text}", Global.Type.SendPoint);
            PointInputBox.Text = string.Empty;
        }

        //Key input
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Logout_Click(sender, e);
                    break;
            }
        }

        private void PointInputBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    SendPoint_Click(sender, e);
                    break;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Global.SendData($"session_key={sessionBox.Text}", Global.Type.Logout);
        }
    }
}