using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HLD.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderNotesController : ControllerBase
    {
        OrderNotesDataAccess _NotesDataAccess;
        public OrderNotesController(IConnectionString connectionString)
        {
            _NotesDataAccess = new OrderNotesDataAccess(connectionString);
        }
        [HttpGet]
        [Authorize]
       [Route("{id}")]
        public IActionResult Get(int id)
        {
            List<GetNotesOrderViewModel> _ViewModels = null;

            _ViewModels = _NotesDataAccess.GetOrderNotes(id);
            if (_ViewModels == null)
            {
                return Ok(new List<GetNotesOrderViewModel>());
            }
            else
            {
                return Ok(_ViewModels);
            }
        }

        [HttpPost]
       
        public IActionResult SaveNotes([FromBody]List<CreateOrderNotesViewModel> notesViewModels)
        {
            bool status = false;

            status = _NotesDataAccess.SaveOrderNotes(notesViewModels);
         
                return Ok(status);
            
        }
        [HttpGet]
        [Authorize]
        [Route("having/{id}")]
        public IActionResult Setashaving(int id)
        {


            bool status = _NotesDataAccess.UpdateOrderAsHavingNotes(id);
       
                return Ok(status);
            
        }

    }
}