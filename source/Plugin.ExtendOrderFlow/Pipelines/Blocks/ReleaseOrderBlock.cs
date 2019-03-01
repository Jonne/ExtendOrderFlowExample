using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.ManagedLists;
using Sitecore.Commerce.Plugin.Orders;
using Sitecore.Framework.Eventing;
using Sitecore.Framework.Pipelines;

using System.Threading.Tasks;

namespace Plugin.ExtendOrderFlow.Pipelines.Blocks
{
    [PipelineDisplayName("ExtendOrderFlow.block.ReleaseOrderBlock")]
    public class ReleaseOrderBlock : PipelineBlock<Order, Order, CommercePipelineExecutionContext>
    {
        private readonly IPersistEntityPipeline persistEntityPipeline;
        private readonly IEventRegistry eventRegistry;
        private readonly IRemoveListEntitiesPipeline removeListEntitiesPipeline;

        public ReleaseOrderBlock(IPersistEntityPipeline persistEntityPipeline, IEventRegistry eventRegistry, IRemoveListEntitiesPipeline removeListEntitiesPipeline)
            : base(null)
        {
            this.persistEntityPipeline = persistEntityPipeline;
            this.eventRegistry = eventRegistry;
            this.removeListEntitiesPipeline = removeListEntitiesPipeline;
        }


        public override async Task<Order> Run(Order arg, CommercePipelineExecutionContext context)
        {
            KnownOrderStatusPolicy policy = context.GetPolicy<KnownOrderStatusPolicy>();

            arg.Status = policy.Released;

            KnownOrderListsPolicy listPolicy = context.GetPolicy<KnownOrderListsPolicy>();
            arg.GetComponent<TransientListMembershipsComponent>().Memberships.Add(listPolicy.ReleasedOrders);

            await removeListEntitiesPipeline.Run(new ListEntitiesArgument(new string[1]
            {
                arg.Id
            }, "WaitingForPaymentOrders"), context);

            await persistEntityPipeline.Run(new PersistEntityArgument(arg), context).ConfigureAwait(false);
                        
            await eventRegistry.ListItemUpdated().Send(arg, Name);

            return arg;
        }
    }
}
