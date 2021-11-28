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
    public class userController : Controller
    {
        ad_posting_projectEntities db = new ad_posting_projectEntities();
        // GET: user
        public ActionResult Index(int? page)
        {
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.tbl_cateogry.Where(x => x.cat_status == 1).OrderByDescending(x => x.cat_id).ToList();
            IPagedList<tbl_cateogry> cate = list.ToPagedList(pageindex, pagesize);



            return View(cate);
        }

        public ActionResult signup()
        {

            return View();
        }

        [HttpPost]

        public ActionResult signup(tbl_user us, HttpPostedFileBase img)
        {
            string path = upload_image(img);
            if (path.Equals("-1"))
            {
                ViewBag.error = "image not uploaded";
            }
            else
            {
                tbl_user u = new tbl_user();

                u.u_name = us.u_name;
                u.u_email = us.u_email;
                u.u_password = us.u_password;
                u.u_contact = us.u_contact;
                u.u_image = path;
                u.ad_id_fk = us.ad_id_fk;
                u.cat_id_fk = us.cat_id_fk;
                db.tbl_user.Add(u);
                db.SaveChanges();
                return RedirectToAction("login");

            }



            return View();
        }


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

        public ActionResult login()
        {

            return View();
        }
        [HttpPost]

        public ActionResult login(tbl_user us)
        {

            tbl_user u = db.tbl_user.Where(x => x.u_email == us.u_email && x.u_password == us.u_password).SingleOrDefault();

            if (u != null)
            {
                Session["u_name"] = u.u_name;
                Session["u_id"] = u.u_id;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.error = "invalid email and password";
            }

            return View();
        }


        public ActionResult logout()
        {
            Session.Abandon();
            Session.RemoveAll();

            return View("Index");
        }


        public ActionResult post_ad()
        {
            tbl_cateogry cat = new tbl_cateogry();

            List<tbl_cateogry> li = db.tbl_cateogry.ToList();
            ViewBag.cat_list = new SelectList(li, "cat_id", "cat_name");


            return View();
        }

        [HttpPost]

        public ActionResult post_ad(tbl_product pr, HttpPostedFileBase img)
        {
            tbl_cateogry cat = new tbl_cateogry();

            List<tbl_cateogry> li = db.tbl_cateogry.ToList();
            ViewBag.cat_list = new SelectList(li, "cat_id", "cat_name");

            string path = uploadimage(img);
            if (path.Equals("-1"))
            {
                ViewBag.error = "image not uploaded";
            }
            else
            {

                tbl_product p = new tbl_product();
                p.pro_name = pr.pro_name;
                p.pro_price = pr.pro_price;
                p.pro_desc = pr.pro_desc;
                p.pro_image = path;
                p.cat_id_fk = pr.u_id_fk;
                p.u_id_fk = Convert.ToInt32(Session["u_id"].ToString());
                db.tbl_product.Add(p);
                db.SaveChanges();
                Response.Redirect("displayadd");

            }

            return View();


        }


        //image uploading,,,,,,

        public string uploadimage(HttpPostedFileBase file)
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

        public ActionResult displayadd(int ? ide , int ? page)
        {

            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.tbl_product.Where(x => x.cat_id_fk == ide).OrderByDescending(x => x.pro_id).ToList();
            IPagedList<tbl_product> pro = list.ToPagedList(pageindex, pagesize);

            return View(pro);
        }


        
    }
}