using ChatClient.MVVM.Core;
using ChatClient.MVVM.Model;
using ChatClient.Net;
using System.Collections.ObjectModel;
using System.Windows;

namespace ChatClient.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        public ObservableCollection<UserModel> Users { get; set; }
        // public ObservableCollection<string> Messages { get; set; }
        public RelayCommand ConnectToServerCommand { get; set; }
        public RelayCommand SendMessageCommand { get; set; }
        private Server server;
        //public string Message { get; set; }
        public string Username { get; set; }






        public ObservableCollection<MessageModel> Messages { get; set; }
        public ObservableCollection<ContactModel> Contacts { get; set; }
        private string _message;
        public RelayCommand SendCommand { get; set; }

        public string Message
        {
            get { return _message; }
            set 
            {
                _message = value;
                OnPropertyChanged();
            }
        }
        private ContactModel _selectedContact;
            
        public ContactModel SelectedContact
        {
            get { return _selectedContact; }
            set 
            { 
                _selectedContact = value;
                OnPropertyChanged();
            }
        }


        public MainViewModel()
        {
            Messages = new ObservableCollection<MessageModel>();
            Contacts = new ObservableCollection<ContactModel>();

            SendCommand = new RelayCommand(o =>
            {
                Messages.Add(new MessageModel()
                {
                    Message = Message,
                    FirstMessage = false
                });

                Message = "";
            });

            Messages.Add(new MessageModel
            {
                Username = "Allison",
                UsernameColor = "#409aff",
                ImageSource = "https://i.imgur.com/yMWvLXd.png",
                Message = "Test",
                Time = DateTime.Now,
                IsNativeOrigin = false,
                FirstMessage = true,
            });

            for (int i = 0; i < 3; i++)
            {
                Messages.Add(new MessageModel
                {
                    Username = "Allison",
                    UsernameColor = "#409aff",
                    ImageSource = "https://i.imgur.com/yMWvLXd.png",
                    Message = "Test",
                    Time = DateTime.Now,
                    IsNativeOrigin = false,
                    FirstMessage = false,

                });
            }

            for (int i = 0; i < 4; i++)
            {
                Messages.Add(new MessageModel
                {
                    Username = "bunnz",
                    UsernameColor = "#409aff",
                    ImageSource = "https://i.imgur.com/yMWvLXd.png",
                    Message = "Test",
                    Time = DateTime.Now,
                    IsNativeOrigin = false,
                });
            }

            Messages.Add(new MessageModel
            {
                Username = "bunnz",
                UsernameColor = "#409aff",
                ImageSource = "https://i.imgur.com/yMWvLXd.png",
                Message = "Last",
                Time = DateTime.Now,
                FirstMessage = true,
                IsNativeOrigin = false,
            });

            for (int i = 0; i < 5; i++)
            {
                Contacts.Add(new ContactModel()
                {
                    Username = $"Allison {i}",
                    ImageSource = "https://i.imgur.com/i2szTsp.png",
                    Messages = Messages
                });
            }


            Users = new ObservableCollection<UserModel>();
            //Messages = new ObservableCollection<string>();
            server = new Server();
            server.connectedEvent += UserConnected;
            server.msgReceivedEvent += MessageReceived;
            server.userDisconnectedEvent += RemoveUser;
            ConnectToServerCommand = new RelayCommand(o => server.ConnectToServer(Username), o=> !string.IsNullOrEmpty(Username));
            SendMessageCommand = new RelayCommand(o => server.SendMesssageToServer(Message), o=> !string.IsNullOrEmpty(Message));
        }

        private void RemoveUser()
        {
            var uid = server.packetReader.ReadMessage();
            var user = Users.Where(x => x.UID == uid).FirstOrDefault();
            Application.Current.Dispatcher.Invoke(() => { Users.Remove(user); });
        }
        private void MessageReceived()
        {
            var msg = server.packetReader.ReadMessage();
            //Application.Current.Dispatcher.Invoke(() => Messages.Add(msg));
        }

        private void UserConnected()
        {
            var user = new UserModel
            {
                Username = server.packetReader.ReadMessage(),
                UID = server.packetReader.ReadMessage(),
            };

            if(!Users.Any(x => x.UID == user.UID))
            {
                Application.Current.Dispatcher.Invoke(() => Users.Add(user));
            }
        }

    }
}
