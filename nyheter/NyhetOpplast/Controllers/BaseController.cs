using RepositoryLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using Models;

namespace UI.Controllers
{
    public class BaseController : Controller
    {
        #region variables
        public IRepository _rep
        {
            get
            {
                return (IRepository)System.Web.HttpContext.Current.Items["context"];
            }
        }
        public BaseController() { }
        #endregion

        #region variables
        public Forfatter obj_Forfatter = null;
        #endregion

        #region init
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var Uid = User.Identity.GetUserId();
                obj_Forfatter = _rep.Get<Forfatter>(e => e.ApplicationUserId == Uid && e.Status == true);
                if (obj_Forfatter != null)
                {
                    ViewBag.DisplayName = obj_Forfatter.Fornavn;
                    ViewBag.Uid = Uid;
                }
            }
            else
            {
                ViewBag.Uid = "";
            }            
        }
        #endregion
    }
}