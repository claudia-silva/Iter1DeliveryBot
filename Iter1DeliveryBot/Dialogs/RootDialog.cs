using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Microsoft.Bot.Builder.Dialogs.PromptDialog;

namespace Iter1DeliveryBot.Dialogs
{

    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public string nameEntered;

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        public Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            PromptDialog.Text(context, NameEntered, @"Hi I am DeliveryBot, What's your name?");

            return Task.CompletedTask;
        }

        private async Task SendWelcomeMessageAsync(IDialogContext context)
        {
            await context.PostAsync($@"Hi {nameEntered}! I can help with the following:");
            PromptDialog.Choice(context, this.InitialMenuSelection, new List<string>() { "Delivery", "Other" }, "Please select an option?");
        }

        public async Task NameEntered(IDialogContext context, IAwaitable<string> result)
        {
            nameEntered = await result;
            await this.SendWelcomeMessageAsync(context);
        }

        public async Task InitialMenuSelection(IDialogContext context, IAwaitable<string> result)
        {
            var optionSelected = await result;

            switch (optionSelected)
            {
                case "Delivery":
                    //await context.PostAsync($"You selected {optionSelected}");                  
                    context.Call(new DeliveryDialog(), this.DeliveryDialogResumeAfter);

                    break;
                case "other":
                    // await context.PostAsync($"You selected {optionSelected}");
                    break;
            }
        }

        public async Task DeliveryDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            await this.SendWelcomeMessageAsync(context);
        }

    }
}
