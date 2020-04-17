using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Collections;

namespace Service
{
    public sealed class Example : IBackgroundTask {
        private BackgroundTaskDeferral backgroundTaskDeferral;
        private AppServiceConnection appServiceconnection;

        public void Run(IBackgroundTaskInstance taskInstance) {
            backgroundTaskDeferral = taskInstance.GetDeferral();

            taskInstance.Canceled += OnTaskCanceled;

            var details = taskInstance.TriggerDetails as AppServiceTriggerDetails;
            appServiceconnection = details.AppServiceConnection;
            appServiceconnection.RequestReceived += OnRequestReceived;
        }

        private async void OnRequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args) {
            var messageDeferral = args.GetDeferral();
            var response = new ValueSet();
            response.Add("Response", "OK");

            try {
                await args.Request.SendResponseAsync(response);
            }
            catch (Exception) {
            }
            finally {
                messageDeferral.Complete();
            }
        }

        private void OnTaskCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason) {
            if (backgroundTaskDeferral != null) {
                backgroundTaskDeferral.Complete();
            }
        }
    }
}
