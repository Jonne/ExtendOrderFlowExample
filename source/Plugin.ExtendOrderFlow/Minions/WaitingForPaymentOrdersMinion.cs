using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Plugin.ExtendOrderFlow.Pipelines;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Orders;
using System;
using System.Threading.Tasks;

namespace Plugin.ExtendOrderFlow.Minions
{
    public class WaitingForPaymentOrdersMinion : Minion
    {
        protected IWaitingForPaymentOrderMinionPipeline MinionPipeline { get; set; }

        public override void Initialize(IServiceProvider serviceProvider, ILogger logger, MinionPolicy policy, CommerceEnvironment environment, CommerceContext globalContext)
        {
            base.Initialize(serviceProvider, logger, policy, environment, globalContext);
            this.MinionPipeline = serviceProvider.GetService<IWaitingForPaymentOrderMinionPipeline>();
        }

        [Obsolete("This method is deprecated, use Execute instead.")]
        public override Task<MinionRunResultsModel> Run()
        {
            throw new NotImplementedException();
        }

        protected override async Task<MinionRunResultsModel> Execute()
        {
            int itemsProcessed = 0;

            var orders = await GetListItems<Order>(Policy.ListToWatch, Policy.ItemsPerBatch).ConfigureAwait(false);

            foreach (Order order in orders)
            {
                CommerceContext commerceContext = new CommerceContext(Logger, MinionContext.TelemetryClient, (IGetLocalizableMessagePipeline)null)
                {
                    Environment = Environment
                };

                CommercePipelineExecutionContextOptions executionContextOptions = new CommercePipelineExecutionContextOptions(commerceContext);
                
                await MinionPipeline.Run(order, executionContextOptions);

                itemsProcessed++;
            }

            return new MinionRunResultsModel
            {
                DidRun = true,
                ItemsProcessed = itemsProcessed
            };
        }
    }
}
