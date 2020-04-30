using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AxisLightControlLib_v2.Identification
{
    public class ParamRequest
    {
        /// <summary>
        /// Props to hold the light service url and the request timeout
        /// </summary>
        private string _parameter_Url = "http://{0}/axis-cgi/param.cgi";
        public string Parameter_Url { get { return _parameter_Url; } set { _parameter_Url = value; } }

        private double _request_timeout = 25;
        public double Request_Timeout { get { return _request_timeout; } set { _request_timeout = value; } }

        /// <summary>
        /// Http call that uses HttpClient lib and takes camera ip, credentials and json body to post the request to the light control service
        /// </summary>
        /// <param name="_ip"></param>
        /// <param name="_user"></param>
        /// <param name="_password"></param>
        /// <param name="_action"></param>
        /// <returns></returns>
        public async Task<ParamResponse> ApiCall(string _ip, string _user, string _password, string _action)
        {
            using (HttpClient httpClient = new HttpClient(new HttpClientHandler()
            {
                Credentials = new NetworkCredential(
                            _user,
                            _password
                        ).GetCredential(new Uri(@"http://localhost"), "Digest")
            }, true))
            {
                //httpClient.DefaultRequestHeaders.Accept.Clear();
                //httpClient.DefaultRequestHeaders.Accept.Add(
                //    new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.Timeout = TimeSpan.FromSeconds(_request_timeout);

                ParamResponse serverResponse = new ParamResponse();

                try
                {
                    using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format(_parameter_Url, _ip)) { Version = HttpVersion.Version10 })
                    {
                        request.Content = new StringContent(_action, Encoding.UTF8, "text/plain");
                        HttpResponseMessage response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead);

                        serverResponse.IsSuccess = response.IsSuccessStatusCode;
                        serverResponse.HttpStatusCode = response.StatusCode;

                        if (serverResponse.IsSuccess)
                            serverResponse.Content = await response.Content.ReadAsStringAsync();
                        else
                            serverResponse.Content = await response.Content.ReadAsStringAsync();

                    }
                }
                catch (System.Threading.Tasks.TaskCanceledException)
                {
                    serverResponse.IsSuccess = false;
                    serverResponse.Content = "[SendRequest] Request timed out";
                }
                catch (Exception ex)
                {
                    serverResponse.IsSuccess = false;
                    serverResponse.Content = "[SendRequest] " + ex.Message + (ex.InnerException != null ? " " + ex.InnerException.Message : "");
                }

                return serverResponse;
            }
        }
    }
}
