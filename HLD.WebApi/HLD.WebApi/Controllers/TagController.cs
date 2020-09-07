using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using Hld.WebApi.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HLD.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        TagDataAccess _dataAccess;


        public TagController(IConnectionString connectionString)
        {
            _dataAccess = new TagDataAccess(connectionString);
        }
        [HttpPost]
        [Authorize]
        [Route("Create")]
        public IActionResult SaveTag([FromBody] TagViewModel tagViewModel  )
        {
            bool status = false;
            try
            {
               
                status = _dataAccess.SaveTag(tagViewModel);
                return Ok(status);
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetTag()
        {
            List<TagViewModel> tagViewModel = new List<TagViewModel>();
            try
            {
                tagViewModel = _dataAccess.GetTag();
                return Ok(tagViewModel);
            }
            catch (Exception)
            {

                throw;
            }

        }

        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetTagById(int id)
        {
            TagViewModel tagViewModel = new TagViewModel();
            try
            {
                tagViewModel = _dataAccess.GetTagById(id);
                return Ok(tagViewModel);
            }
            catch (Exception)
            {

                throw;
            }

        }
        
        [HttpPost]
        [Authorize]
        [Route("Assigntag")]
        public IActionResult AssignTagtoSKU([FromBody]  List<AssignTagViewModel> viewModel)
        {
            bool status = false;
            try
            {

                status = _dataAccess.AssignTag(viewModel);
                return Ok(status);
            }
            catch (Exception)
            {

                throw;
            }

        }

        [Authorize]
        [HttpGet]
        [Route("GetBy/{sku}")]
        public List<SkuTagOrderViewModel> GetTagBySku(string sku)
        {
            List<SkuTagOrderViewModel> tagViewModel = new List<SkuTagOrderViewModel>();
            try
            {
                tagViewModel = _dataAccess.GetTagforSku(sku);
                return tagViewModel;
            }
            catch (Exception)
            {

                throw;
            }

        }


        [HttpPost]
        [Authorize]
        [Route("RemoveTag")]
        public IActionResult RemoveTagtoSKU([FromBody]  AssignTagViewModel viewModel)
        {
            bool status = false;
            try
            {

                status = _dataAccess.RemoveTag(viewModel);
                return Ok(status);
            }
            catch (Exception)
            {

                throw;
            }

        }


    }
}