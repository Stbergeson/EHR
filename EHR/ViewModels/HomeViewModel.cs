using EHR.Models.Users;

namespace EHR.ViewModels
{
    public class HomeViewModel : BaseViewModel, IPageViewModel
    {
        User user;

        public HomeViewModel()
        {

        }

        public object _User
        {
            get { return user; }
            set { user = (User)value; }
        }
    }

}
