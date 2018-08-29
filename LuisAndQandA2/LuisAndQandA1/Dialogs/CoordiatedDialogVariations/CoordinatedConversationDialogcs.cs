using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace LuisAndQandA1.Dialogs
{

#pragma warning disable 1998

    [Serializable]
    public class CoordinatedConversationDialog : IDialog<object>
    {
        private const string EnterUserInfo = "Enter new employee info";
        private const string HRLinks = "HR Links";
        private const string TaxHelp = "Tax help";
        private const string RoyaltyInfo = "Royalty info";

        private const string ResetPasswordOption = "Reset Password";

        private const string ChangePasswordOption = "Change Password";


        public async Task StartAsync(IDialogContext context)
        {
            //Option 1
            //System.Threading.Thread.Sleep(1000);
            await context.PostAsync("Welcome! Do you need help today?");
            context.Wait(this.MessageReceivedAsync);

            //Option 2
            //this.NoMessageNeededAsync(context);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            PromptDialog.Choice(
                context,
                this.AfterChoiceSelected,
                new[] {
                    EnterUserInfo,
                    HRLinks,
                    TaxHelp,
                    RoyaltyInfo,
                    ResetPasswordOption
                    //ChangePasswordOption, ResetPasswordOption
                },
                "Welcome - what do you want to do today?",
                "I am sorry but I didn't understand that. I need you to select one of the options below",
                attempts: 2);
        }

        private async Task NoMessageNeededAsync(IDialogContext context)
        {
            System.Threading.Thread.Sleep(2000);
            PromptDialog.Choice(
                context,
                this.AfterChoiceSelected,
                new[] {
                    EnterUserInfo,
                    HRLinks,
                    TaxHelp,
                    RoyaltyInfo,
                    ResetPasswordOption
                    //ChangePasswordOption, ResetPasswordOption
                },
                "Welcome - what do you want to do today?",
                "I am sorry but I didn't understand that. I need you to select one of the options below",
                attempts: 2);
        }

        private async Task AfterChoiceSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                var selection = await result;

                switch (selection)
                {

                //EnterUserInfo
                //HRLinks
                //TaxHelp
                //RoyaltyInfo
                //ResetPasswordOption
                //ChangePasswordOption

                    case EnterUserInfo:
                        context.Call(new CoordinatedNameAgeRootDialog(), this.AfterEnterUserInfo);
                        //context.Forward(new CoordinatedNameAgeRootDialog(), this.AfterResetPassword, selection, CancellationToken.None);
                        break;

                    case HRLinks:
                        context.Call(new CoordinatedHRLinksDialog(), this.AfterHRLinks);
                        //await this.StartAsync(context);
                        //context.Call(new CoordinatedQandADialog(), this.AfterResetPassword);
                        break;                        

                    case TaxHelp:
                        context.Call(new CoordinatedQandADialog(), this.AfterTaxHelp);

                        break;

                    case RoyaltyInfo:
                        context.Call(new CoordinatedQandADialog2(), this.AfterRoyaltyInfo);
                        break;                        

                    //case ChangePasswordOption:
                    //    await context.PostAsync("This functionality is not yet implemented! Try resetting your password.");
                    //    await this.StartAsync(context);
                    //    break;  
                    

                    case ResetPasswordOption:
                        context.Call(new ResetPasswordDialog(), this.AfterResetPassword);
                        break;
                }
            }
            catch (TooManyAttemptsException)
            {
                //await this.StartAsync(context);
                await this.NoMessageNeededAsync(context);

            }
        }

        private async Task AfterEnterUserInfo(IDialogContext context, IAwaitable<bool> result)
        {
            var success = await result;

            if (!success)
            {
                await context.PostAsync("We didn't get your user info - if you'd like to try again, please select \"Enter user info\" from the menu.");
            }

            //await this.StartAsync(context);
            await this.NoMessageNeededAsync(context);

        }

        private async Task AfterHRLinks(IDialogContext context, IAwaitable<bool> result)
        {
            var success = await result;

            if (!success)
            {
                await context.PostAsync("Sorry we couldn't help - if you'd like to try again, please select \"HR Links\" from the menu.");
            }

            //await this.StartAsync(context);
            await this.NoMessageNeededAsync(context);

        }

        private async Task AfterTaxHelp(IDialogContext context, IAwaitable<bool> result)
        {
            var success = await result;

            if (!success)
            {
                await context.PostAsync("Sorry we couldn't help - if you'd like to try again, please select \"Tax help\" from the menu.");
            }

            //await this.StartAsync(context);
            await this.NoMessageNeededAsync(context);

        }

        private async Task AfterRoyaltyInfo(IDialogContext context, IAwaitable<bool> result)
        {
            var success = await result;

            if (!success)
            {
                await context.PostAsync("Sorry we couldn't help - if you'd like to try again, please select \"Royalty info\" from the menu.");
            }

            //await this.StartAsync(context);
            await this.NoMessageNeededAsync(context);

        }



        private async Task AfterResetPassword(IDialogContext context, IAwaitable<bool> result)
        {
            var success = await result;

            if (!success)
            {
                await context.PostAsync("Your identity was not verified and your password cannot be reset");
            }

            //await this.StartAsync(context);
            await this.NoMessageNeededAsync(context);

        }


    }
}