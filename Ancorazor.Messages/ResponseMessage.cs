#region

#endregion

namespace Ancorazor.API.Messages
{
    public class ResponseMessage<TData>
    {
        public TData Data { get; set; }
        public string ErrorCode { get; set; } = "0000";
        public string Message { get; set; }
        public bool Succeed { get; set; } = true;
    }
}