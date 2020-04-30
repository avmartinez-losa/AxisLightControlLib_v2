using System.Net;


namespace AxisLightControlLib_v2.Identification
{
    public class ParamResponse
    {
        public bool IsSuccess { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public string Content { get; set; }
    }
}
