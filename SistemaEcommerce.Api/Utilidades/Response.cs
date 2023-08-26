namespace SistemaEcommerce.Api.Utilidades
{
    public class Response<T>
    {
        public bool status { get; set; }
        public T data { get; set; }
        public string msg { get; set; }
    }

}
