using System;
using System.Threading.Tasks;
using LuisAndQandA1s;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace LuisAndQandA1
{
#pragma warning disable 1998

    [Serializable]
    public class FirstTimeConversationDialog : IDialog<bool>
    {
        private const string PhoneRegexPattern = @"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$";
        private const string EmailRegexPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";

        private CustomerInfo _customerInfo = new CustomerInfo();

        //public FirstTimeConversationDialog(CustomerInfo customer)
        //{
        //    this._customerInfo = customer;
        //}
        
        private int attempts = 3;

        //public async Task StartAsync(IDialogContext context)
        //{
        //    await context.PostAsync("What is your name?");

        //    context.Wait(this.MessageReceivedAsync);
        //}

        //private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        //{
        //    var customerName = await result;

        //    /* If the message returned is a valid name, return it to the calling dialog. */
        //    if ((customerName.Text != null) && (customerName.Text.Trim().Length > 0))
        //    {
        //        /* Completes the dialog, removes it from the dialog stack, and returns the result to the parent/calling
        //            dialog. */
        //        //context.Done(message.Text);
        //        this._customerInfo.Name = customerName;      
        //    }
        //    /* Else, try again by re-prompting the user. */
        //    else
        //    {
        //        --attempts;
        //        if (attempts > 0)
        //        {
        //            await context.PostAsync("I'm sorry, I don't understand your reply. What is your name (e.g. 'Bill', 'Melinda')?");

        //            context.Wait(this.MessageReceivedAsync);
        //        }
        //        else
        //        {
        //            /* Fails the current dialog, removes it from the dialog stack, and returns the exception to the 
        //                parent/calling dialog. */
        //            context.Fail(new TooManyAttemptsException("Message was not a string or was an empty string."));
        //        }
        //    }


        public async Task StartAsync(IDialogContext context)
        {


            var promptPhoneDialog = new PromptStringRegex(
                "Please enter your phone number (to get in touch with you):",
                PhoneRegexPattern,
                "The value entered is not phone number. Please try again using the following format (xyz) xyz-wxyz:",
                "You have tried to enter your phone number many times. Please try again later.",
                attempts: 2);

            //context.Call(promptPhoneDialog, this.ResumeAfterPhoneEntered);
            context.Call(promptPhoneDialog, this.AfterPhoneEnteredPromptForEmail);

        }

        private async Task AfterPhoneEnteredPromptForEmail(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                var phone = await result;

                if (phone != null)
                {
                    _customerInfo.PhoneNumber = phone;

                    await context.PostAsync($"The phone you provided is: {phone}");

                    var promptEmailDialog = new PromptStringRegex(
                        "Please enter your email address (you can use this to reference your support case later):",
                        EmailRegexPattern,
                        "The value entered is not a valid email email address. Please try again using the following format username@company.com",
                        "You have tried to enter your email too many times. Please try again later.",
                        attempts: 2);

                    context.Call(promptEmailDialog, this.AfterEmailEnteredPromptForPurchaseDate);
                }
                else
                {
                    context.Done(false);
                }
            }
            catch (TooManyAttemptsException)
            {
                context.Done(false);
            }

        }

        private async Task AfterEmailEnteredPromptForPurchaseDate(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                var theemail = await result;

                if (theemail != null)
                {
                    this._customerInfo.EmailAddress = theemail;

                    await context.PostAsync($"The email you provided is: {theemail}");

                    var promptBirthDialog = new PromptDate(
                        "When did you purchase your product?  Please enter the APPROXIMATE date (MM/dd/yyyy):",
//                        "Please enter your date of birth (MM/dd/yyyy):",
                        "The value you entered is not a valid date. Please try again:",
                        "You have tried to enter your date of purchase many times. Please try again later.",
                        attempts: 2);

                    context.Call(promptBirthDialog, this.AfterDateOfPurchaseEntered);
                }
                else
                {
                    context.Done(false);
                }
            }
            catch (TooManyAttemptsException)
            {
                context.Done(false);
            }
        }



        private async Task AfterDateOfPurchaseEntered(IDialogContext context, IAwaitable<DateTime> result)
        {
            try
            {
                var dateOfPurchase = await result;

                if (dateOfPurchase != DateTime.MinValue)
                {
                    this._customerInfo.DateOfPurchase = dateOfPurchase;
                    
                    var newCaseNumber = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 5).ToUpper();

                    await context.PostAsync($"The date you provided is: {dateOfPurchase.ToShortDateString()}.");
                    await context.PostAsync($"Your new case number is _{newCaseNumber}_.");
                    await context.PostAsync($"Thank you {this._customerInfo.Name} for adding that info.  We've registered your phone number as {this._customerInfo.PhoneNumber} and your email as {this._customerInfo.EmailAddress}.  We've emailed you the case number so you can refer to it in the future.");

                    context.Done(true);
                }
                else
                {
                    context.Done(false);
                }
            }
            catch (TooManyAttemptsException)
            {
                context.Done(false);
            }
        }
    }
}