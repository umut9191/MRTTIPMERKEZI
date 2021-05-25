using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MRTTIPMERKEZI.Models;

namespace MRTTIPMERKEZI.Controllers
{
    public class HomeController : Controller
    {

        MRTTIPMERKEZIDBEntities _db = new MRTTIPMERKEZIDBEntities();
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.RandevuAldayiz = false;
            return View();
        }

        public ActionResult GirisYap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GirisYap(Kullanici k)
        {
            string email = k.Email;
            string sifre = k.Sifre;


            var kullanici = from a in _db.Kullanicilar_tbl where a.Email == k.Email && a.Sifre.Equals(k.Sifre) select a;
            if (kullanici.Count() > 0)
            {
                foreach (var item in kullanici)
                {                
                        Session.Add("kullanici", item);
                    break;
                }
            }
            else
            {
                ViewBag.GirisTeHata = true;
                return View();
            }
            return RedirectToAction("ButunRandevular");
        }
        public ActionResult ButunRandevular()
        {
            if (Session["kullanici"] == null)
            {
                return RedirectToAction("GirisYap");
            }

            ViewBag.RandevuAldayiz = true;
            
            var randevular = from i in _db.Randevu_tbl select i;
          
            List<Randevu> randevuliste = new List<Randevu>();
            foreach (var item in randevular)
            {
                Randevu randevu = new Randevu();
                randevu.id = item.id;
                randevu.AdSoyad = item.AdSoyad;
                randevu.BabaAdi = item.BabaAdi;
                randevu.DogumTarihi = (DateTime)item.DogumTarihi;
                randevu.RandevuTarihi = (DateTime)item.RandevuTarihi;
                randevu.TC = item.TC;
                randevu.Doktorunuz = item.Doktorunuz;
                randevuliste.Add(randevu);
            }
            return View(randevuliste);

           
        }
        public ActionResult RandevuAl()
        {
            ViewBag.RandevuAldayiz = true;
            return View();
        }
        [HttpPost]
        public ActionResult Randevulariniz(FormCollection frm)
        {
            ViewBag.RandevuAldayiz = true;
            string txtTC = frm.Get("txtTC").Trim();
            var VarMi = from i in _db.Randevu_tbl where i.TC == txtTC select i;

            if (!(VarMi.Count()>0))
            {
                return RedirectToAction("RandevuAl");
            }
            List<Randevu> randevuliste = new List<Randevu>();
            foreach (var item in VarMi)
            {
                Randevu randevu = new Randevu();
                randevu.id = item.id;
                randevu.AdSoyad = item.AdSoyad;
                randevu.BabaAdi = item.BabaAdi;
                randevu.DogumTarihi = (DateTime)item.DogumTarihi;
                randevu.RandevuTarihi = (DateTime)item.RandevuTarihi;
                randevu.TC = item.TC;
                randevu.Doktorunuz = item.Doktorunuz;
                randevuliste.Add(randevu);
            }
            return View(randevuliste);
        }
        [HttpPost]
        public ActionResult RandevuEkle(FormCollection frm)
        {
            string txtTC = frm.Get("txtTCEkle").Trim();
            string txtAdSoyad = frm.Get("txtAdSoyad").Trim();
            string txtBabaAdi = frm.Get("txtBabaAdi").Trim();
            string takvimDT = frm.Get("takvimDT").Trim();
            string takvimRT = frm.Get("takvimRT").Trim();
            DateTime dogumTarihi = Convert.ToDateTime(takvimDT);
            DateTime randevuTarihi = Convert.ToDateTime(takvimRT);

            string atanacakDoktor = DoktorAtama();
            Randevu_tbl tablo = new Randevu_tbl
            {
                TC = txtTC,
                AdSoyad = txtAdSoyad,
                BabaAdi = txtBabaAdi,
                DogumTarihi = dogumTarihi,
                RandevuTarihi = randevuTarihi,
                Doktorunuz = atanacakDoktor
            };

            _db.Randevu_tbl.Add(tablo);

            _db.SaveChanges();
            return RedirectToAction("RandevuAl");
        }
        public ActionResult RandevuSil(int id)
        {
            var silinecek = _db.Randevu_tbl.First(u => u.id == id);
            _db.Randevu_tbl.Remove(silinecek);
            _db.SaveChanges();
            return RedirectToAction("RandevuAl");
        }
        public ActionResult RandevuGuncelle(int id)
        {
            ViewBag.RandevuAldayiz = true;

            var randevumuz = _db.Randevu_tbl.First(a => a.id == id);
            Randevu randevu = new Randevu();
            randevu.id = randevumuz.id;
            randevu.AdSoyad = randevumuz.AdSoyad;
            randevu.BabaAdi = randevumuz.BabaAdi;
            randevu.DogumTarihi = (DateTime)randevumuz.DogumTarihi;
            randevu.RandevuTarihi = (DateTime)randevumuz.RandevuTarihi;
            randevu.TC = randevumuz.TC;
            randevu.Doktorunuz = randevumuz.Doktorunuz;
            ViewBag.randevu = randevu;


            return View();

        }
        [HttpPost]
        public ActionResult RandevuGuncelle(FormCollection frm)
        {
            string stringid = frm.Get("guncellenecekID").Trim();
            int id = Convert.ToInt32(stringid);

            string txtTC = frm.Get("txtTCEkle").Trim();
            string txtAdSoyad = frm.Get("txtAdSoyad").Trim();
            string txtBabaAdi = frm.Get("txtBabaAdi").Trim();
            string takvimDT = frm.Get("takvimDT").Trim();
            string takvimRT = frm.Get("takvimRT").Trim();

            DateTime dogumTarihi = Convert.ToDateTime(takvimDT);
            DateTime randevuTarihi = Convert.ToDateTime(takvimRT);
            var guncellenecek = _db.Randevu_tbl.First(u => u.id == id);
            guncellenecek.TC = txtTC;
            guncellenecek.AdSoyad = txtAdSoyad;
            guncellenecek.BabaAdi = txtBabaAdi;
            guncellenecek.RandevuTarihi = randevuTarihi;
            guncellenecek.DogumTarihi = dogumTarihi;


            _db.SaveChanges();
            return RedirectToAction("RandevuAl");
        }

        public string DoktorAtama()
        {
            List<string> isimler = new List<string>();
            isimler.Add("Uzm. Dr. Hayriye Atalay");
            isimler.Add("Op. Dr.Mehmet Emin Öztürk");
            isimler.Add("Dr. Muhammed Keliç");
            isimler.Add("Op. Dr.Cenk Tosun");

            string atanacakDoktor = " ";
            Random rastgele = new Random();
            int rastgeleSayi = rastgele.Next(1, 5); //1 ile 5 arasında rastgele bir sayı üretir. 5 dahil değildir.
            int sayac = 0;
            foreach (var item in isimler)
            {
                sayac++;
                if (sayac== rastgeleSayi)
                {
                    atanacakDoktor = item;
                    break;
                }
            }
            return atanacakDoktor;
        }
        public ActionResult Cikis()
        {
            Session.Remove("kullanici");
            return RedirectToAction("GirisYap");
        }
    }
}