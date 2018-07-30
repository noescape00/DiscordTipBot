using System;
using NLog;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;

namespace TipBot.Helpers
{
	public class FullNodeApiClient
	{
		private readonly Settings settings;

		private const string AccountName = "account 0";

		private readonly Logger logger;

		public FullNodeApiClient(Settings settings)
		{
			this.settings = settings;
			this.logger = LogManager.GetCurrentClassLogger();
		}

		public string callApi(string httpMethod, string apiMethod, Dictionary<string, object> jsonParams)
		{
			if (!settings.FullNodeApiEnabled)
			{
				throw new Exception();
			}
			var apiUrl = settings.FullNodeApiUrl + apiMethod;
			string result = null;
			using (var client = new HttpClient())
			{
				HttpResponseMessage response = client.GetAsync(apiUrl).Result;
				if (httpMethod == "GET")
				{
					response = client.GetAsync(apiUrl).Result;
					if (response.IsSuccessStatusCode)
					{
						var responseContent = response.Content;
					    result = responseContent.ReadAsStringAsync().Result;
					}
				}
				if (httpMethod == "POST")
				{

					var json = JsonConvert.SerializeObject(jsonParams);
					var data = new StringContent(json, Encoding.UTF8, "application/json");

					response = client.PostAsync(apiUrl, data).Result;
					if (response.IsSuccessStatusCode)
					{
						var responseContent = response.Content;
						result = responseContent.ReadAsStringAsync().Result;
					}
				}
			}
			return result;

		}

		public string getBlockCount()
		{
			var blockCount = callApi("GET", "BlockStore/getblockcount", null);
			return blockCount;
		}

		public string inspectSmartContractState(string contractAddress, string variableName, string variableType)
		{
			var apiUrl = $"SmartContracts/storage?ContractAddress={contractAddress}&StorageKey={variableName}&DataType={variableType}";
			logger.Info(apiUrl);
			var variableContents = callApi("GET", apiUrl, null);
			logger.Info(variableContents);
			return variableContents;
		}

		public string callSmartContractMethod(Dictionary<string, object>  jsonParams)
		{

			var apiUrl = $"/SmartContracts/build-and-send-call";
			logger.Info(apiUrl);
			var variableContents = callApi("POST", apiUrl, null);
			logger.Info(variableContents);
			return variableContents;
		}
	}
}
