using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace LuisAndQandA1.Dialogs
{
    [Serializable]
    public class CarouselCardsDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var reply = context.MakeMessage();

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = GetCardsAttachments();

            await context.PostAsync(reply);

            context.Wait(this.MessageReceivedAsync);
        }

        private static IList<Attachment> GetCardsAttachments()
        {
            return new List<Attachment>()
            {
                //GetThumbnailCard(
                //    "Azure Storage",
                //    "Offload the heavy lifting of data center management",
                //    "Store and help protect your data. Get durable, highly available data storage across the globe and pay only for what you use.",
                //    new CardImage(url: "http://hr.columbia.edu/sites/all/themes/cuhr_theme/logo.png"),
                //    new CardAction(ActionTypes.OpenUrl, "Learn more", value: "http://hr.columbia.edu/policies/search")),
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












//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Microsoft.Bot.Builder.Dialogs;
//using Microsoft.Bot.Connector;

//namespace LuisAndQandA1.Dialogs
//{
//    [Serializable]
//    public class CarouselCardsDialog : IDialog<object>
//    {
//        public async Task StartAsync(IDialogContext context)
//        {
//            context.Wait(this.MessageReceivedAsync);
//        }

//        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
//        {
//            var reply = context.MakeMessage();

//            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
//            reply.Attachments = GetCardsAttachments();

//            await context.PostAsync(reply);

//            context.Wait(this.MessageReceivedAsync);
//        }

//        private static IList<Attachment> GetCardsAttachments()
//        {
//            return new List<Attachment>()
//            {
//                GetHeroCard(
//                    "Azure Storage",
//                    "Offload the heavy lifting of data center management",
//                    "Store and help protect your data. Get durable, highly available data storage across the globe and pay only for what you use.",
//                    new CardImage(url: "https://docs.microsoft.com/en-us/aspnet/aspnet/overview/developing-apps-with-windows-azure/building-real-world-cloud-apps-with-windows-azure/data-storage-options/_static/image5.png"),
//                    new CardAction(ActionTypes.OpenUrl, "Learn more", value: "https://azure.microsoft.com/en-us/services/storage/")),
//                GetThumbnailCard(
//                    "DocumentDB",
//                    "Blazing fast, planet-scale NoSQL",
//                    "NoSQL service for highly available, globally distributed apps—take full advantage of SQL and JavaScript over document and key-value data without the hassles of on-premises or virtual machine-based cloud database options.",
//                    new CardImage(url: "https://docs.microsoft.com/en-us/azure/documentdb/media/documentdb-introduction/json-database-resources1.png"),
//                    new CardAction(ActionTypes.OpenUrl, "Learn more", value: "https://azure.microsoft.com/en-us/services/documentdb/")),
//                GetHeroCard(
//                    "Azure Functions",
//                    "Process events with a serverless code architecture",
//                    "An event-based serverless compute experience to accelerate your development. It can scale based on demand and you pay only for the resources you consume.",
//                    new CardImage(url: "https://msdnshared.blob.core.windows.net/media/2016/09/fsharp-functions2.png"),
//                    new CardAction(ActionTypes.OpenUrl, "Learn more", value: "https://azure.microsoft.com/en-us/services/functions/")),
//                GetThumbnailCard(
//                    "Cognitive Services",
//                    "Build powerful intelligence into your applications to enable natural and contextual interactions",
//                    "Enable natural and contextual interaction with tools that augment users' experiences using the power of machine-based intelligence. Tap into an ever-growing collection of powerful artificial intelligence algorithms for vision, speech, language, and knowledge.",
//                    new CardImage(url: "https://msdnshared.blob.core.windows.net/media/2017/03/Azure-Cognitive-Services-e1489079006258.png"),
//                    new CardAction(ActionTypes.OpenUrl, "Learn more", value: "https://azure.microsoft.com/en-us/services/cognitive-services/")),
//            };
//        }

//        private static Attachment GetHeroCard(string title, string subtitle, string text, CardImage cardImage, CardAction cardAction)
//        {
//            var heroCard = new HeroCard
//            {
//                Title = title,
//                Subtitle = subtitle,
//                Text = text,
//                Images = new List<CardImage>() { cardImage },
//                Buttons = new List<CardAction>() { cardAction },
//            };

//            return heroCard.ToAttachment();
//        }

//        private static Attachment GetThumbnailCard(string title, string subtitle, string text, CardImage cardImage, CardAction cardAction)
//        {
//            var heroCard = new ThumbnailCard
//            {
//                Title = title,
//                Subtitle = subtitle,
//                Text = text,
//                Images = new List<CardImage>() { cardImage },
//                Buttons = new List<CardAction>() { cardAction },
//            };

//            return heroCard.ToAttachment();
//        }
//    }
//}



