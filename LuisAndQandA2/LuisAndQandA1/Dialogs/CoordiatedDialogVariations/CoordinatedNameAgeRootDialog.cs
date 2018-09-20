using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace LuisAndQandA1.Dialogs
{
#pragma warning disable 1998

    [Serializable]
    public class CoordinatedNameAgeRootDialog : IDialog<bool> //IDialog<object>
    {
        private string name;
        private int age;

        public async Task StartAsync(IDialogContext context)
        {
            /* Wait until the first message is received from the conversation and call MessageReceviedAsync 
             *  to process that message. */
            //context.Wait(this.MessageReceivedAsync);
            await this.SendWelcomeMessageAsync(context);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            /* When MessageReceivedAsync is called, it's passed an IAwaitable<IMessageActivity>. To get the message,
             *  await the result. */
            //var message = await result;

            //await this.SendWelcomeMessageAsync(context);
        }

        private async Task SendWelcomeMessageAsync(IDialogContext context)
        {
            await context.PostAsync("Hi - we're glad you're here.  Please enter your customer information so can best help you and reference your support ticket in the future.");

            context.Call(new SimpleNameDialog(), this.NameDialogResumeAfter);
        }

        private async Task NameDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                this.name = await result;

                context.Call(new SimpleAgeDialog(this.name), this.AgeDialogResumeAfter);
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("I'm sorry, I'm having issues understanding you. Let's try again.");

                await this.SendWelcomeMessageAsync(context);
            }
        }

        private async Task AgeDialogResumeAfter(IDialogContext context, IAwaitable<int> result)
        {
            try
            {
                this.age = await result;

                await context.PostAsync($"Your name is { name } and your age is { age }.");

                context.Done(true);
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("I'm sorry, I'm having issues understanding you. Let's start over!");
                context.Done(false);

                //OPTION 1 : BETTER CONVERSATION FLOW - GOING STRAIGHT BACK INTO THE NAME DIALOG
                //context.Call(new SimpleNameDialog(), this.NameDialogResumeAfter);
            }
            finally
            {
                //OPTION -1: DEFAULT CONVERSATION FLOW - REPROMT AND REPEATING WELCOME MESSAGE
                // await this.SendWelcomeMessageAsync(context);

                //OPTION 2 : BETTER CONVERSATION FLOW - NO RE-PROMPT NOR REPEATING WELCOME MESSAGE
                //await Task.CompletedTask;
                //context.Done(false);
            }
        }
    }
}