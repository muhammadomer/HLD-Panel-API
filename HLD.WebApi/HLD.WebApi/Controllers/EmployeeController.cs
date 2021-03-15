using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HLD.WebApi.Controllers
{
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        EmployeeDataAccess _employeeDataAccess;
        public EmployeeController(IConnectionString connectionString)
        {
            _employeeDataAccess = new EmployeeDataAccess(connectionString);
        }

        [HttpPost]
        //[Authorize]
        [Route("api/Employee/save")]
        public IActionResult Post([FromBody] EmployeeViewModel employeeViewModel)
        {
            bool status = false;
            if (_employeeDataAccess.SaveEmployee(employeeViewModel))
            {
                status = true;
                return Ok(status);
            }
            else
            {
                return Ok(status);
            }
        }

        [HttpGet]
        //[Authorize]
        [Route("api/Employee/GetEmployees")]
        public IActionResult Get()
        {
            List<EmployeeViewModel> _ViewModels = null;

            _ViewModels = _employeeDataAccess.GetAllEmployees();
            if (_ViewModels == null)
            {
                return Ok(new List<EmployeeViewModel>());
            }
            else
            {
                return Ok(_ViewModels);
            }
        }

        [HttpGet]
        [Route("api/Employee/GetEmployeeById/{id}")]
        public IActionResult GetEmployeeById(int id)
        {
            EmployeeViewModel ViewModel = new EmployeeViewModel();
            try
            {
                ViewModel = _employeeDataAccess.GetEmployeeById(id);
                return Ok(ViewModel);
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpGet]
        [Route("api/Employee/GetEmployeeByEmployeeId/{id}")]
        public IActionResult GetEmployeeByEmployeeId(int id)
        {
            EmployeeViewModel ViewModel = new EmployeeViewModel();
            try
            {
                ViewModel = _employeeDataAccess.GetEmployeeByEmployeeId(id);
                return Ok(ViewModel);
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpPut]
        //[Authorize]
        [Route("api/Employee/UpdateEmployee")]
        public IActionResult Put(EmployeeViewModel ViewModel)
        {
            bool status = false;
            if (_employeeDataAccess.UpdateEmployeeById(ViewModel))
            {
                status = true;
                return Ok(status);
            }
            else
            {
                return Ok(status);
            }
        }

        [HttpPut]
        //[Authorize]
        [Route("api/Employee/UpdateEmpActiveStatusById")]
        public IActionResult UpdateEmpActiveStatusById(EmployeeViewModel ViewModel)
        {
            bool status = false;
            if (_employeeDataAccess.UpdateEmpActiveStatusById(ViewModel))
            {
                status = true;
                return Ok(status);
            }
            else
            {
                return Ok(status);
            }
        }
    }
}
