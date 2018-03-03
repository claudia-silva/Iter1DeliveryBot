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
    public class ReArrangeDialog : IDialog<string>
    {
        private string trackingNo;
        private string optionSelected;

        public ReArrangeDialog(string trackingNo)
        {
            this.trackingNo = trackingNo;
        }
        public Task StartAsync(IDialogContext context)
        {
            PromptDialog.Choice(context, ReArrangeNextSteps, new List<string>() { "Re-Arrange Date", "Re-Arrange Time" }, "Please select an option?");
            return Task.CompletedTask;
        }

        public async Task ReArrangeNextSteps(IDialogContext context, IAwaitable<string> result)
        {
            optionSelected = await result;
            if (optionSelected == "Re-Arrange Date")
            {
                await this.ReArrangeDate(context);
            }
            else
                await this.ReArrangeTime(context);
        }

        public async Task ReArrangeDate(IDialogContext context)
        {
            await context.PostAsync($@"Your parcel with Track No: {this.trackingNo} is currently with your local Hermes and will be delivered to you today by 4pm. What would you like to change it to?");
            PromptDialog.Choice(context, this.ReArrangeDateResumeAfter, new List<string>() { "Monday", "Tuesday", "Wednesday" }, "These are the available dates, Please select an option?");
        }

        public async Task ReArrangeDateResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            optionSelected = await result;
            PromptDialog.Text(context, NextSteps, $@"Your parcel with Track No: {this.trackingNo} is currently with your local Hermes and will be delivered to you on {this.optionSelected}");
        }

        public async Task ReArrangeTime(IDialogContext context)
        {
            await context.PostAsync($@"Your parcel with Track No: {this.trackingNo} is currently with your local Hermes and will be delivered to you today by 4pm. What would you like to change it to?");
            PromptDialog.Choice(context, this.ReArrangeTimeResumeAfter, new List<string>() { "10:00AM", "11:00AM", "12:00PM", "13:00PM" }, "These are the available times, Please select an option?");
        }

        public async Task ReArrangeTimeResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            optionSelected = await result;
            PromptDialog.Text(context, NextSteps, $@"Your parcel with Track No: {this.trackingNo} is currently with your local Hermes and will be delivered to you at {this.optionSelected}");
        }

        public async Task NextSteps(IDialogContext context, IAwaitable<string> result)
        {
            PromptDialog.Choice(context, this.ReArrangeResumeAfter, new List<string>() { "Yes", "No" }, "Is there anything else that DeliveryBot can help you with?");
        }

        public async Task ReArrangeResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            var optionSelected = await result;

            switch (optionSelected)
            {
                case "Yes":
                    context.Done(trackingNo);
                    break;
                case "No":
                    PromptDialog.Text(context, ReArrangeResumeAfter, "Thank you for using DeliveryBot, Hope to speak with you again soon!");
                    break;
            }
        }
    }
}