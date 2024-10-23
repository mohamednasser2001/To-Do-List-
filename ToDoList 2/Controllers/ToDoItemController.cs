using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList_2.Data;
using ToDoList_2.Models;

namespace ToDoList_2.Controllers
{
    public class ToDoItemController : Controller
    {
        ApplicationDbContext context=new ApplicationDbContext();

        public IActionResult Index(string YourName)
        {
            CookieOptions options = new CookieOptions();
            options.Secure = true;
            options.Expires = DateTimeOffset.Now.AddDays(1);
            Response.Cookies.Append("Name", YourName, options);
            var item=context.items.ToList();
            return View(item);
        }

        public IActionResult NotFound()
        {
            return View();
        }
   


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Items items, IFormFile FilePdf)
        {
            if (FilePdf!=null &&  FilePdf.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(FilePdf.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\PDF", fileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    FilePdf.CopyTo(stream);
                }

                items.FilePdf = fileName;
            }
            context.items.Add(items);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }




        public IActionResult Edit(int itemsId)
        {
            var items = context.items.Find(itemsId);
            return RedirectToAction("NotFound");
        }

        [HttpPost]
        public IActionResult Edit(Items items, IFormFile FilePdf)
        {
            var olditem = context.items.AsNoTracking().FirstOrDefault(e => e.Id == items.Id);
            if (FilePdf != null && FilePdf.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(FilePdf.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\PDF", fileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    FilePdf.CopyTo(stream);
                }

                items.FilePdf = fileName;
            }
            else
            {
                items.FilePdf = olditem.FilePdf;
            }
             context.items.Update(items);
            return RedirectToAction("NotFound");
        }

        public IActionResult Delete(Items items)
        {
            var olditem = context.items.AsNoTracking().FirstOrDefault(e => e.Id == items.Id);
            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\PDF", olditem.FilePdf);

            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }

            //items item = new items() { };
            context.items.Remove(items);
            context.SaveChanges();

            TempData["success"] = "Delete product successfully";


            return RedirectToAction(nameof(Index));
        }
    }
}
