using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SMTP;

public partial class MainWindow : Window, INotifyPropertyChanged
{
    private ObservableCollection<MimeMessage> inboxMails = new();
    private ObservableCollection<MimeMessage> sentMails = new();
    private ObservableCollection<MimeMessage> snoozedMails = new();
    private ObservableCollection<MimeMessage> draftMails = new();
    private ObservableCollection<MimeMessage> starredMails = new();


    public ObservableCollection<MimeMessage> InboxMails { get => inboxMails; set { inboxMails = value; OnPropertyChanged(); } }
    public ObservableCollection<MimeMessage> SentMails { get => sentMails; set { sentMails = value; OnPropertyChanged(); } }
    public ObservableCollection<MimeMessage> SnoozedMails { get => snoozedMails; set { snoozedMails = value; OnPropertyChanged(); } }
    public ObservableCollection<MimeMessage> DraftMails { get => draftMails; set { draftMails = value; OnPropertyChanged(); } }
    public ObservableCollection<MimeMessage> StarredMails { get => starredMails; set { starredMails = value; OnPropertyChanged(); } }

    public MainWindow()
    {
        InitializeComponent();
        var a = new MimeMessage();
        DataContext = this;
        InboxCommand = new RelayCommand(InboxCommandExecute);
        SentCommand = new RelayCommand(SentCommandExecute);
        StarredCommand = new RelayCommand(StarredCommandExecute);
        SnoozedCommand = new RelayCommand(SnoozedCommandExecute);
        DraftCommand = new RelayCommand(DraftCommandExecute);
    }
    public async Task<(IList<UniqueId> ids, IMailFolder folder)> FunctionExecuter(ListView list, ImapClient imap,  SpecialFolder? specialFolder, string host = "imap.gmail.com",
        int port = 993, string username = "cripnocy@gmail.com", string password = "hjfq qqnq erwr ytue")
    {
        await imap.ConnectAsync(host, port, true);
        await imap.AuthenticateAsync(username, password);

        IMailFolder folder;
        if (specialFolder is null)
            folder = imap.GetFolder("Inbox");
        else
            folder = imap.GetFolder(specialFolder.Value);

        await folder.OpenAsync(FolderAccess.ReadOnly);
        var ids = await folder.SearchAsync(SearchQuery.All);

        return (ids, folder);

    }

    public ICommand InboxCommand { get; set; }
    public async void InboxCommandExecute(object? obj)
    {
        if (obj is ListView list)
        {
            try
            {
                using var imap = new ImapClient();
 
                var (ids, folder) = await FunctionExecuter(list, imap, null);
                InboxMails = new ObservableCollection<MimeMessage>(); 
                list.ItemsSource = InboxMails;

                foreach (var id in ids)
                {
                    var message = await folder.GetMessageAsync(id);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        InboxMails.Add(message);
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

    public ICommand SentCommand { get; set; }
    public async void SentCommandExecute(object? obj)
    {
        if (obj is ListView list)
        {
            try
            {
                using var imap = new ImapClient();

                var (ids, folder) = await FunctionExecuter(list, imap, null);

                SentMails = [];
                list.ItemsSource = SentMails;
                foreach (var id in ids)
                {
                    var message = await folder.GetMessageAsync(id);
                    Application.Current.Dispatcher.Invoke(new(() =>
                    {
                        SentMails.Add(message);
                    }));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

    public ICommand StarredCommand { get; set; }
    public async void StarredCommandExecute(object? obj)
    {
        if (obj is ListView list)
        {
            try
            {
                using var imap = new ImapClient();

                var (ids, folder) = await FunctionExecuter(list, imap, null);

                StarredMails = [];
                list.ItemsSource = StarredMails;
                foreach (var id in ids)
                {
                    var message = await folder.GetMessageAsync(id);
                    Application.Current.Dispatcher.Invoke(new(() =>
                    {
                        StarredMails.Add(message);
                    }));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

    public ICommand SnoozedCommand { get; set; }
    public async void SnoozedCommandExecute(object? obj)
    {
        if (obj is ListView list)
        {
            try
            {
                using var imap = new ImapClient();

                var (ids, folder) = await FunctionExecuter(list, imap, null);

                SnoozedMails = [];
                list.ItemsSource = SnoozedMails;
                foreach (var id in ids)
                {
                    var message = await folder.GetMessageAsync(id);
                    Application.Current.Dispatcher.Invoke(new(() =>
                        SnoozedMails.Add(message)
                    ));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

    public ICommand DraftCommand { get; set; }
    public async void DraftCommandExecute(object? obj)
    {
        if (obj is ListView list)
        {
            try
            {
                using var imap = new ImapClient();

                var (ids, folder) = await FunctionExecuter(list, imap, null);

                DraftMails = [];
                list.ItemsSource = DraftMails;
                foreach (var id in ids)
                {
                    var message = await folder.GetMessageAsync(id);
                    Application.Current.Dispatcher.Invoke(new(() =>
                    {
                        DraftMails.Add(message);
                    }));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }


    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        var window = new ComposeMail();
        window.Show();
    }

    private void ListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        var window = new ComposeMail();
        window.Show();
    }
}