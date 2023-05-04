using BikeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using System.Drawing;
using static Google.Protobuf.WireFormat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BikeAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BikesController : Controller
    {

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("GetBikes")]
        [HttpGet]
        public async Task<IEnumerable<BikesApplicationModel.Bike>> GetBikes()
        {
            BikesContext _bikeContext = new BikesContext();
            IEnumerable<BikesApplicationModel.Bike> bikes = await _bikeContext.Bikes.Select(b =>
            new BikesApplicationModel.Bike
            {
                IdBike = b.IdBike,
                Name = b.Name,
                Image = b.Image,
                Type = b.Type,
                Brand = b.Brand,
                Size = b.Size,
                NumberDishes = b.NumberDishes,
                NumberSprockets = b.NumberSprockets
            }
            ).ToListAsync();
            return bikes;
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("GetBike")]
        [HttpGet]
        public async Task<BikesApplicationModel.Bike> GetBike(int id)
        {
            BikesContext _bikeContext = new BikesContext();
            BikesApplicationModel.Bike bike = await _bikeContext.Bikes.Select(b =>
            new BikesApplicationModel.Bike
            {
                IdBike = b.IdBike,
                Name = b.Name,
                Image = b.Image,
                Type = b.Type,
                Brand = b.Brand,
                Size = b.Size,
                NumberDishes = b.NumberDishes,
                NumberSprockets = b.NumberSprockets
            }
            ).FirstOrDefaultAsync(b => b.IdBike == id);
            return bike;
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("CreateBike")]
        [HttpPost]
        public async Task<BikesApplicationModel.GeneralResult> Create(BikesApplicationModel.Bike bike)
        {
            BikesApplicationModel.GeneralResult generalResult = new BikesApplicationModel.GeneralResult()
            {
                Result = false
            };

            try
            {
                BikesContext _BikesContext = new BikesContext();
                Models.Bike newBike = new Models.Bike
                {
                    IdBike = bike.IdBike,
                    Name = bike.Name,
                    Image = bike.Image,
                    Type = bike.Type,
                    Brand = bike.Brand,
                    Size = bike.Size,
                    NumberDishes = bike.NumberDishes,
                    NumberSprockets = bike.NumberSprockets
                };
                _BikesContext.Bikes.Add(newBike);
                await _BikesContext.SaveChangesAsync();
                generalResult.Result = true;
            }
            catch (Exception ex)
            {
                generalResult.Result = false;
                generalResult.ErrorMessage = ex.Message;
            }
            return generalResult;
        }




        [Route("UpdateBike")]
        [HttpPut]
        public async Task<BikesApplicationModel.GeneralResult> Update(BikesApplicationModel.Bike bike)
        {
            BikesApplicationModel.GeneralResult generalResult = new BikesApplicationModel.GeneralResult()
            {
                Result = false
            };

            try
            {
                BikesContext _BikesContext = new BikesContext();
                var bikeToUpdate = await _BikesContext.Bikes.FindAsync(bike.IdBike);
                if (bikeToUpdate != null)
                {
                    bikeToUpdate.IdBike = bike.IdBike;
                    bikeToUpdate.Name = bike.Name;
                    bikeToUpdate.Image = bike.Image;
                    bikeToUpdate.Type = bike.Type;
                    bikeToUpdate.Brand = bike.Brand;
                    bikeToUpdate.Size = bike.Size;
                    bikeToUpdate.NumberDishes = bike.NumberDishes;
                    bikeToUpdate.NumberSprockets = bike.NumberSprockets;

                    await _BikesContext.SaveChangesAsync();
                    generalResult.Result = true;
                }
                else
                {
                    generalResult.ErrorMessage = $"Bicicleta no encontrado.";
                }
            }
            catch (Exception ex)
            {
                generalResult.ErrorMessage = ex.Message;
            }
            return generalResult;
        }


        [Route("DeleteBike")]
        [HttpDelete]
        public async Task<BikesApplicationModel.GeneralResult> Delete(int idBike)
        {
            BikesApplicationModel.GeneralResult generalResult = new BikesApplicationModel.GeneralResult()
            {
                Result = false
            };

            BikesContext _BikesContext = new BikesContext();

            try
            {
                var bike = await _BikesContext.Bikes.FindAsync(idBike);
                if (bike != null)
                {
                    _BikesContext.Bikes.Remove(bike);
                    await _BikesContext.SaveChangesAsync();
                    generalResult.Result = true;
                }
                else
                {
                    generalResult.ErrorMessage = $"Bicicleta no encontrado.";
                }
            }
            catch (Exception ex)
            {
                generalResult.ErrorMessage = ex.Message;
            }
            return generalResult;
        }


        [Route("GuardarImagen")]
        [HttpPost]

        public async Task<string> GuardarImagen([FromForm] SubirImagenAPI fichero)
        {
            var ruta = String.Empty;

            if (fichero.Archivo.Length > 0) {
                var nombreArchivo = Guid.NewGuid().ToString() + ".jpg";
                ruta = $"Imagenes/{nombreArchivo}";
                using (var stream = new FileStream(ruta, FileMode.Create)) {
                    await fichero.Archivo.CopyToAsync(stream);
                }
            }

            return ruta;

        }


    }
}
