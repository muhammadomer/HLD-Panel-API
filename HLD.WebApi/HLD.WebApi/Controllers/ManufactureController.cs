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

        [HttpGet]
        [Authorize]
        [Route("api/Manufacture/GetManufacture")]
        public List<GetManufactureViewModel> GetManufacture()
        {
            try
            {
                List<GetManufactureViewModel> _ViewModels = new List<GetManufactureViewModel>();
                _ViewModels = DataAccess.GetManufacture();
                    return _ViewModels;
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        [HttpGet]
        [Authorize]
        [Route("api/Manufacture/GetManufactureModel")]
        public List<GetManufactureModelViewModel> GetManufactureModel( int Id)
        {
            try
            {
                List<GetManufactureModelViewModel> _ViewModels = new List<GetManufactureModelViewModel>();
                _ViewModels = DataAccess.GetManufactureModel(Id);
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
        public List<GetDeviceModelViewMdel> GetDeviceModelModel(int Id)
        {
            try
            {
                List<GetDeviceModelViewMdel> _ViewModels = new List<GetDeviceModelViewMdel>();
                _ViewModels = DataAccess.GetDeviceModelModel(Id);
                return _ViewModels;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}