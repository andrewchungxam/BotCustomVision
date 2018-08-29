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
    public class QandADialog : IDialog<object>
    {
        private QandAService _QnAServiceS1 =
        new QandAService
            (
                ConfigurationManager.AppSettings["QandAHost1"],
                ConfigurationManager.AppSettings["QandAKnowledgebaseId1"],
                ConfigurationManager.AppSettings["QandAEndPointKey1"]
            );

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedASync);
            return Task.CompletedTask;
        }

        public async Task MessageReceivedASync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            string safeText = HttpUtility.UrlEncode(((IMessageActivity)context.Activity).Text);
            string answer = await _QnAServiceS1.GetAnswer(safeText);
            if (string.IsNullOrEmpty(answer))
            {
                string regularText = ((IMessageActivity)context.Activity).Text;
                string regularAnswer = await _QnAServiceS1.GetAnswer(regularText);

                if (string.IsNullOrEmpty(regularAnswer))
                {
                    await context.PostAsync("No good match found in KB.");
                }
                else
                {
                    await context.PostAsync(regularAnswer);
                }
            }
            else
            {
                await context.PostAsync(answer);
                //context.Done(true);
            }
        }
    }
}