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
    public class CreateInfoViewModel : INotifyPropertyChanged
    {
        public CreateInfoViewModel(ref MainModel mainModel)
        {
            model = mainModel;

            Id = StringHelper.IdTitle;
            Password = StringHelper.PasswordTitle;
            Info = StringHelper.CreateInfoInfoTitle;
        }

        #region String constants
        public string CreateInfoPath => ResourcesHelper.CreateInfoPath;
        public string CancelPath => ResourcesHelper.CancelPath;
        public string CreateInfoTitle => StringHelper.CreateInfoTitle;
        public string CancelTitle => StringHelper.CancelTitle;
        #endregion

        #region Fields
        private readonly MainModel model;

        private string id;
        private string password;
        private string info;

        private RelayCommand createInfoCommand;
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
        public RelayCommand CreateInfoCommand => createInfoCommand ?? (createInfoCommand = new RelayCommand(obj =>
        {
            Info = model.AddInfo(Id, Password);
        }));
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        #endregion
    }
}
