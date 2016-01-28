namespace Crosschat.Server.Application.DataTransferObjects.Requests
{
    public class ResponseBase
    {
        public ResponseBase()
        {
            Error = CommonErrors.Success;
        }

        //In order to associate Request with response
        public long Token { get; set; }

        //false means timeout or connection close
        public bool RequestResult { get { return Error == CommonErrors.Success; }}

        public CommonErrors Error { get; set; }
    }

    public enum CommonErrors
    {
        //negative values are not errors ;)
        Success = 0,
        Timeout,
        AuthenticationFailure,
        Maintenance,
        Banned,
    }
}