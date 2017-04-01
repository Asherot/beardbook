using System.Web.Mvc;
using BeardBook.Commands;
using Microsoft.AspNet.Identity;

namespace BeardBook.Controllers
{
    public class FriendsController : BaseController
    {
        #region dependencies

        private readonly ICommandHandler<AddFriendCommand> _addFriendHandler;
        private readonly ICommandHandler<RemoveFriendCommand> _removeFriendHandler;

        public FriendsController(
            ICommandHandler<AddFriendCommand> addFriendHandler,
            ICommandHandler<RemoveFriendCommand> removeFriendHandler)
        {
            _addFriendHandler = addFriendHandler;
            _removeFriendHandler = removeFriendHandler;
        } 
        
        #endregion

        public ActionResult AddFriend(int id)
        {
            _addFriendHandler.Handle(new AddFriendCommand(
                User.Identity.GetUserId<int>(), 
                id));

            return new RedirectResult(Url.Action("Index", "Profile") + "#friends");
        }

        public ActionResult RemoveFriend(int id)
        {
            _removeFriendHandler.Handle(new RemoveFriendCommand(
                User.Identity.GetUserId<int>(),
                id));

            return new RedirectResult(Url.Action("Index", "Profile") + "#friends");
        }
    }
}