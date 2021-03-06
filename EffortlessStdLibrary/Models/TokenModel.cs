﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EffortlessStdLibrary.Models
{
    public class TokenModel
    {
        public UserModel User { get; set; }
        public string Token { get; set; }
        public TokenDetailModel TokenDeserialized => TokenDetailModel.CreateFromToken(Token);
    }
}
