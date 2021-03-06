﻿#if NETSTANDARD2_0        
using ServiceStack.Host;
#else
using System.Web;
#endif
using ServiceStack.Host.Handlers;

namespace ServiceStack
{
    public class RequestInfoFeature : IPlugin, Model.IHasStringId
    {
        public string Id { get; set; } = Plugins.RequestInfo;
        public void Register(IAppHost appHost)
        {
            appHost.CatchAllHandlers.Add(ProcessRequest);

            appHost.GetPlugin<MetadataFeature>()
                .AddDebugLink($"?{Keywords.Debug}={Keywords.RequestInfo}", "Request Info");
        }

        public IHttpHandler ProcessRequest(string httpMethod, string pathInfo, string filePath)
        {
            var pathParts = pathInfo.TrimStart('/').Split('/');
            return pathParts.Length == 0 ? null : GetHandlerForPathParts(pathParts);
        }

        private static IHttpHandler GetHandlerForPathParts(string[] pathParts)
        {
            var pathController = pathParts[0].ToLower();
            return pathController == Keywords.RequestInfo
                ? new RequestInfoHandler()
                : null;
        }
    }
}