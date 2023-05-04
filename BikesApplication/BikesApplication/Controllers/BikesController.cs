using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BikesApplication.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

namespace BikesApplication.Controllers
{
    public class BikesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> List()
        {
            BikesApplicationModel.Token token = await Functions.APIServices.LoginAPILogin(
            new BikesApplicationModel.Token
            {
              token = "adfadsfadsfasd"
            });

            if (string.IsNullOrEmpty(token.token))
            {
                return NotFound();
            }

            IEnumerable<BikesApplicationModel.Bike> bikes = await Functions.APIServices.GetBikes(token.token);
            return View(bikes);
        }


        [Authorize]
        public IActionResult Create()
        {
            return View();
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([Bind("IdBike,Name,Image,Type,Brand,Size,NumberDishes,NumberSprockets")] BikesApplicationModel.Bike bike, IFormFile Image)
        {
            if (ModelState.IsValid)
            {

                if (Image != null && Image.Length > 0)
                {
                    bike.Image = SaveImage(Image);
                }

                BikesApplicationModel.Token token = await Functions.APIServices.LoginAPILogin(
                new BikesApplicationModel.Token
                {
                    token = "adfadsfadsfasd"
                });

                if (string.IsNullOrEmpty(token.token))
                {
                    return NotFound();
                }
                await Functions.APIServices.BikeSet(bike, token.token);
            }

            return RedirectToAction(nameof(List));
        }


        [Authorize]
        public IActionResult Delete(int id)
        {
            BikesContext _bikesContext = new BikesContext();

            Models.Bike bike = _bikesContext.Bikes.Find(id);

            if (bike == null)
            {
                return NotFound();
            }

            try
            {
                _bikesContext.Bikes.Remove(bike);
                _bikesContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "No se pudo eliminar la bicicleta. Inténtelo de nuevo.");
            }

            return RedirectToAction("List");
        }


        [Authorize]
        public IActionResult Edit(int id)
        {
            BikesContext _bikesContext = new BikesContext();
            Models.Bike bike = _bikesContext.Bikes.Find(id);
            return View(bike);
        }


        [Authorize]
        [HttpPost]
        public IActionResult Edit(int idBike, string name, string image, string type, string brand, decimal size, int numberDishes, int numberSprockets)
        {
            BikesContext _bikesContext = new BikesContext();
            Models.Bike bike = _bikesContext.Bikes.Find(idBike);
            string message = "", errorMessage = "";

            if (bike == null)
            {
                return NotFound();
            }

            bike.IdBike = idBike;
            bike.Name = name;
            bike.Image = image;
            bike.Type = type;
            bike.Brand = brand;
            bike.Size = size;
            bike.Type = type;
            bike.NumberDishes = numberDishes;
            bike.NumberSprockets = numberSprockets;

            try
            {
                _bikesContext.SaveChanges();
                message = "Los cambios se han guardado correctamente.";
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "No se pudieron guardar los cambios. Inténtelo de nuevo.");
                errorMessage = "No se ha podido editar el artículo.";
                return View(bike);
            }
            return RedirectToAction(nameof(List));
        }


        private string SaveImage(IFormFile file)
        {
            var fileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Imagenes", fileName);
            if (System.IO.File.Exists(filePath))
            {
                int i = 1;
                string fileNameOnly = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                while (System.IO.File.Exists(filePath))
                {
                    fileName = string.Format("{0}({1})", fileNameOnly, i++);
                    filePath = Path.Combine(Directory.GetCurrentDirectory(), "Imagenes", fileName + extension);
                }
            }
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyToAsync(stream).Wait();
            }
            return fileName;
        }

    }
}