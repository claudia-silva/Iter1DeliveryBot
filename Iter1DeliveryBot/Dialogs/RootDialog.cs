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
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        public Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            PromptDialog.Text(context, NameEntered, @"Hi! What's your name?");

            return Task.CompletedTask;
        }

        public async Task NameEntered(IDialogContext context, IAwaitable<string> result)
        {
            var nameEntered = await result;
            await context.PostAsync($@"Hi {nameEntered}! I am DeliveryBot, I can help with the following:");
            PromptDialog.Choice(context, this.InitialMenuSelection, new List<string>() { "Delivery", "Other" }, "Please select an option?");
        }

        public async Task InitialMenuSelection(IDialogContext context, IAwaitable<string> result)
        {
            var optionSelected = await result;

            switch (optionSelected)
            {
                case "Delivery":
                    await context.PostAsync($"You selected {optionSelected}");
                    //context.Call(new DeliveryDialog(), this.NameEntered);

                    break;
                case "other":
                    await context.PostAsync($"You selected {optionSelected}");
                    break;
            }
        }

        //public async Task DeliveryDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        //{
        //    var trackingNo = await result;
        //    await context.PostAsync($@"You entered {trackingNo}, Are you sure this is your Tracking No?");

        //}

    }
}
