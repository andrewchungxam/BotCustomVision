using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace LuisAndQandA1.Dialogs
{
    [Serializable]
    public class CoordinatedQandADialog2 : IDialog<bool> //IDialog<object>
    {
        private QandAService _QnAServiceC2 =
        new QandAService
            (
                ConfigurationManager.AppSettings["QandAHost2"],
                ConfigurationManager.AppSettings["QandAKnowledgebaseId2"],
                ConfigurationManager.AppSettings["QandAEndPointKey2"]
            );

        public Task StartAsync(IDialogContext context)
        {
            string TopicOfQandAMaker = "the Prince of Wales and the Duchess of Cornwall";
            context.PostAsync(String.Format("Welcome - what question did you have about {0}?", TopicOfQandAMaker));
            
            context.Wait(MessageReceivedASync);
            return Task.CompletedTask;
        }

        public async Task MessageReceivedASync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            string safeText = HttpUtility.UrlEncode(((IMessageActivity)context.Activity).Text);
            string answer = await _QnAServiceC2.GetAnswer(safeText);
            if (string.IsNullOrEmpty(answer))
            {
                string regularText = ((IMessageActivity)context.Activity).Text;
                string regularAnswer = await _QnAServiceC2.GetAnswer(regularText);

                if (string.IsNullOrEmpty(regularAnswer))
                {
                    await context.PostAsync("No good match found in KB.");
                    context.Done(false);
                }
                else
                {
                    await context.PostAsync(regularAnswer);
                    context.Done(true);
                }
            }
            else
            {
                await context.PostAsync(answer);
                context.Done(true);

            }
        }
    }
}