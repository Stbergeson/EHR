using EHR.Commands;
using System.Windows.Input;
using DataAccess;
using EHR.Models.Users;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace EHR.ViewModels
{


    public class SplashViewModel : BaseViewModel, IPageViewModel
    {
        private ICommand _goToMain;

        SQLDataAccess _db;
        UserContext userContext;
        User user;
        List<User> users;
        string password;
        string errorMessage;

        public string Username 
        {
            get
            {
                return user.Username;
            }

            set
            {
                user.Username = value;
            }
        }

        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }

        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }

            set
            {
                errorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
        }

        public SplashViewModel()
        {
            _db = new SQLDataAccess();
            userContext = new UserContext(_db);
            user = new User();
        }

        public string HashPass(string value)
        {
            using (SHA256 mySHA256 = SHA256.Create())
            {
                byte[] bytes = Encoding.ASCII.GetBytes(value);
                byte[] hashValue = mySHA256.ComputeHash(bytes);
                return ByteArrayToString(hashValue);
            }
        }

        public string ByteArrayToString(byte[] array)
        {
            string str = "";

            for (int i = 0; i < array.Length; i++)
            {
                str += $"{array[i]:X2}";
            }

            return str;
        }

        public async Task Verify()
        {
            if(user.Username != null && password != null)
            {
                user.Password = HashPass(password);
                users = await userContext.VerifyLogin(user);
                if(users.Count == 1)
                {
                    user = users[0];
                } 
            }
        }

        public ICommand GoToMain
        {
            get
            {
                try {
                    return _goToMain ?? (_goToMain = new RelayCommand(x =>
                    {
                        AsyncContext.Run(Verify);
                        if (users != null && users.Count == 1)
                            Mediator.Notify("GoToMainScreen", user);
                        else
                            ErrorMessage = "Username/Password incorrect";
                    }));
                }
                catch
                {
                    return _goToMain;
                }

            }
        }

    }
}
