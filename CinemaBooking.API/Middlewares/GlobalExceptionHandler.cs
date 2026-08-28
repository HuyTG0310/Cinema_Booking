using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBooking.API.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            // Bắt lỗi ValidationException từ FluentValidation
            if (exception is FluentValidation.ValidationException validationException)
            {
                // Gom nhóm các lỗi theo tên property
                var errors = validationException.Errors
                    .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                    .ToDictionary(g => g.Key, g => g.ToArray());

                // Sử dụng HttpValidationProblemDetails chuẩn của .NET
                var validationProblem = new HttpValidationProblemDetails(errors)
                {
                    Status = StatusCodes.Status400BadRequest,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "Dữ liệu đầu vào không hợp lệ",
                    Detail = "Vui lòng kiểm tra lại các trường dữ liệu."
                };

                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsJsonAsync(validationProblem, cancellationToken);
                return true;
            }

            // Bắt các lỗi hệ thống khác (Code 500)
            var genericProblem = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Title = "Lỗi máy chủ nội bộ",
                Detail = exception.Message // Chú ý: Ở môi trường Production thực tế, không nên trả về trực tiếp Message để tránh lộ logic
            };

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(genericProblem, cancellationToken);

            return true; // Trả về true để báo hiệu lỗi đã được xử lý
        }
    }
}
