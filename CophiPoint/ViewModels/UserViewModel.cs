using System;
using System.Collections.Generic;
using System.Text;
using CophiPoint.Api.Models;

namespace CophiPoint.ViewModels
{
    public class UserViewModel
    {

        public UserViewModel() { }
        public void Set(AccountInfo info)
        {
            User = info.Email;
            Balance = info.Balance;
            //TODO changes notifications
        }

        public string User { get; set; }

        public decimal Balance { get; set; }
    }
}
