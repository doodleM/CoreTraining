namespace ShoppingModule.Web.Entities
{
    public class Response<T>
    {
        public bool Success { get; set; }

        public int? Code { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }
    }
}