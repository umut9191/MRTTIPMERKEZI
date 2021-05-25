using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRTTIPMERKEZI.Models
{
    public class Randevu
    {
        public int id { get; set; }
        public string TC { get; set; }
        public  string AdSoyad { get; set; }
        public string BabaAdi { get; set; }
        public DateTime DogumTarihi { get; set; }
        public DateTime RandevuTarihi { get; set; }
        public string Doktorunuz { get; set; }

    }
}