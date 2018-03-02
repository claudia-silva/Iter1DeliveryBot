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

        public ReArrangeDialog(string trackingNo)
        {
            this.trackingNo = trackingNo;
        }
        public Task StartAsync(IDialogContext context)
        {
            PromptDialog.Choice(context, this.ReArrangeResumeAfter, new List<string>() { "Re-Arrange Date", "Re-Arrange Time" }, "Please select an option?");
            return Task.CompletedTask;
        }

        public async Task ReArrangeResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            var optionSelected = await result;

            //switch (optionSelected)
            //{
            //    case "Date":
            //        context.Call(new ReArrangeDialog(), ReArrangeDate);
            //        break;
            //    case "Time":
            //        context.Call(new ReArrangeDialog(), ReArrangeTime);
            //        break;
            //}
        }

        //public async Task ReArrangeDate(IDialogContext context, IAwaitable<string> result)
        //{
        //    PromptDialog.Text(context, NextSteps, $@"Your parcel with Track No: {this.trackingNo} is currently with your local Hermes and will be delivered to you today by 4pm.");
        //}


        //public async Task ReArrangeTime(IDialogContext context, IAwaitable<string> result)
        //{

        //}
    }
}