using System;
using System.Collections.Generic;
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
    public class TakeMoneyViewModel : INotifyPropertyChanged
    {
        public TakeMoneyViewModel(ref MainModel mainModel)
        {
            model = mainModel;

            Id = StringHelper.IdTitle;
            Password = StringHelper.PasswordTitle;
            Info = StringHelper.TakeMoneyInfoTitle;
        }

        #region Fields
        private readonly MainModel model;

        private string id;
        private string password;
        private string info;

        private RelayCommand takeMoneyCommand;
        #endregion

        #region String constants
        public string TakeMoneyPath => ResourcesHelper.TakeMoneyPath;
        public string CancelPath => ResourcesHelper.CancelPath;
        public string TakeMoneyTitle => StringHelper.TakeMoneyTitle;
        public string CancelTitle => StringHelper.CancelTitle;
        #endregion

        #region Properties
        public string Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }

        public string Info
        {
            get => info;
            set
            {
                info = value;
                OnPropertyChanged("Info");
            }
        }
        #endregion

        #region Commands
        public RelayCommand TakeMoneyCommand => takeMoneyCommand ?? (takeMoneyCommand = new RelayCommand(obj =>
        {
            Info = model.TakeMoney(Id, Password, Info);
        }));
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        #endregion
    }
}
