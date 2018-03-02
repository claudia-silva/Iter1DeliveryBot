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
    public class DeliveryDialog : IDialog<string>
    {
        public Task StartAsync(IDialogContext context)
        {
            PromptDialog.Choice(context, this.DeliveryDialogResumeAfter, new List<string>() { "Track a Parcel", "Re-arrange Delivery Date or Time", "Re-arrange Delivery Address", "Collect Parcel from a Local Service Point" }, "Please select an option?");
            return Task.CompletedTask;
        }

        public async Task DeliveryDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            var optionSelected = await result;

            context.Call(new TrackingNoDialog(optionSelected), TrackNoResumeAfter);

            //switch (optionSelected)
            //{
            //    case "Track a Parcel":
            //        break;
            //    case "Re-arrange Delivery Date or Time":
            //        //PromptDialog.Text(context, DeliveryDialogResumeAfter, "Please enter your Tracking No?");
            //        break;
            //    case "Change Delivery Address":
            //        //PromptDialog.Text(context, DeliveryDialogResumeAfter, "Please enter your Tracking No?");
            //        break;
            //    case "Collect Parcel from a Local Service Point":                 
            //        //PromptDialog.Text(context, DeliveryDialogResumeAfter, "Please enter your Tracking No?");
            //        break;
            //}
        }

        public async Task TrackNoResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            var optionSelected = await result;
            context.Done(optionSelected);
        }

    }
}
