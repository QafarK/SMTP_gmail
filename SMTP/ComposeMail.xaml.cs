using System.ComponentModel;
using System.Net.Mail;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows;

namespace SMTP;

public partial class ComposeMail : Window, INotifyPropertyChanged
{
    private bool ccVisiblity;
    private bool bccVisiblity;
    private string destination;
    private string ccTxt;
    private string bccTxt;
    private string content="";

    public bool CcVisiblity { get => ccVisiblity; set { ccVisiblity = value; OnPropertyChanged(); } }
    public bool BccVisiblity { get => bccVisiblity; set { bccVisiblity = value; OnPropertyChanged(); } }
    public string BccTxt { get => bccTxt; set { bccTxt = value; OnPropertyChanged(); } }
    public string CcTxt { get => ccTxt; set { ccTxt = value; OnPropertyChanged(); } }
    public string Destination { get => destination; set { destination = value; OnPropertyChanged(); } }
    public string MailContent { get => content; set { content = value; OnPropertyChanged(); } }
    public ComposeMail()
    {
        InitializeComponent();
        DataContext = this;
        CcVisiblity = false;
        BccVisiblity = false;
    }

    private void BccBtn_Click(object sender, RoutedEventArgs e)
    {
        BccVisiblity = !BccVisiblity;
    }

    private void CcBtn_Click(object sender, RoutedEventArgs e)
    {
        CcVisiblity = !CcVisiblity;
    }



    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void SendBtn_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("cripnocy@gmail.com");
            mail.To.Add(Destination);
            if (BccVisiblity)
                mail.Bcc.Add(BccTxt); 
            if (CcVisiblity)
                mail.CC.Add(CcTxt);
            mail.Subject = string.Empty;
            mail.Body = MailContent?.ToString() ?? "Null content";

            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential("cripnocy@gmail.com", "hjfq qqnq erwr ytue");
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }

        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}





