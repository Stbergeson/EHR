﻿using EHR.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHR.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private IPageViewModel _currentPageViewModel;
        private List<IPageViewModel> _pageViewModels;

        public List<IPageViewModel> PageViewModels
        {
            get
            {
                if (_pageViewModels == null)
                    _pageViewModels = new List<IPageViewModel>();

                return _pageViewModels;
            }
        }

        public IPageViewModel CurrentPageViewModel
        {
            get
            {
                return _currentPageViewModel;
            }
            set
            {
                _currentPageViewModel = value;
                OnPropertyChanged("CurrentPageViewModel");
            }
        }

        private void ChangeViewModel(IPageViewModel viewModel)
        {
            if (!PageViewModels.Contains(viewModel))
                PageViewModels.Add(viewModel);

            CurrentPageViewModel = PageViewModels
                .FirstOrDefault(vm => vm == viewModel);
        }


        private void OnGoMainScreen(object obj)
        {
            ChangeViewModel(PageViewModels[1]);
            HomeViewModel vm = (HomeViewModel)PageViewModels[1];
            vm._User = obj;
        }



        public MainWindowViewModel()
        {
            // Add available pages and set page
            PageViewModels.Add(new SplashViewModel());
            PageViewModels.Add(new HomeViewModel());

            CurrentPageViewModel = PageViewModels[0];

            Mediator.Subscribe("GoToMainScreen", OnGoMainScreen);
        }
    }
}
