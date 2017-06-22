using System;
using System.Web.Mvc;
using BeardBook.DAL;
using BeardBook.Entities;
using Microsoft.AspNet.Identity;

namespace BeardBook.Controllers
{
    [Authorize]
    public class FilesController : BaseController
    {
        #region dependencies

        private readonly IQueryHandler<GetFileQuery, File> _fileHandler;

        public FilesController(IQueryHandler<GetFileQuery, File> fileHandler)
        {
            _fileHandler = fileHandler;
        }

        #endregion

        public ActionResult GetFile(int id)
        {
            try
            {
                var file = _fileHandler.Handle(
                    new GetFileQuery(id, User.Identity.GetUserId<int>()));

                return File(file.Data, file.ContentType);
            }
            catch (InvalidOperationException)
            {
                return View("PermissionDenied");
            }
        }
    }
}