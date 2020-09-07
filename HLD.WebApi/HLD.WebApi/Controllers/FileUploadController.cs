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
    public class FileUploadController : Controller
    {

        FileUploadDataAccess  _dataAccess;
        public FileUploadController(IConnectionString connectionString)
        {
            _dataAccess = new FileUploadDataAccess(connectionString);
        }

        [HttpPost]
        [Authorize]
        [Route("api/FileUpload/SaveFileUpload")]
        public IActionResult SaveFileUpload([FromBody] FileUploadViewModel viewModel)
        {
            bool status = false;
            status = _dataAccess.SaveFileUpload(viewModel);
            return Ok(status);
        }


        [HttpPost]
        [Authorize]
        [Route("api/FileUpload/SaveFileUploadStatusLog")]
        public IActionResult SaveFileUploadStatusLog([FromBody] FileUploadStatusLogViewModel viewModel)
        {
            bool status = false;
            status = _dataAccess.SaveFileUploadStatusLog(viewModel);
            return Ok(status);
        }

        [HttpPost]
        [Authorize]
        [Route("api/FileUpload/GetFileUploadByFileNameAndFileType/{FileName}/{FileType}")]
        public IActionResult GetFileUploadByNameAndFileType([FromBody] FileUploadViewModel viewModel)
        {
            bool status = false;
            status = _dataAccess.SaveFileUpload(viewModel);
            return Ok(status);
        }

        [HttpPost]
        [Authorize]
        [Route("api/FileUpload/SaveSkuAsinMapping")]
        public IActionResult SaveAsinSkuMappingWithFile([FromBody] List<AsinSkuMappingViewModel> asinSkuMappingViewModel)
        {
            bool status = false;
             status = _dataAccess.AsinSkuMappingDataAccess(asinSkuMappingViewModel);
            return Ok(status);
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}