using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnThiBaBang.Models;

namespace OnThiBaBang.Controllers
{
    public class HoaDonBansController : Controller
    {
        private Database1Entities db = new Database1Entities();

        // GET: HoaDonBans
        public ActionResult Index(String tenkh)
        {
            //ViewBag.tenkh = new SelectList(db.KhachHangs, "tenkh", "tenkhh");

            var hoaDonBans = db.HoaDonBans.Include(h => h.KhachHang).Include(h => h.Sach);
            if (!string.IsNullOrEmpty(tenkh))
            {
                hoaDonBans = db.HoaDonBans.Where(item=>item.KhachHang.HoTen.Contains(tenkh));
            }
            return View(hoaDonBans.ToList());
        }
        
        public ActionResult Cau10 ()
        {
            var max = db.HoaDonBans.Include(h => h.KhachHang).Include(h => h.Sach).Max(item=> item.SoLuong);
            var hoaDonBans = db.HoaDonBans.Where(item => item.SoLuong == max);
            return View(hoaDonBans.ToList());
        }

        public ActionResult Cau9()
        {
            
            var Summax = db.HoaDonBans
                .GroupBy(item => item.MaSach)
                .Select(k => new
                {
                    MaSach = k.Key,
                    Tongdoanhthu = k.Sum(item => item.SoLuong * item.Sach.DonGia)
                })
                .OrderByDescending(item => item.Tongdoanhthu)
                .FirstOrDefault();

            var hoaDonBans = db.Saches.Where(item=>item.MaSach==Summax.MaSach);

            return View(hoaDonBans.ToList());
        }

        // GET: HoaDonBans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoaDonBan hoaDonBan = db.HoaDonBans.Find(id);
            if (hoaDonBan == null)
            {
                return HttpNotFound();
            }
            return View(hoaDonBan);
        }

        // GET: HoaDonBans/Create
        public ActionResult Create()
        {
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "HoTen");
            ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach");
            return View();
        }

        // POST: HoaDonBans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaHD,MaSach,MaKH,NgayBan,SoLuong")] HoaDonBan hoaDonBan)
        {
            if (ModelState.IsValid)
            {
                db.HoaDonBans.Add(hoaDonBan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "HoTen", hoaDonBan.MaKH);
            ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", hoaDonBan.MaSach);
            return View(hoaDonBan);
        }

        // GET: HoaDonBans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoaDonBan hoaDonBan = db.HoaDonBans.Find(id);
            if (hoaDonBan == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "HoTen", hoaDonBan.MaKH);
            ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", hoaDonBan.MaSach);
            return View(hoaDonBan);
        }

        // POST: HoaDonBans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaHD,MaSach,MaKH,NgayBan,SoLuong")] HoaDonBan hoaDonBan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hoaDonBan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "HoTen", hoaDonBan.MaKH);
            ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", hoaDonBan.MaSach);
            return View(hoaDonBan);
        }

        // GET: HoaDonBans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoaDonBan hoaDonBan = db.HoaDonBans.Find(id);
            if (hoaDonBan == null)
            {
                return HttpNotFound();
            }
            return View(hoaDonBan);
        }

        // POST: HoaDonBans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HoaDonBan hoaDonBan = db.HoaDonBans.Find(id);
            db.HoaDonBans.Remove(hoaDonBan);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
