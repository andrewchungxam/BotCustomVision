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
    public class QandADialog2 : IDialog<object>
    {
        private QandAService _QnAServiceS2 =
        new QandAService
            (
                ConfigurationManager.AppSettings["QandAHost2"],
                ConfigurationManager.AppSettings["QandAKnowledgebaseId2"],
                ConfigurationManager.AppSettings["QandAEndPointKey2"]
            );

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedASync);
            return Task.CompletedTask;
        }

        public async Task MessageReceivedASync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            string safeText = HttpUtility.UrlEncode(((IMessageActivity)context.Activity).Text);
            string answer = await _QnAServiceS2.GetAnswer(safeText);
            if (string.IsNullOrEmpty(answer))
            {
                string regularText = ((IMessageActivity)context.Activity).Text;
                string regularAnswer = await _QnAServiceS2.GetAnswer(regularText);

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
            }
        }
    }
}