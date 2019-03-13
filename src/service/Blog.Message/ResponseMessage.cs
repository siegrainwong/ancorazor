#region

#endregion

namespace Blog.Message
{
    public class ResponseMessage<TData>
    {
        public bool Succeed { get; set; } = true;
        public string ErrorCode { get; set; } = "0000";
        public string Message { get; set; }
        public TData Data { get; set; }
    }
}