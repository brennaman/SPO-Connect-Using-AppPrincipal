using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPOAppPrincipal.Console
{
    class Program
    {
        static void Main(string[] args)
        {

            string spHostUrl = ConfigurationManager.AppSettings["SPHostUrl"];
            Uri site = new Uri(spHostUrl);
            string realm = TokenHelper.GetRealmFromTargetUrl(site);

            var tokenResponse = TokenHelper.GetAppOnlyAccessToken(TokenHelper.SharePointPrincipal,
                site.Authority,
                realm);

            using (ClientContext context = TokenHelper.GetClientContextWithAccessToken(spHostUrl, tokenResponse.AccessToken))
            {

                context.RequestTimeout = Timeout.Infinite;

                var web = context.Web;
                context.Load(web);
                context.ExecuteQueryRetry();

                System.Console.WriteLine("Web URL: " + web.Url);

                //get a list of Lists in the web
                context.Load(web.Lists);
                context.ExecuteQueryRetry();

                System.Console.WriteLine("Lists:");
                foreach (var list in web.Lists)
                {
                    System.Console.WriteLine(list.Title);
                }

                System.Console.WriteLine("Finished. Press any key to exit.");
                System.Console.ReadKey();
                

            }

            

        }
    }
}
