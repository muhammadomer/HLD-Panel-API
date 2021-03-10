using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HLD.WebApi.Controllers
{
    [ApiController]
    public class EmployeeRole : ControllerBase
    {
        EmployeeRoleDataAccess _employeeRoleDataAccess;

        public EmployeeRole(IConnectionString connectionString)
        {
            _employeeRoleDataAccess = new EmployeeRoleDataAccess(connectionString);
        }
        [HttpPost]
        //[Authorize]
        [Route("api/EmployeeRole/save")]
        public IActionResult Post([FromBody] EmployeeRoleViewModel employeeRoleViewModel)
        {
            bool status = false;
            if (_employeeRoleDataAccess.SaveEmployeeRole(employeeRoleViewModel))
            {
                status = true;
                return Ok(status);
            }
            else
            {
                return Ok(new { Status = status });
            }
        }

        [HttpGet]
        //[Authorize]
        [Route("api/EmployeeRole/GetEmployeeRoles")]
        public IActionResult GetAllEmployeeRoles()
        {
            List<EmployeeRoleViewModel> _ViewModels = null;

            _ViewModels = _employeeRoleDataAccess.GetAllEmployeeRoles();
            if (_ViewModels == null)
            {
                return Ok(new List<EmployeeRoleViewModel>());
            }
            else
            {
                return Ok(_ViewModels);
            }
        }
    }
}
