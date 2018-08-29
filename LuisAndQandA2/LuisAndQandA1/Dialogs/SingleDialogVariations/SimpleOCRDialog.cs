using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;
using Microsoft.ProjectOxford.Vision;
using ImageCaption.Services;
using Microsoft.Cognitive.CustomVision.Prediction;
using System.Text;

namespace LuisAndQandA1.Dialogs
{
    //public class PredictionResult
    //{
    //    public string PredictionTag { get; set; }
    //    public decimal PredictionProbability { get; set; }

    //}

    [Serializable]
    public class SimpleOCRDialog : IDialog<string>
    {
        private readonly ICaptionService captionService = new MicrosoftCognitiveCaptionService();
        private int attempts = 3;
        private string itemType = "headphones";

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync($"Post an image of your {itemType}!");

            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            //// calculate something for us to return
            //int length = (activity.Text ?? string.Empty).Length;

            //// return our reply to the user
            //await context.PostAsync($"You sent {activity.Text} which was {length} characters");

            //context.Wait(MessageReceivedAsync);

            var imageAttachment = activity.Attachments?.FirstOrDefault(a => a.ContentType.Contains("image"));

            var connector = new ConnectorClient(new Uri(activity.ServiceUrl));

            //if (imageAttachment != null)
            //{
            //    using (var stream = await GetImageStream(connector, imageAttachment))
            //    {
            //        //return await this.captionService.GetCaptionAsync(stream);
            //        var textOfStream = await this.captionService.GetCaptionAsync(stream);
            //        await context.PostAsync($"{textOfStream}");
            //    }
            //}
            ////ELSE --??
            //context.Wait(MessageReceivedAsync);


            string predictionProjectId = "3e152a33-92ee-4326-b4c9-c8068edab9d8"; //"YOUR_PROJECT_ID", //This is the custom vision project we are predicting against
            string predictionKey = "eafbd7b247bd41c1afd6d6da352f7099";

            PredictionEndpoint endpoint = new PredictionEndpoint() { ApiKey = predictionKey };

            if (imageAttachment != null)
            {
                using (var stream = await GetImageStream(connector, imageAttachment))
                {
                    var predictionImageResult = endpoint.PredictImage(new Guid(predictionProjectId), stream);
                    StringBuilder predictionImageResultString = new StringBuilder();

                    var resultsList = new List<PredictionResult>();

                    foreach (var c in predictionImageResult.Predictions)
                    {

                        if (c.Probability > .50)
                        {
                            predictionImageResultString.Append(c.Tag);
                            predictionImageResultString.Append(", ");
                            var probability = (decimal)c.Probability * 100;
                            var probabilityFixed = probability.ToString("#.##");
                            predictionImageResultString.Append(probabilityFixed);
                            predictionImageResultString.Append("%");
                            predictionImageResultString.Append("; ");
                        }
                        //resultsDictionary.Add(c.Tag, (decimal)c.Probability);
                        //resultsList.Add(new PredictionResult() { PredictionTag = c.Tag, PredictionProbability = (decimal)c.Probability});
                    }

                    await context.PostAsync($"Azure Custom Vision results: {predictionImageResultString}");

                    // var resultsDictionaryString = resultsDictionary.ToString();
                    // await context.PostAsync($"{resultsDictionaryString}");

                    var resultsListToString = String.Join<PredictionResult>(", ", resultsList.ToArray());
                    //await context.PostAsync($"{resultsListToString}");

                }
            }

            context.Wait(MessageReceivedAsync);

            //if (imageAttachment != null)
            //{
            //    using (var stream = await GetImageStream(connector, imageAttachment))
            //    {
            //        //return await this.captionService.GetCaptionAsync(stream);
            //        var textOfStream = await this.captionService.GetCaptionAsync(stream);
            //        await context.PostAsync($"{textOfStream}");
            //    }
            //}

        }

        private static async Task<Stream> GetImageStream(ConnectorClient connector, Attachment imageAttachment)
        {
            using (var httpClient = new HttpClient())
            {
                // The Skype attachment URLs are secured by JwtToken,
                // you should set the JwtToken of your bot as the authorization header for the GET request your bot initiates to fetch the image.
                // https://github.com/Microsoft/BotBuilder/issues/662
                var uri = new Uri(imageAttachment.ContentUrl);
                if (uri.Host.EndsWith("skype.com") && uri.Scheme == "https")
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetTokenAsync(connector));
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/octet-stream"));
                }

                return await httpClient.GetStreamAsync(uri);
            }
        }

        /// <summary>
        /// Gets the JwT token of the bot. 
        /// </summary>
        /// <param name="connector"></param>
        /// <returns>JwT token of the bot</returns>
        private static async Task<string> GetTokenAsync(ConnectorClient connector)
        {
            var credentials = connector.Credentials as MicrosoftAppCredentials;
            if (credentials != null)
            {
                return await credentials.GetTokenAsync();
            }

            return null;
        }

    }
}

