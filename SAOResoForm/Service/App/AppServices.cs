using SAOResoForm.Service.Repository;
using SAOResoForm.Service.Repository.tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAOResoForm.Service.App
{
	public class AppServices
	{
		public IRepositoryService RepositoryService { get; }
		public ITool Tool { get; }

		public AppServices(
			IRepositoryService repositoryService,
			ITool tool)
		{
			RepositoryService = repositoryService;
			Tool = tool;
		}
	}
}