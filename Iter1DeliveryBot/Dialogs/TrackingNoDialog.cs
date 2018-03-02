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
    public class TrackingNoDialog : IDialog<string>
    {
        private string trackingNo;
        private string optionSelected;

        public TrackingNoDialog(string optionSelected)
        {
            this.optionSelected = optionSelected;
        }
        public Task StartAsync(IDialogContext context)
        {
            PromptDialog.Text(context, TrackNoResumeAfter, "Please enter your Tracking No?");
            return Task.CompletedTask;
        }

        public async Task TrackNoResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            trackingNo = await result;
            await context.PostAsync($@"Is this your Tracking No: { trackingNo}?");
            PromptDialog.Choice(context, this.VerifyTrackNo, new List<string>() { "Yes", "No" }, "Please select an option?");
        }

        public async Task VerifyTrackNo(IDialogContext context, IAwaitable<string> result)
        {
            var optionSel = await result;

            if (optionSel == "Yes")
            {
                switch (optionSelected)
                {
                    case "Track a Parcel":
                        context.Call(new TrackParcelDialog(trackingNo), TrackParcelResumeAfter);
                        break;
                    case "Re-arrange Delivery Date or Time":
                        context.Call(new ReArrangeDialog(trackingNo), ReArrangeResumeAfter);
                        break;
                    case "Change Delivery Address":
                        //PromptDialog.Text(context, DeliveryDialogResumeAfter, "Please enter your Tracking No?");
                        break;
                    case "Collect Parcel from a Local Service Point":
                        //PromptDialog.Text(context, DeliveryDialogResumeAfter, "Please enter your Tracking No?");
                        break;
                }
            }
            else
                context.Call(new TrackingNoDialog(optionSel), TrackNoResumeAfter);
        }


        public async Task TrackParcelResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            var optionSelected = await result;
            context.Done(trackingNo);
        }

        public async Task ReArrangeResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            var optionSelected = await result;
            context.Done(trackingNo);
        }
    }
}