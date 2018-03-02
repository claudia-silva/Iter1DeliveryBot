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
    public class TrackParcelDialog : IDialog<string>
    {
        private string trackingNo;

        public TrackParcelDialog(string trackingNo)
        {
            this.trackingNo = trackingNo;
        }
        public Task StartAsync(IDialogContext context)
        {
            PromptDialog.Text(context, NextSteps, $@"Your parcel with Track No: {this.trackingNo} is currently with your local Hermes and will be delivered to you today by 4pm.");
            return Task.CompletedTask;
        }

        public async Task NextSteps(IDialogContext context, IAwaitable<string> result)
        {
            await context.PostAsync("Is there anything else that DeliveryBot can help you with?");
            PromptDialog.Choice(context, this.TrackParcelResumeAfter, new List<string>() { "Yes", "No" }, "Please select an option?");
        }

        public async Task TrackParcelResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            var optionSelected = await result;

            switch (optionSelected)
            {
                case "Yes":
                    context.Done(trackingNo);
                    break;
                case "No":
                    PromptDialog.Text(context, TrackParcelResumeAfter, "Thank you for using DeliveryBot, Hope to speak with you again soon!");
                    break;
            }

        }
    }
}