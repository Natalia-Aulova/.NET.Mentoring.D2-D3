using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using ProfileSample.DAL;
using ProfileSample.Models;

namespace ProfileSample.Controllers
{
    public class HomeController : Controller
    {
        ProfileSampleEntities _context;

        public HomeController()
        {
            _context = new ProfileSampleEntities();
        }

        public ActionResult Index()
        {
            var sources = _context.ImgSources.Take(20).Select(x => x.Id);
            
            var model = new List<ImageModel>();

            foreach (var id in sources)
            {
                var item = _context.ImgSources.Find(id);

                var obj = new ImageModel()
                {
                    Name = item.Name,
                    Data = item.Data
                };

                model.Add(obj);
            } 

            return View("Index", model);
        }

        public ActionResult Index_v1()
        {
            var model = new List<ImageModel>();

            foreach (var item in _context.ImgSources.Take(20))
            {
                var obj = new ImageModel()
                {
                    Name = item.Name,
                    Data = item.Data
                };

                model.Add(obj);
            }

            return View("Index", model);
        }

        public ActionResult Index_v2()
        {
            var model = new List<ImageModel>();

            foreach (var item in _context.ImgSources.Take(20))
            {
                var obj = new ImageModel()
                {
                    Id = item.Id,
                    Name = item.Name
                };

                model.Add(obj);
            }

            return View("Index_v2", model);
        }

        public ActionResult Image(int id)
        {
            var item = _context.ImgSources.Find(id);
            
            return File(item.Data, "image");
        }

        public ActionResult Convert()
        {
            var files = Directory.GetFiles(Server.MapPath("~/Content/Img"), "*.jpg");

            using (var context = new ProfileSampleEntities())
            {
                foreach (var file in files)
                {
                    using (var stream = new FileStream(file, FileMode.Open))
                    {
                        byte[] buff = new byte[stream.Length];

                        stream.Read(buff, 0, (int)stream.Length);

                        var entity = new ImgSource()
                        {
                            Name = Path.GetFileName(file),
                            Data = buff,
                        };

                        context.ImgSources.Add(entity);
                        context.SaveChanges();
                    }
                }
            }

            return RedirectToAction("Index");
        }
    }
}