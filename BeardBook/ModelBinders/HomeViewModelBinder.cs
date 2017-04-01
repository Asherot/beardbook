using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BeardBook.Models.HomeViewModels;

namespace BeardBook.ModelBinders
{
    public class HomeViewModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var allowedContentTypes = new[] {"image/png", "image/jpeg", "video/mp4"};
            const int allowedMaxSize = 10 * 1024 * 1024;

            var homeViewModel = new HomeViewModel
            {
                MediaFiles = new List<HttpPostedFileBase>(),
                PostText = controllerContext.HttpContext.Request.Unvalidated.Form.Get("PostText")
            };

            var files = controllerContext.HttpContext.Request.Files;
            for (var i = 0; i < files.Count; i++)
            {
                var file = files[i];
                if (file == null || file.ContentLength <= 0) continue;

                if (!allowedContentTypes.Contains(file.ContentType))
                {
                    bindingContext.ModelState.AddModelError("", "Allowed file types: png, jpg, mp4");
                }
                else if (file.ContentLength > allowedMaxSize)
                {
                    bindingContext.ModelState.AddModelError("", "Maximum allowed file size is 10MB");
                }
                else
                {
                    homeViewModel.MediaFiles.Add(file);
                }
            }
            return homeViewModel;
        }
    }
}