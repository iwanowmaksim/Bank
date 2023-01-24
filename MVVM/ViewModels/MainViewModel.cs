using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Xml.Linq;
using Core;
using Helpers;
using Microsoft.Win32;

namespace Bank
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            model = new MainModel();
        }

        #region Fields
        private MainModel model;

        private RelayCommand openCheckBalanceCommand;
        private RelayCommand openCreateInfoCommand;
        private RelayCommand openGiveMoneyCommand;
        private RelayCommand openTakeMoneyCommand;
        #endregion

        #region String constants
        public string LogoImagePath => ResourcesHelper.LogoImagePath;
        public string CheckBalancePath => ResourcesHelper.CheckBalancePath;
        public string CreateInfoPath => ResourcesHelper.CreateInfoPath;
        public string GiveMoneyPath => ResourcesHelper.GiveMoneyPath;
        public string TakeMoneyPath => ResourcesHelper.TakeMoneyPath;
        public string CheckBalanceTitle => StringHelper.CheckBalanceTitle;
        public string CreateInfoTitle => StringHelper.CreateInfoTitle;
        public string GiveMoneyTitle => StringHelper.GiveMoneyTitle;
        public string TakeMoneyTitle => StringHelper.TakeMoneyTitle;
        #endregion

        #region Commands
        public RelayCommand OpenCheckBalanceCommand => openCheckBalanceCommand ?? (openCheckBalanceCommand = new RelayCommand(obj =>
        {
            Window checkBalanceView = new CheckBalanceView()
            {
                DataContext = new CheckBalanceViewModel(ref model)
            };
            checkBalanceView.ShowDialog();
        }));

        public RelayCommand OpenCreateInfoCommand => openCreateInfoCommand ?? (openCreateInfoCommand = new RelayCommand(obj =>
        {
            Window createInfoView = new CreateInfoView()
            {
                DataContext = new CreateInfoViewModel(ref model)
            };
            createInfoView.ShowDialog();
        }));

        public RelayCommand OpenGiveMoneyCommand => openGiveMoneyCommand ?? (openGiveMoneyCommand = new RelayCommand(obj =>
        {
            Window giveMoneyView = new GiveMoneyView()
            {
                DataContext = new GiveMoneyViewModel(ref model)
            };
            giveMoneyView.ShowDialog();
        }));

        public RelayCommand OpenTakeMoneyCommand => openTakeMoneyCommand ?? (openTakeMoneyCommand = new RelayCommand(obj =>
        {
            Window takeMoneyView = new TakeMoneyView()
            {
                DataContext = new TakeMoneyViewModel(ref model)
            };
            takeMoneyView.ShowDialog();
        }));
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        #endregion
    }
}
