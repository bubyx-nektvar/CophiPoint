using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using CophiPoint.Api.Models;

namespace CophiPoint.ViewModels
{
    public class UserViewModel : INotifyPropertyChanged
    {
        private string _user;
        private decimal _balance;

        public event PropertyChangedEventHandler PropertyChanged;

        public string User {
            get => _user;
            set
            {
                if (_user != value)
                {
                    _user = value;
                    OnPropertyChanged(nameof(User));
                }
            }
        }

        public decimal Balance {
            get => _balance;
            set
            {
                if (_balance != value)
                {
                    _balance = value;
                    OnPropertyChanged(nameof(Balance));
                }
            }
        }

        public UserViewModel() { }
        public void Set(AccountInfo info)
        {
            User = info.Email;
            Balance = info.Balance;
            //TODO changes notifications
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
