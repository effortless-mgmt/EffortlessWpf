using System;
using System.Collections.Generic;
using System.Text;

namespace EffortlessStdLibrary.Models
{
    public class AddressModel : IEffortlessModel
    {
        public long Id { get; set; }
        public string Street { get; set; }
        public int ZipCode { get; set; }
        public string City { get; set; }
        public string ReadableString => $"{ZipCode} {City}, {Street}";
    }
}
