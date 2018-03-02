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
            var optionSelected = await result;

            switch (optionSelected)
            {
                case "Yes":
                    // await context.PostAsync($"You selected {optionSelected}");
                    context.Call(new TrackParcelDialog(trackingNo), TrackParcelResumeAfter);
                    break;
                case "No":
                    //await context.PostAsync($"You selected {optionSelected}");
                    context.Call(new TrackingNoDialog(), TrackNoResumeAfter);
                    break;
            }

        }
        public async Task TrackParcelResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            var optionSelected = await result;
            context.Done(trackingNo);
        }
    }
}