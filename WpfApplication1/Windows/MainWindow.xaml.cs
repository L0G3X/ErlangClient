using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Info;

namespace Login
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            IdBox.MaxLength = 13;
            PasswordBox.MaxLength = 20;            
        }

        //Login
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(IdBox.Text) && string.IsNullOrEmpty(PasswordBox.Password))
                Log.Text = "ID와 Password를 제대로 입력해 주세요";
            else
            {
                var response = Global.SendData($"id={IdBox.Text}&password={PasswordBox.Password}", Global.Type.Login);

                IdBox.Text = String.Empty;
                PasswordBox.Password = String.Empty;

                if (!response.Equals("ERROR"))
                {
                    var child = new Window1(response)
                    {
                        Owner = LoginWindow
                    };
                    child.Show();
                    LoginWindow.Hide();
                }
            }
        }
        //Join
        private void Join_Click(object sender, RoutedEventArgs e)
        {   
            if (string.IsNullOrEmpty(IdBox.Text) && string.IsNullOrEmpty(PasswordBox.Password))
                IdBox.Text = "ID와 Password를 제대로 입력해 주세요";
            else
            {
                switch (Global.SendData($"id={IdBox}&password={PasswordBox.Password}", Global.Type.Join))
                {
                    case "ok":
                        Log.Text = "가입완료";
                        break;
                    case "duplicataed":
                        Log.Text = "이미 있는 아이디입니다.";
                        break;
                    default:
                        Log.Text = "ERROR";
                        break;
                }
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.Escape:
                    Close();
                    break;
                case Key.Enter:
                    Login_Click(sender, e);
                    break;
            }
        }
    }
}
