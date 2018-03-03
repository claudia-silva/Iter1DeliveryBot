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
    public class CollectionDialog : IDialog<string>
    {
        private string trackingNo;
        private string optionSelected;

        public CollectionDialog(string trackingNo)
        {
            this.trackingNo = trackingNo;
        }
        public Task StartAsync(IDialogContext context)
        {
            PromptDialog.Choice(context, this.CollectionRequired, new List<string>() { "Yes", "No" }, $@"Your parcel with Track No: { this.trackingNo} is being delivered to the Address: 1 Bob Road, LD1 0NS. Would you like to now collect this at a Local Service Point?");
            return Task.CompletedTask;
        }

        public async Task CollectionRequired(IDialogContext context, IAwaitable<string> result)
        {
            optionSelected = await result;
            if (optionSelected == "Yes")
            {
                await this.ChangeToCollection(context);
            }
            else
            {
                //await this.ReArrangeTime(context);
            }
        }

        public async Task ChangeToCollection(IDialogContext context)
        {
            await context.PostAsync("These are the nearest Local Service Points to you:");
            PromptDialog.Choice(context, this.CollectionChangeResumeAfter, new List<string>() { "Asda", "Sainsburys", "Tesco" }, "These are the nearest Local Service Points to you: Please choose one?");
        }

        public async Task CollectionChangeResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            optionSelected = await result;
            PromptDialog.Text(context, NextSteps, $@"Your parcel with Track No: {this.trackingNo} will now be delivered to {this.optionSelected}");
        }
        public async Task NextSteps(IDialogContext context, IAwaitable<string> result)
        {
            await context.PostAsync("Is there anything else that DeliveryBot can help you with?");
            PromptDialog.Choice(context, this.CollectionResumeAfter, new List<string>() { "Yes", "No" }, "Please select an option?");
        }

        public async Task CollectionResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            optionSelected = await result;

            switch (optionSelected)
            {
                case "Yes":
                    context.Done(trackingNo);
                    break;
                case "No":
                    PromptDialog.Text(context, CollectionResumeAfter, "Thank you for using DeliveryBot, Hope to speak with you again soon!");
                    break;
            }
        }
    }
}