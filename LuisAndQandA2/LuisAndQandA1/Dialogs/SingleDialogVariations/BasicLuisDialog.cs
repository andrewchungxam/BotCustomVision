using System;
using System.Configuration;
using System.Threading.Tasks;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

namespace LuisAndQandA1.Dialogs
{
    // For more information about this template visit http://aka.ms/azurebots-csharp-luis
    [Serializable]
    public class BasicLuisDialog : LuisDialog<object>
    {
        public BasicLuisDialog() : base(new LuisService(new LuisModelAttribute(
            ConfigurationManager.AppSettings["LuisAppId"],
            ConfigurationManager.AppSettings["LuisAPIKey"],
            domain: ConfigurationManager.AppSettings["LuisAPIHostName"])))
        {
        }

        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        [LuisIntent("Input.NewUser")]
        public async Task InputNewUserIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        [LuisIntent("HR.Links")]
        public async Task HRLinksIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }
        
        [LuisIntent("Taxes.Questions")]
        public async Task TaxesQuestionsIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        [LuisIntent("Marathon.Royalties")]
        public async Task MarathonRoyaltiesIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }
        
        [LuisIntent("Password.Reset")]
        public async Task PasswordResetIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }
        
        [LuisIntent("Communication.Confirm")]
        public async Task CommunicationConfirmIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        [LuisIntent("Communication.GoBack")]
        public async Task CommunicationGoBackIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }


        private async Task ShowLuisResult(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"You have reached {result.Intents[0].Intent} with your input.  This allows me to start that dialog."); //You said: {result.Query}");
            context.Wait(MessageReceived);
        }

    }
}






































// Go to https://luis.ai and create a new intent, then train/publish your luis app.
// Finally replace "Greeting" with the name of your newly created intent in the following handler
//[LuisIntent("Greeting")]
//public async Task GreetingIntent(IDialogContext context, LuisResult result)
//{
//    await this.ShowLuisResult(context, result);
//}

//[LuisIntent("Cancel")]
//public async Task CancelIntent(IDialogContext context, LuisResult result)
//{
//    await this.ShowLuisResult(context, result);
//}

//[LuisIntent("Help")]
//public async Task HelpIntent(IDialogContext context, LuisResult result)
//{
//    await this.ShowLuisResult(context, result);
//}