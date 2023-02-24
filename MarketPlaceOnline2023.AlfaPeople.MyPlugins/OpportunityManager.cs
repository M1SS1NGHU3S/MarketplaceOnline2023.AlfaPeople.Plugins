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

			ExecuteAccountOpportunitiesUpdate(context);
			TracingService.Trace("Update concluído");

		}

		private void ExecuteAccountOpportunitiesUpdate(IPluginExecutionContext context)
		{
			Entity oppPreImage = (Entity)context.PreEntityImages["PreImage"];
			TracingService.Trace("PreImage salva");

			EntityReference preAccountReference = oppPreImage.Contains("parentaccountid") ? (EntityReference)oppPreImage["parentaccountid"] : null;

			if (preAccountReference != null)
			{
				TracingService.Trace("preAccountReference salva");

				UpdateAccountTotalOpportunities(preAccountReference, false);
				TracingService.Trace("Pre account total opportunities update conclúido");

				if (context.MessageName == "Update")
				{
					Entity oppPostImage = (Entity)context.PostEntityImages["PostImage"];
					TracingService.Trace("PostImage Update salva");

					EntityReference postAccountReference = (EntityReference)oppPostImage["parentaccountid"];
					TracingService.Trace("postAccountReference salva");

					UpdateAccountTotalOpportunities(postAccountReference, true);
				}
			}
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
