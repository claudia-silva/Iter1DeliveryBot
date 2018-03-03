using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Microsoft.Bot.Builder.Dialogs.PromptDialog;

namespace DeliveryBot.Dialogs
{
    [Serializable]
    public class ChangeAddressDialog : IDialog<string>
    {
        private string trackingNo;
        private string optionSelected;

        public ChangeAddressDialog(string trackingNo)
        {
            this.trackingNo = trackingNo;
        }
        public Task StartAsync(IDialogContext context)
        {
            PromptDialog.Choice(context, this.ChangeAddressNextSteps, new List<string>() { "Yes", "No" }, $@"Your parcel with Track No: { this.trackingNo} is being delivered to the Address: 1 Bob Road, LD1 0NS.Would you like to change this ?");
            return Task.CompletedTask;
        }

        public async Task ChangeAddressRequired(IDialogContext context, IAwaitable<string> result)
        {
            PromptDialog.Choice(context, this.ChangeAddressNextSteps, new List<string>() { "Yes", "No" }, "Please select an option?");
        }

        public async Task ChangeAddressNextSteps(IDialogContext context, IAwaitable<string> result)
        {

            optionSelected = await result;
            switch (optionSelected)
            {
                case "Yes":
                    PromptDialog.Text(context, ChangeAddress, "What would you like to change it to?");
                    break;
                case "No":
                    PromptDialog.Text(context, ChangeAddressResumeAfter, "");
                    break;
            }
        }

        public async Task ChangeAddress(IDialogContext context, IAwaitable<string> result)
        {
            optionSelected = await result;
            PromptDialog.Text(context, NextSteps, $@"Your parcel with Track No: {this.trackingNo} will now be delivered to {this.optionSelected}");
        }

        public async Task NextSteps(IDialogContext context, IAwaitable<string> result)
        {
            await context.PostAsync("Is there anything else that DeliveryBot can help you with?");
            PromptDialog.Choice(context, this.ChangeAddressResumeAfter, new List<string>() { "Yes", "No" }, "Please select an option?");
        }

        public async Task ChangeAddressResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            optionSelected = await result;

            switch (optionSelected)
            {
                case "Yes":
                    context.Done(trackingNo);
                    break;
                case "No":
                    PromptDialog.Text(context, ChangeAddressResumeAfter, "Thank you for using DeliveryBot, Hope to speak with you again soon!");
                    break;
            }
        }
    }
}