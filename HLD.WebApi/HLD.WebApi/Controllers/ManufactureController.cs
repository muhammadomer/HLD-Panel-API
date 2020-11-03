using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HLD.WebApi.Controllers
{
   
    public class ManufactureController : Controller
    {
        ManufactureDataAccess DataAccess;
        IConnectionString _connectionString = null;
        public ManufactureController(IConnectionString connectionString)
        {
            DataAccess = new ManufactureDataAccess(connectionString);
            _connectionString = connectionString;
        }

        public IActionResult Index()
        {
            return View();
        }


        //[HttpGet]
        //[Authorize]
        //[Route("api/Manufacture/GetManufacture")]
        //public List<GetManufactureViewModel> GetManufacture()
        //{
        //    try
        //    {
        //        List<GetManufactureViewModel> _ViewModels = new List<GetManufactureViewModel>();
        //        _ViewModels = DataAccess.GetManufacture();
        //            return _ViewModels;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}


        [HttpGet]
        [Authorize]
        [Route("api/Manufacture/GetManufacture")]
        public IActionResult GetAllColorByName()
        {
            List<GetManufactureViewModel> _ViewModels = null;

            _ViewModels = DataAccess.GetManufacturelist();

            if (_ViewModels == null)
            {
                return Ok(null);
            }
            else
            {
                return Ok(_ViewModels);
            }
        }


        [HttpGet]
        [Authorize]
        [Route("api/Manufacture/GetManufactureModel")]
        public List<GetManufactureModelViewModel> GetManufactureModel( int ManufactureId)
        {
            try
            {
                List<GetManufactureModelViewModel> _ViewModels = new List<GetManufactureModelViewModel>();
                _ViewModels = DataAccess.GetManufactureModel(ManufactureId);
                return _ViewModels;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet]
        [Authorize]
        [Route("api/Manufacture/GetDeviceModelModel")]
        public List<GetDeviceModelViewMdel> GetDeviceModelModel(int ManufactureModel)
        {
            try
            {
                List<GetDeviceModelViewMdel> _ViewModels = new List<GetDeviceModelViewMdel>();
                _ViewModels = DataAccess.GetDeviceModelModel(ManufactureModel);
                return _ViewModels;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/Manufacture/AddManufacturer")]
        public IActionResult AddManufacturer([FromBody] AddManufactureViewModel model)
        {
            bool status = false;
            status = DataAccess.AddManufacture(model);
            return Ok(status);
        }

        [HttpPost]
        [Authorize]
        [Route("api/Manufacture/AddManufacturerModel")]
        public IActionResult AddManufacturerModel([FromBody] AddManufacturerModelViewModel model)
        {
            bool status = false;
            status = DataAccess.AddManufacturerModel(model);
            return Ok(status);
        }

        [HttpPost]
        [Authorize]
        [Route("api/Manufacture/AddDeviceModel")]
        public IActionResult AddDeviceModel([FromBody] AddDeviceModelView model)
        {
            bool status = false;
            status = DataAccess.AddDeviceModel(model);
            return Ok(status);
        }
        //[HttpGet]
        //[Authorize]
        //[Route("api/Manufacture/CheckManufactureExistOrNot")]
        //public List<GetManufactureModelViewModel> CheckManufactureExistOrNot(string manufacture)
        //{
        //    try
        //    {
        //        List<GetManufactureModelViewModel> _ViewModels = new List<GetManufactureModelViewModel>();
        //        _ViewModels = DataAccess.GetManufactureModel(manufacture);
        //        return _ViewModels;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

        [HttpGet]
        [Authorize]
        [Route("api/Manufacture/CheckManufactureExists/{name}")]
        public IActionResult CheckManufactureExists(string name)
        {
            bool status = false;
            if (DataAccess.CheckManufactureExists(name))
            {
                status = true;
                return Ok(new { Status = status, Message = "Manufacture exists ,please select another" });
            }
            else
            {
                return Ok(new { Status = status, Message = "Manufacture Not Exists" });
            }
        }

    }
}