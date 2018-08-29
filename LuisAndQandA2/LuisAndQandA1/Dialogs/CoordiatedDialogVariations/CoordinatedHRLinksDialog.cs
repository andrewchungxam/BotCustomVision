using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace LuisAndQandA1.Dialogs
{
    [Serializable]
    public class CoordinatedHRLinksDialog : IDialog<bool> //IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            //OPTION 1 - WAITS FOR A MESSAGE
            //context.Wait(this.MessageReceivedAsync);

            //OPTION 2 - GOES DIRECTLY INTO SHOWING A MESSAGE
            await this.NoMessageNeededAsync(context);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var reply = context.MakeMessage();

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = GetCardsAttachments();

            await context.PostAsync(reply);

            //context.Wait(this.MessageReceivedAsync);

            //WAIT FOR 1 SECOND
            System.Threading.Thread.Sleep(1000);
            context.Done(true);
        }

        public virtual async Task NoMessageNeededAsync(IDialogContext context)
        {
            var reply = context.MakeMessage();

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = GetCardsAttachments();

            await context.PostAsync(reply);

            //context.Wait(this.MessageReceivedAsync);

            //WAIT FOR 1 SECOND
            System.Threading.Thread.Sleep(1000);
            context.Done(true);
        }



        private static IList<Attachment> GetCardsAttachments()
        {
            return new List<Attachment>()
            {
                GetThumbnailCard(
                    "Company HR Policy",
                    "Human Resources Policies",
                    "Contoso's one-stop shop for all HR policies and links.  All guides are tailored to each of the regional Contoso offices. Also, you can find links to the new employee orientation guides here.  Look here first!",
                    new CardImage(url: "https://www.shrm.org/_layouts/15/SHRM.Core/design/images/SHRMLogo.svg"),
                    new CardAction(ActionTypes.OpenUrl, "Learn more", value: "https://www.shrm.org/pages/default.aspx")),
                GetThumbnailCard(
                    "Insurance Election Schedule",
                    "Human resource policies",
                    "The new year is just around the corner - don't forget to choose your insurance provider and policy!  Each link describes each policy in depth.  You can also find your last year's elections here.",
                    new CardImage(url: "http://static.weboffice.uwa.edu.au/visualid/graphics/uwacrest.png"),
                    new CardAction(ActionTypes.OpenUrl, "Learn more", value: "http://www.hr.uwa.edu.au/policies/policies/a-z")),
                 GetThumbnailCard(
                     "Remote Working Tools",
                    "Human resource policies",
                    "15% of Contoso's employees are remote workers - here's a list of the best tools to use when you're on the road.",
                    new CardImage(url: "https://www.shrm.org/_layouts/15/SHRM.Core/design/images/SHRMLogo.svg"),
                    new CardAction(ActionTypes.OpenUrl, "Learn more", value: "https://www.shrm.org/pages/default.aspx")),
            };
        }

        private static Attachment GetHeroCard(string title, string subtitle, string text, CardImage cardImage, CardAction cardAction)
        {
            var heroCard = new HeroCard
            {
                Title = title,
                Subtitle = subtitle,
                Text = text,
                Images = new List<CardImage>() { cardImage },
                Buttons = new List<CardAction>() { cardAction },
            };

            return heroCard.ToAttachment();
        }

        private static Attachment GetThumbnailCard(string title, string subtitle, string text, CardImage cardImage, CardAction cardAction)
        {
            var heroCard = new ThumbnailCard
            {
                Title = title,
                Subtitle = subtitle,
                Text = text,
                Images = new List<CardImage>() { cardImage },
                Buttons = new List<CardAction>() { cardAction },
            };

            return heroCard.ToAttachment();
        }
    }
}
