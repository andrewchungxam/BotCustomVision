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
    public class CoordinatedLuisDialog : LuisDialog<object>
    {
        public CoordinatedLuisDialog() : base(new LuisService(new LuisModelAttribute(
            ConfigurationManager.AppSettings["LuisAppId"],
            ConfigurationManager.AppSettings["LuisAPIKey"],
            domain: ConfigurationManager.AppSettings["LuisAPIHostName"])))
        {
        }


        [LuisIntent("Input.NewUser")]
        public async Task InputNewUserIntent(IDialogContext context, LuisResult result)
        {
            context.Call(new CoordinatedNameAgeRootDialog(), this.AfterResetPassword);
        }

        [LuisIntent("HR.Links")]
        public async Task HRLinksIntent(IDialogContext context, LuisResult result)
        {
            context.Call(new CoordinatedHRLinksDialog(), this.AfterHRLinks);
        }

        [LuisIntent("Taxes.Questions")]
        public async Task TaxesQuestionsIntent(IDialogContext context, LuisResult result)
        {
            context.Call(new CoordinatedQandADialog(), this.AfterTaxHelp);
        }

        [LuisIntent("Marathon.Royalties")]
        public async Task MarathonRoyaltiesIntent(IDialogContext context, LuisResult result)
        {
            context.Call(new CoordinatedQandADialog2(), this.AfterRoyaltyInfo);
        }

        [LuisIntent("Password.Reset")]
        public async Task PasswordResetIntent(IDialogContext context, LuisResult result)
        {
            context.Call(new ResetPasswordDialog(), this.AfterResetPassword);
        }

        [LuisIntent("Communication.Confirm")]
        public async Task CommunicationConfirmIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
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
        
        private async Task AfterEnterUserInfo(IDialogContext context, IAwaitable<bool> result)
        {
            var success = await result;

            if (!success)
            {
                await context.PostAsync("We didn't get your user info - please try again!");
            }

            System.Threading.Thread.Sleep(1000);
            await context.PostAsync("How can I help?");
            await this.StartAsync(context);
        }

        private async Task AfterHRLinks(IDialogContext context, IAwaitable<bool> result)
        {
            var success = await result;

            if (!success)
            {
                await context.PostAsync("Sorry we couldn't help - please try again!");
            }

            System.Threading.Thread.Sleep(1000);
            await context.PostAsync("How can I help?");
            await this.StartAsync(context);
        }

        private async Task AfterTaxHelp(IDialogContext context, IAwaitable<bool> result)
        {
            var success = await result;

            if (!success)
            {
                await context.PostAsync("Sorry we couldn't help - please try again!");
            }
            System.Threading.Thread.Sleep(1000);
            await context.PostAsync("How can I help?");
            await this.StartAsync(context);
        }

        private async Task AfterRoyaltyInfo(IDialogContext context, IAwaitable<bool> result)
        {
            var success = await result;

            if (!success)
            {
                await context.PostAsync("Sorry we couldn't help - please try again!");
            }
            System.Threading.Thread.Sleep(1000);
            await context.PostAsync("How can I help?");
            await this.StartAsync(context);
        }
       

        private async Task AfterResetPassword(IDialogContext context, IAwaitable<bool> result)
        {
            var success = await result;

            if (!success)
            {
                await context.PostAsync("Your identity was not verified and your password cannot be reset");
            }

            System.Threading.Thread.Sleep(1000);
            await context.PostAsync("How can I help?");
            await this.StartAsync(context);
        }
    }
}