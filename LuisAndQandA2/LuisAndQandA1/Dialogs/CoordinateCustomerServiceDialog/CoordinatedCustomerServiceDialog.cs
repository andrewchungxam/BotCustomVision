using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace LuisAndQandA1.Dialogs
{

#pragma warning disable 1998

    [Serializable]
    public class CoordinatedCustomerServiceDialog : IDialog<object>
    {
        private const string NewCustomerSupportInfo = "Enter New Customer Info";
        private const string ComputerVisionCaption = "Computer Vision: Captions";
        private const string ComputerVisionOCRandBarcode = "Computer Vision: Barcode and OCR";
        private const string CustomVision = "Custom Vision";
        private const string QandATaxes = "Q&A: Taxes";
        private const string QandARoyalty = "Q&A: Royalty";
        private const string ProductInfoLinks = "Product Info Links";

        private const string ChangePasswordOption = "Change Password";


        public async Task StartAsync(IDialogContext context)
        {
            //Option 1
            //System.Threading.Thread.Sleep(1000);
            await context.PostAsync("Welcome to the customer service help desk! How can we help you today?");
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
                    NewCustomerSupportInfo,
                    ComputerVisionCaption,
                    ComputerVisionOCRandBarcode,
                    CustomVision,
                    QandATaxes,
                    QandARoyalty,
                    ProductInfoLinks
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
                    NewCustomerSupportInfo,
                    ComputerVisionCaption,
                    ComputerVisionOCRandBarcode,
                    CustomVision,
                    QandATaxes,
                    QandARoyalty,
                    ProductInfoLinks
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

                    case NewCustomerSupportInfo:
                        context.Call(new FirstTimeConversationDialog(), this.AfterResetPassword);
                        break;

                    case ComputerVisionCaption:
                        context.Call(new CoordinatedComputerVisionCaptionDialog(), this.AfterRoyaltyInfo);
                        break;                        

                    case ComputerVisionOCRandBarcode:
                        //context.Call(new SimpleZXingBarcodeDialog(), this.AfterTaxHelp);
                        context.Call(new CoordinatedComputerVisionOCRandBarcodeDialog(), this.AfterRoyaltyInfo);
                        break;

                    case CustomVision:
                        context.Call(new CoordinatedCustomImageCaptionDialog(), this.AfterRoyaltyInfo);
                        break;                        

                    case QandATaxes:
                        context.Call(new CoordinatedQandADialog(), this.AfterTaxHelp);
                        break;
                        
                    case QandARoyalty:
                        context.Call(new CoordinatedQandADialog2(), this.AfterRoyaltyInfo);
                        break;

                    case ProductInfoLinks:
                        context.Call(new CoordinatedHRLinksDialog(), this.AfterHRLinks);
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