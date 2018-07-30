using System;
using NLog;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;
using System.Web;

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

		private string buildUrl(string apiMethod, Dictionary<string, object> queryStringParams)
		{
			var builder = new UriBuilder(settings.FullNodeApiUrl + apiMethod);
			builder.Port = settings.FullNodeApiPort;
			if (queryStringParams != null)
			{
				var query = HttpUtility.ParseQueryString(builder.Query);
				foreach (var key in queryStringParams.Keys)
				{
					query[key] = queryStringParams[key].ToString();
				}
				builder.Query = query.ToString();
			}
			var apiUrl = builder.ToString();
			return apiUrl;
		}

		public string callApi(string httpMethod, string apiMethod, Dictionary<string, object> jsonParams)
		{
			if (!settings.FullNodeApiEnabled)
			{
				throw new Exception(); //todo, proper exception
			}

			string result = null;
			using (var client = new HttpClient())
			{
				HttpResponseMessage response = null;
				if (httpMethod == "GET")
				{
					var apiUrl = buildUrl(apiMethod, jsonParams);
					response = client.GetAsync(apiUrl).Result;
				}
				if (httpMethod == "POST")
				{
					var apiUrl = buildUrl(apiMethod, null);
					response = client.GetAsync(apiUrl).Result;

					var json = JsonConvert.SerializeObject(jsonParams);
					var data = new StringContent(json, Encoding.UTF8, "application/json");

					response = client.PostAsync(apiUrl, data).Result;
				}
				if (response.IsSuccessStatusCode)
				{
					var responseContent = response.Content;
					result = responseContent.ReadAsStringAsync().Result;
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
			var querystringParams = new Dictionary<string, object>
			{
				{"ContractAddress", contractAddress},
				{"StorageKey", variableName},
				{"DataType", variableType}
			};
			var variableContents = callApi("GET", apiUrl, querystringParams);
			logger.Info(variableContents);
			return variableContents;
		}

		public string callSmartContractMethod(string contractAddress, string methodName, uint amount, string senderAddress)
		{
			var smartContractParameters = new List<string>();

			var apiUrl = $"/SmartContracts/build-and-send-call";
			logger.Info(apiUrl);

			var jsonParams = new Dictionary<string, object>
			{
				{"walletName", settings.WalletName},
				{"accountName", FullNodeApiClient.AccountName},
				{"contractAddress", contractAddress},
				{"methodName", methodName},
				{"amount", amount},
				{"feeAmount", "10000"},
				{"password", this.settings.WalletPassword},
				{"gasPrice", 1},
				{"gasLimit", 5000000},
				{"sender", senderAddress},
				{"parameters", smartContractParameters}
			};


			var variableContents = callApi("POST", apiUrl, jsonParams);
			logger.Info(variableContents);
			return variableContents;
		}
	}
}
