using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Controllers
{
    [Authorize]
    public class AdminController : BaseController
    {
        public ActionResult RedigerNyheter()
        {
            var list = _rep.GetAll<Nyhet>(e => e.ForfatterId == obj_Forfatter.Id).ToList();
            return View(list);
        }
        public ActionResult VisAlleNyheter()
        {
            var list = _rep.GetAll<Nyhet>(e => e.ForfatterId == obj_Forfatter.Id).ToList();
            return View(list);
        }

        public ActionResult LagNyNyhet(Guid? id)
        {
            if (id.HasValue)
            {
                var objNyhet = _rep.Get<Nyhet>(e => e.Id == id.Value);
                if (objNyhet != null)
                {
                    ViewBag.Title = "Rediger nyhet";
                    return View(objNyhet);
                }
                else
                    return View("Error");
            }
            ViewBag.Title = "Lag ny nyhet";
            return View(new Nyhet());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LagNyNyhet(Nyhet obj, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (obj.Id != Guid.Empty)
                {
                    var oldobjNyhet = _rep.Get<Nyhet>(e => e.Id == obj.Id);
                    if (oldobjNyhet != null)
                    {
                        oldobjNyhet.Tittel = obj.Tittel;
                        oldobjNyhet.Tekst = obj.Tekst;
                        TempData["msg"] = "Nyhet updated sucessfully!!";
                    }
                    else
                        return View("Error");
                }
                else
                {
                    if (image != null)
                    {
                        obj.BildeSrc = new byte[image.ContentLength];
                        image.InputStream.Read(obj.BildeSrc, 0, image.ContentLength);
                        obj.ForfatterId = obj_Forfatter.Id;
                        obj.DatoPostet = DateTime.Now;
                        _rep.Add<Nyhet>(obj);
                        TempData["msg"] = "Nyhet added sucessfully!!";
                    }
                    else
                    {
                        ModelState.AddModelError("", "Please upload a file");
                        return View(obj);
                    }
                }
                _rep.SaveChanges();
                return RedirectToAction("LagNyNyhet");
            }
            return View(obj);
        }


        public ActionResult VisEnNyhet(Guid Id)
        {
            if (Id != null)
            {
                var objNyhet = _rep.Get<Nyhet>(e => e.Id == Id);
                if (objNyhet != null)
                    return View(objNyhet);
            }
            return View("Error");
        }

        public ActionResult SlettNyhet(Guid id)
        {
            if (id != null)
            {
                var objNyhet = _rep.Get<Nyhet>(e => e.Id == id);
                if (objNyhet != null)
                    return View(objNyhet);
            }
            return View("Error");
        }

        [HttpPost]
        public ActionResult SlettNyhet(Nyhet obj)
        {
            if (obj != null)
            {
                var objNyhet = _rep.Get<Nyhet>(e => e.Id == obj.Id);
                if (objNyhet != null)
                {
                    _rep.Delete<Nyhet>(objNyhet);
                    _rep.SaveChanges();
                    TempData["msg"] = "Nyhet deleted sucessfully!!";
                    return RedirectToAction("RedigerNyheter");
                }
            }
            return View("Error");
        }

    }
}