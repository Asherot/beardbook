using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using BeardBook.Commands;
using BeardBook.DAL;
using BeardBook.Entities;
using BeardBook.Models.ConversationsViewModels;
using Microsoft.AspNet.Identity;

namespace BeardBook.Controllers
{
    [Authorize]
    public class ConversationsController : BaseController
    {
        #region dependencies

        private readonly IQueryHandler<GetConversationsUsersQuery, IEnumerable<User>> _usersHandler;
        private readonly IQueryHandler<GetConversationQuery, Conversation> _conversationHandler;
        private readonly ICommandHandler<SendMessageCommand> _messageHandler;

        public ConversationsController(
            IQueryHandler<GetConversationsUsersQuery, IEnumerable<User>> usersHandler,
            IQueryHandler<GetConversationQuery, Conversation> conversationHandler,
            ICommandHandler<SendMessageCommand> messageHandler)
        {
            _usersHandler = usersHandler;
            _conversationHandler = conversationHandler;
            _messageHandler = messageHandler;
        } 

        #endregion

        public ActionResult Index(int? id)
        {
            var userId = User.Identity.GetUserId<int>();
            var conversation = _conversationHandler.Handle(
                new GetConversationQuery(userId, id ?? 0));

            if (conversation == null) return View("NoConversations");

            var model = Mapper.Map<ConversationViewModel>(conversation);
            model.UserId = userId;

            return Request.IsAjaxRequest() 
                ? (ActionResult) PartialView("_MessageList", model)
                : View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendMessage([Bind(Include = "ConversationId,MessageText")] ConversationViewModel model)
        {
            if (!ModelState.IsValid) return View("Index", model);
            if (string.IsNullOrWhiteSpace(model.MessageText)) return RedirectToAction("Index");

            _messageHandler.Handle(new SendMessageCommand(
                User.Identity.GetUserId<int>(),
                model.ConversationId,
                model.MessageText,
                Url.Action("Index", "Conversations", new {}, Request.Url?.Scheme) + "/Index"));
            
            return RedirectToAction("Index");
        }

        [ChildActionOnly]
        public ActionResult FriendsBar()
        {
            var users = _usersHandler.Handle(
                new GetConversationsUsersQuery(User.Identity.GetUserId<int>()));

            var model = Mapper.Map<List<ConversationUserViewModel>>(users);
            return View("_FriendsBar", model);
        }
    }
}