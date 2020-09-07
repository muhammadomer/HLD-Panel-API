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
    public class HelpController : ControllerBase
    {
        HelpDataAccess _dataAccess;


        public HelpController(IConnectionString connectionString)
        {
            _dataAccess = new HelpDataAccess(connectionString);
        }
        [Authorize]
        [HttpPost]
        [Route("Create")]
        public IActionResult SaveEdirData([FromBody] PostDataViewModel ViewModel)
        {
            bool status = false;
            try
            {

                status = _dataAccess.SaveEdirData(ViewModel);
                return Ok(status);
            }
            catch (Exception ex)
            {
                
                throw;
            }

        }
        [Authorize]
        [HttpGet]
        public IActionResult GetEditorDataList()
        {
            List<PostDataViewModel> ListViewModel = new List<PostDataViewModel>();
            try
            {
                ListViewModel = _dataAccess.GetEditorData();
                return Ok(ListViewModel);
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpPost]
        [Authorize]
        [Route("Update")]
        public IActionResult Put(PostDataViewModel ViewModel)
        {
            bool status = false;
            if (_dataAccess.UpdateData(ViewModel))
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
        [Authorize]
        [Route("Edit")]
        public PostDataViewModel Put(int id)
        {
            
            PostDataViewModel viewModel = null;
            viewModel = _dataAccess.UpdateEditorData(id);

            return viewModel;
        }
        [HttpGet]
        [Authorize]
        [Route("Delete")]
        public IActionResult DeleteBrand(int id)
        {
            bool status = false;
            status = _dataAccess.DeleteData(id);
            if (status == false)
            {
                return Ok(new { Status = false, Message = "Some error occured" });
            }
            else
            {
                return Ok(new { Status = true, Message = "Delete Successfully" });
            }

        }

        [HttpGet]
        [Route("GetCatagory")]
        public IActionResult GetCatagory()
        {
            List<PostData> Data = _dataAccess.GetCatagory();
            return Ok(Data);
        }

        [Authorize]
        [HttpGet]
        [Route("GetDataByCatagory")]
        public List<PostDataViewModel> GetDataByCatagory(int catagoryid)
        {
            List<PostDataViewModel> ListViewModel = new List<PostDataViewModel>();
            try
            {
                ListViewModel = _dataAccess.GetDataByCatagory(catagoryid);
                return ListViewModel;
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Authorize]
        [HttpGet]
        [Route("GetDataByCatagoryByTitle")]
        public List<PostDataViewModel> GetDataByCatagoryByTitle(int catagoryidByTitle)
        {
            List<PostDataViewModel> ListViewModel = new List<PostDataViewModel>();
            try
            {
                ListViewModel = _dataAccess.GetDataByCatagoryByTitle(catagoryidByTitle);
                return ListViewModel;
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Authorize]
        [HttpGet]
        [Route("GetDataByCatagoryByTitleSearch")]
        public List<PostDataViewModel> GetDataByCatagoryByTitleSearch(string title)
        {
            List<PostDataViewModel> ListViewModel = new List<PostDataViewModel>();
            try
            {
                ListViewModel = _dataAccess.GetDataByCatagoryByTitleSearch(title);
                return ListViewModel;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}