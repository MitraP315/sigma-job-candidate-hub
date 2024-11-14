namespace JobHubAPI.Crud
{
    public class ResponseModel
    {
        private int v;
        private string error;
        public ResponseModel()
        {
        }
        public ResponseModel(int v, string error)
        {
            this.v = v;
            this.error = error;
        }

        public ResponseModel(int code, string msg, object data)
        {
            Code = code;
            Message = msg;
            Data = data;
        }
        public int Code { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
