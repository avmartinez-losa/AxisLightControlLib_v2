using System;
using System.Threading.Tasks;

namespace AxisLightControlLib_v2.Identification
{
    public class ParamConf
    {
        public async Task<string> SetIRCutFilterON(string _ip, string _user, string _password)
        {
            
            var action = "?action=update&ImageSource.I0.DayNight.IrCutFilter=yes";
            var request = new ParamRequest();
            var response = "";
            try
            {
                var responseServer = await request.ApiCall(_ip, _user, _password, action);

                if (responseServer.Content.Contains("OK"))
                    response = responseServer.Content;
                else if ((int)responseServer.HttpStatusCode != 200)
                    response = (int)responseServer.HttpStatusCode + responseServer.Content;

            }
            catch (Exception ex)
            {
                response = ex.Message;
            }
            return response;

        }
        public async Task<string> SetIRCutFilterOFF(string _ip, string _user, string _password)
        {

            var action = "?action=update&ImageSource.I0.DayNight.IrCutFilter=no";
            var request = new ParamRequest();
            var response = "";
            try
            {
                var responseServer = await request.ApiCall(_ip, _user, _password, action);

                if (responseServer.Content.Contains("OK"))
                    response = responseServer.Content;
                else if ((int)responseServer.HttpStatusCode != 200)
                    response = (int)responseServer.HttpStatusCode + responseServer.Content;

            }
            catch (Exception ex)
            {
                response = ex.Message;
            }
            return response;

        }
        public async Task<string> SetIRCutFilterAuto(string _ip, string _user, string _password)
        {

            var action = "?action=update&ImageSource.I0.DayNight.IrCutFilter=auto";
            var request = new ParamRequest();
            var response = "";

            try
            {
                var responseServer = await request.ApiCall(_ip, _user, _password, action);

                if (responseServer.Content.Contains("OK"))
                    response = responseServer.Content;
                else if ((int)responseServer.HttpStatusCode != 200)
                    response = (int)responseServer.HttpStatusCode + responseServer.Content;

            }
            catch (Exception ex)
            {
                response = ex.Message;
            }
            return response;

        }
        public async Task<bool> CheckHTTPVersion(string _ip, string _user, string _password)
        {
            var action = "?action=list&group=Properties.API.HTTP.Version";                       
            var requestVersion = new ParamRequest();
            int actionPropertyVersionResponse = 0;

            try
            {
                var responseVersion = await requestVersion.ApiCall(_ip, _user, _password, action);
                Int32.TryParse(responseVersion.Content, out actionPropertyVersionResponse);
                
                if (actionPropertyVersionResponse == 3)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> IdentifyLightControlv2(string _ip, string _user, string _password)
        {
            
            var action = "?action=list&group=Properties.LightControl.LightControl2";
            var requestVersion = new ParamRequest();

            var responseLightVersion = await requestVersion.ApiCall(_ip, _user, _password, action);

            if (responseLightVersion.Content == "yes")
                return true;
             else
                return false;
           
        }
        public Task<bool> IdentifyLightControlv1(string _ip, string _user, string _password)
        {
            return null;
        }
    }
}
