using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace LuisAndQandA1
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
//SINGLE DIALOG ONLY
                //await Conversation.SendAsync(activity, () => new Dialogs.RootEchoDialog());
                //await Conversation.SendAsync(activity, () => new Dialogs.QandADialog());
                //await Conversation.SendAsync(activity, () => new Dialogs.QandADialog2());
                
                //await Conversation.SendAsync(activity, () => new Dialogs.BasicLuisDialog());

                //await Conversation.SendAsync(activity, () => new Dialogs.SimpleImageCaptionDialog());
                //await Conversation.SendAsync(activity, () => new Dialogs.SimpleCustomImageCaptionDialog());
                //await Conversation.SendAsync(activity, () => new Dialogs.SimpleOCRDialog());
                //await Conversation.SendAsync(activity, () => new Dialogs.SimpleZXingBarcodeDialog());

                //await Conversation.SendAsync(activity, () => new Dialogs.SimpleRootDialog());
                //await Conversation.SendAsync(activity, () => new Dialogs.SimplePasswordResetDialog());
                //await Conversation.SendAsync(activity, () => new Dialogs.CarouselCardsDialog());



                //COORDINATED
                //await Conversation.SendAsync(activity, () => new Dialogs.CoordinatedConversationDialog());
                //await Conversation.SendAsync(activity, () => new Dialogs.CoordinatedLuisDialog());

                //CUSTOMER SERVICE COORDINATED

                await Conversation.SendAsync(activity, () => new Dialogs.CoordinatedCustomerServiceDialog());



            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}