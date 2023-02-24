using MarketplaceOnline2023.AlfaPeople.MVC.Controllers;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceOnline2023.AlfaPeople.MyPlugins
{
    public class OpportunityManager : IPlugin
    {
        public IOrganizationService Service { get; set; }
        public ITracingService TracingService { get; set; }

        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            TracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            Service = serviceFactory.CreateOrganizationService(context.UserId);

            TracingService.Trace("Serviços funcionando");

            Entity oppPreImage = (Entity)context.PreEntityImages["PreImageDelete"];
            TracingService.Trace("PreImage salva");

            EntityReference preAccountReference = (EntityReference)oppPreImage["parentaccountid"];
            TracingService.Trace("preAccountReference salva");

            UpdateAccountTotalOpportunities(preAccountReference, false);
        }

        public void UpdateAccountTotalOpportunities(EntityReference account, bool incrementOrDecrement)
        {
			AccountController accountController = new AccountController(Service);

            Entity opportunityAcc = accountController.GetAccountById(account.Id);
            TracingService.Trace($"Opportunity account salva: {opportunityAcc.Id}");

            accountController.ChangeTotalOpportunities(opportunityAcc, incrementOrDecrement);
            TracingService.Trace("Número de oportunidades alterada");
		}
    }
}
