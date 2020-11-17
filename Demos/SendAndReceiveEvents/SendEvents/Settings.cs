namespace SendEvents
{

	public static class Settings
	{
		public const string ehubNamespaceConnectionString = "Endpoint=sb://dallasaustin.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=0WTvZX+/W4sbfjSN+Xb5xTCnsuxpq2/RIHtQ9bjUkIY=";
		public const string eventHubName = "SendAndReceiveEvents";
		public const string blobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=dallasaustin;AccountKey=KrBvv1/Ykxoa+BNPfCvVXZG5UnCcH/SROwX7J1OWAnr4XEJDHLcD6N5+QgArOzrXjUrLqqAxqV4FHCAsBjUSKw==;EndpointSuffix=core.windows.net";
		public const string blobContainerName = "receiveevents";
	}

}