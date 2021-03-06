﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EffortlessStdLibrary.Models
{
    public class UserModel : IEffortlessModel
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string FullNameCapitalized => System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(FullName.ToLower());
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
    }
}
