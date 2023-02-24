using MarketplaceOnline2023.AlfaPeople.MVC.Models;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceOnline2023.AlfaPeople.MVC.Controllers
{
	public class AccountController
	{
		public IOrganizationService ServiceClient { get; set; }
		public Account Account { get; set; }

		public AccountController(IOrganizationService serviceClient)
		{
			this.ServiceClient = serviceClient;
			this.Account = new Account(this.ServiceClient, "account");
		}

		public Entity GetAccountById(Guid accountId)
		{
			return Account.GetAccountById(accountId, new string[] { "mkt_valortotaloportunidades" });
		}

		public void ChangeTotalOpportunities(Entity oppAccount, bool incrementOrDecrement)
		{
			Account.ChangeTotalOpportunities(oppAccount, incrementOrDecrement);
		}
	}
}
