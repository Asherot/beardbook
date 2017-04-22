using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using BeardBook.DAL;
using BeardBook.Entities;
using BeardBook.Identity;
using BeardBook.Models.ProfileViewModels;
using BeardBook.Commands;
using BeardBook.Models;
using AutoMapper;

namespace BeardBook.Controllers
{
    [Authorize]
    public class ProfileController : BaseController
    {
        #region dependencies

        private readonly AppUserManager _userManager;
        private readonly ICommandHandler<UpdateUserInfoCommand> _updateUserHandler;
        private readonly ICommandHandler<RemoveAvatarCommand> _avatarHandler;
        private readonly IQueryHandler<GetUserFilesQuery, IEnumerable<File>> _userFilesHandler;
        private readonly IQueryHandler<FindUsersQuery, IEnumerable<UserResult>> _usersHandler;

        public ProfileController(AppUserManager userManager,
            ICommandHandler<UpdateUserInfoCommand> updateUserHandler,
            ICommandHandler<RemoveAvatarCommand> avatarHandler,
            IQueryHandler<GetUserFilesQuery, IEnumerable<File>> userFilesHandler,
            IQueryHandler<FindUsersQuery, IEnumerable<UserResult>> usersHandler)
        {
            _userManager = userManager;
            _updateUserHandler = updateUserHandler;
            _avatarHandler = avatarHandler;
            _userFilesHandler = userFilesHandler;
            _usersHandler = usersHandler;
        }

        #endregion

        [HttpGet]
        public ActionResult Index(string searchTerm, bool? onlyFriends)
        {
            var userId = User.Identity.GetUserId<int>();
            var user = _userManager.FindById(userId);
            var userResults = _usersHandler.Handle(new FindUsersQuery(userId, searchTerm, onlyFriends ?? false));

            var model = Mapper.Map<ProfileViewModel>(user);
            model.Users = Mapper.Map<List<UserViewModel>>(userResults);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Friends", model.Users);
            }
            return View(model);
        }

        public ActionResult Edit()
        {
            var user = _userManager.FindById(User.Identity.GetUserId<int>());
            var model = Mapper.Map<EditViewModel>(user);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            if (string.IsNullOrWhiteSpace(model.Nickname)) model.Nickname = null;
            
            _updateUserHandler.Handle(new UpdateUserInfoCommand(
                User.Identity.GetUserId<int>(),
                model.FirstName,
                model.LastName,
                model.Nickname,
                model.UploadedFile));

            return RedirectToAction("Index");
        }

        public ActionResult RemoveAvatar()
        {
            _avatarHandler.Handle(
                new RemoveAvatarCommand(User.Identity.GetUserId<int>()));
            return RedirectToAction("Index");
        }

        [ChildActionOnly]
        public ActionResult UploadedPhotosTab()
        {
            var photos = _userFilesHandler.Handle(new GetUserFilesQuery(
                User.Identity.GetUserId<int>(),
                FileType.Photo));

            var model = Mapper.Map<List<UploadedMediaViewModel>>(photos);
            return View("_UploadedMediaTab", model);
        }

        [ChildActionOnly]
        public ActionResult UploadedVideosTab()
        {
            var videos = _userFilesHandler.Handle(new GetUserFilesQuery(
                User.Identity.GetUserId<int>(),
                FileType.Video));

            var model = Mapper.Map<List<UploadedMediaViewModel>>(videos);
            return View("_UploadedMediaTab", model);
        }
    }
}