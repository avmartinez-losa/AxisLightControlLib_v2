using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AxisLightControlLib_v2.Entry
{
    public class EntryServiceRequest
    {
        public string _message = "<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:ent=\"http://www.axis.com/vapix/ws/entry\">"
                                  + "<soap:Header/>"
                                      + @"<soap:Body>{0}</soap:Body>"
                                  + @"</soap:Envelope>";

        private string _service_url = "http://{0}/vapix/services";
        public string Service_URL { get { return _service_url; } set { _service_url = value; } }

        private double _request_timeout = 25;
        public double Request_Timeout { get { return _request_timeout; } set { _request_timeout = value; } }


        protected async Task<EntryServiceResponse> sendRequestAsync(string IP, string User, string Password, string Action)
        {

            using (HttpClient httpClient = new HttpClient(new HttpClientHandler()
            {
                Credentials = new NetworkCredential(
                        User,
                        Password
                    ).GetCredential(new Uri(@"http://localhost"), "Digest")
            }, true))
            {
                httpClient.Timeout = TimeSpan.FromSeconds(_request_timeout);
                EntryServiceResponse serviceResponse = new EntryServiceResponse();

                try
                {
                    using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, string.Format(Service_URL, IP)) { Version = HttpVersion.Version10 })
                    {
                        request.Content = new StringContent(string.Format(_message, Action));

                        HttpResponseMessage Response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead);

                        serviceResponse.IsSuccess = Response.IsSuccessStatusCode;
                        serviceResponse.HttpStatusCode = Response.StatusCode;

                        if (serviceResponse.IsSuccess)
                            serviceResponse.SOAPContent = XElement.Parse(await Response.Content.ReadAsStringAsync());
                        else
                            serviceResponse.Content = await Response.Content.ReadAsStringAsync();
                    }
                }
                catch (System.Threading.Tasks.TaskCanceledException)
                {
                    serviceResponse.IsSuccess = false;
                    serviceResponse.Content = "[SendSOAPRequest] Request timed out";
                }
                catch (Exception ex)
                {
                    serviceResponse.IsSuccess = false;
                    serviceResponse.Content = "[SendSOAPRequest] " + ex.Message + (ex.InnerException != null ? " " + ex.InnerException.Message : "");
                }

                return serviceResponse;
            }
        }
    }
}
