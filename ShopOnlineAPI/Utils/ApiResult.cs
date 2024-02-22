using System.Text.Json.Serialization;

namespace ShopOnlineAPI.Utils
{
    public class ApiResult
    {
        public string status { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object error_message { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string code_error { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object data { get; set; }

        public static ApiResult Success(object data = null)
        {
            return new ApiResult
            {
                status = Status.Success,
                data = data
            };
        }

        public static ApiResult Error(string code_error, object message)
        {
            return new ApiResult
            {
                status = Status.Error,
                code_error = code_error,
                error_message = message
            };
        }

        public static ApiResult FatalError()
        {
            return new ApiResult
            {
                status = Status.Error,
                code_error = "fatal_error",
                error_message = "An unexpected error occurred"
            };
        }

        public static ApiResult NotFound(string message = null)
        {
            return new ApiResult
            {
                status = Status.NotFound,
                error_message = message
            };
        }
        public static ApiResult BadRequest(string message = null)
        {
            return new ApiResult
            {
                status = Status.BadRequest,
                error_message = message
            };
        }

        public static class Status
        {
            public const string Success = "success";
            public const string Error = "error";
            public const string BadRequest = "bad_request";
            public const string NotFound = "not_found";
        }
    }
}
