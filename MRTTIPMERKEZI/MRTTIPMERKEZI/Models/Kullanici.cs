using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRTTIPMERKEZI.Models
{
    public class Kullanici
    {
        public int id { get; set; }
        
        public string AdSoyad { get; set; }
        public string Email { get; set; }
        public string Sifre { get; set; }
    }
}