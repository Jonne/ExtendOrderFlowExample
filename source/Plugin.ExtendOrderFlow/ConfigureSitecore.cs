// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigureSitecore.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2017
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Plugin.ExtendOrderFlow
{
    using System.Reflection;

    using Microsoft.Extensions.DependencyInjection;
    using Plugin.ExtendOrderFlow.Pipelines;
    using Plugin.ExtendOrderFlow.Pipelines.Blocks;
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.Plugin.Orders;
    using Sitecore.Framework.Configuration;
    using Sitecore.Framework.Pipelines.Definitions.Extensions;

    /// <summary>
    /// The Habitat configure class.
    /// </summary>
    /// <seealso cref="IConfigureSitecore" />
    public class ConfigureSitecore : IConfigureSitecore
    {
        /// <summary>
        /// The configure services.
        /// </summary>
        /// <param name="services">
        /// The services.
        /// </param>
        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllPipelineBlocks(assembly);

            services.Sitecore().Pipelines(
                config =>
                    config
                        .ConfigurePipeline<IRunningPluginsPipeline>(c =>
                        {
                            c.Add<RegisteredPluginBlock>().After<RunningPluginsBlock>();
                        })
                        .ConfigurePipeline<IPendingOrdersMinionPipeline>(c =>
                        {
                            c.Replace<Sitecore.Commerce.Plugin.Orders.ReleaseOrderBlock, MoveOrderToWaitingForPayment>();
                        })
                        .AddPipeline<IWaitingForPaymentOrderMinionPipeline, WaitingForPaymentOrderMinionPipeline>(c =>
                        {
                            c.Add<Pipelines.Blocks.ReleaseOrderBlock>();
                        }));
        }
    }
}