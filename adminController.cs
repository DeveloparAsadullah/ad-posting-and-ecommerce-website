using ad_posting_eproject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace ad_posting_eproject.Controllers
{
    public class adminController : Controller
    {
        ad_posting_projectEntities db = new ad_posting_projectEntities();
        // GET: admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult admin()
        {


            return View();
        }

        [HttpPost]

        public ActionResult admin(tbl_admin ad)
        {

            tbl_admin a = db.tbl_admin.Where(x => x.ad_email == ad.ad_email && x.ad_password == ad.ad_password).SingleOrDefault();

            if (a!=null)
            {
                Session["name"] = a.ad_name;
                Session["id"] = a.ad_id;
                return RedirectToAction("category");
            }
            else
            {
                ViewBag.error = "invalid email and password";
            }
           
            return View();

        }

        public ActionResult category()
        {


            return View();
        }

        [HttpPost]

        public ActionResult category(tbl_cateogry ca,HttpPostedFileBase img)
        {

            tbl_admin ad = new tbl_admin();
            string path = upload_image(img);
            if (path.Equals("-1"))
            {
                ViewBag.erorr = "image could not uploaded";
            }

            else
            {
                tbl_cateogry c = new tbl_cateogry();
                c.cat_name = ca.cat_name;
                c.cat_status = 1;
                c.cat_image = path;
                c.ad_id_fk = Convert.ToInt32(Session["id"].ToString());
                db.tbl_cateogry.Add(c);
                db.SaveChanges();
                return RedirectToAction("viewcategory");
            }



            return View();
        }


        //image uploading code

        public string upload_image(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1";

            int random = r.Next();

            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".png") || extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg"))
                {

                    try
                    {
                        path = Path.Combine(Server.MapPath("~/Content/upload"), random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "~/Content/upload/" + random + Path.GetFileName(file.FileName);

                    }
                    catch (Exception ex)
                    {
                        path = "-1";

                        throw;
                    }

                }
                else
                {
                    Response.Write("<script>alert('only jpeg ,jpg,png formate is allowed')</script>");

                }


            }
            else
            {
                Response.Write("<script>alert('please select file')</script>");
                path = "-1";

            }

            return path;
        }

        public ActionResult viewcategory(int ? page)
        {
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.tbl_cateogry.Where(x => x.cat_status == 1).OrderByDescending(x => x.cat_id).ToList();
            IPagedList<tbl_cateogry> cate = list.ToPagedList(pageindex, pagesize);


            return View(cate);
        }



    }
}