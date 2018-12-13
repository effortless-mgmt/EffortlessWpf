using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace EffortlessStdLibrary.Models
{
    public class TokenDetailModel
    {
        public int Exp { get; set; }
        public DateTime ExpireDate => new DateTime(Exp);

        public static TokenDetailModel CreateFromToken(string jwtToken)
        {
            var tokenStr = Encoding.UTF8.GetString(Convert.FromBase64String(jwtToken?.Split('.')[1]));
            return JsonConvert.DeserializeObject<TokenDetailModel>(tokenStr);
        }
    }
}
