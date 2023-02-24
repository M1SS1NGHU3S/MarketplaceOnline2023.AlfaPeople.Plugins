using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceOnline2023.AlfaPeople.MVC.Models
{
	public class Account
	{
		public IOrganizationService Service { get; set; }
		public string LogicalName { get; set; }

		public Account(IOrganizationService serviceClient, string logicalName)
		{
			this.Service = serviceClient;
			this.LogicalName = logicalName;
		}

		public Entity GetAccountById(Guid accountId, string[] columns)
		{
			return Service.Retrieve(this.LogicalName, accountId, new ColumnSet(columns));
		}

		public void ChangeTotalOpportunities(Entity oppAccount, bool incrementOrDecrement)
		{
			int opportunityNum = oppAccount.Contains("mkt_valortotaloportunidades") ? (int)oppAccount["mkt_valortotaloportunidades"] : 0;

			if (incrementOrDecrement) { opportunityNum++; } 
			else { opportunityNum--; }

			oppAccount["mkt_valortotaloportunidades"] = opportunityNum;
			Service.Update(oppAccount);
		}
	}
}
