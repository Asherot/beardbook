using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using AutoMapper;
using BeardBook.Commands;
using BeardBook.DAL;
using BeardBook.Entities;
using BeardBook.Identity;
using BeardBook.Models;
using BeardBook.Models.HomeViewModels;

namespace BeardBook.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        #region dependencies

        private readonly AppUserManager _userManager;
        private readonly IQueryHandler<GetWallPostsQuery, IEnumerable<Post>> _getUserPostsHandler;
        private readonly ICommandHandler<AddPostCommand> _addPostHandler;

        public HomeController(AppUserManager userManager,
            IQueryHandler<GetWallPostsQuery, IEnumerable<Post>> getUserPostsHandler,
            ICommandHandler<AddPostCommand> addPostHandler)
        {
            _userManager = userManager;
            _getUserPostsHandler = getUserPostsHandler;
            _addPostHandler = addPostHandler;
        }

        #endregion

        [AllowAnonymous]
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated) return View("Showcase");

            var posts = _getUserPostsHandler.Handle(
                new GetWallPostsQuery(User.Identity.GetUserId<int>()));

            var model = new HomeViewModel
            {
                PostViewModels = Mapper.Map<List<PostViewModel>>(posts)
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPost([Bind(Exclude = "PostViewModels")] HomeViewModel model)
        {
            if (!ModelState.IsValid) return View("Index", model);
            if (string.IsNullOrWhiteSpace(model.PostText)) return RedirectToAction("Index");
            
            _addPostHandler.Handle(new AddPostCommand(
                User.Identity.GetUserId<int>(),
                model.PostText,
                model.MediaFiles));

            return RedirectToAction("Index");
        }

        [ChildActionOnly]
        public ActionResult OptionsBar()
        {
            var user = _userManager.FindById(User.Identity.GetUserId<int>());
            var model = Mapper.Map<OptionsBarViewModel>(user);
            return View("_OptionsBar", model);
        }
    }
}