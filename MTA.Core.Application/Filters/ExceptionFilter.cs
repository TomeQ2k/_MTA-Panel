using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Logic.Responses;
using MTA.Core.Application.Models;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data;
using Serilog;

namespace MTA.Core.Application.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            var errorMessage = context.Exception.Message;
            var (statusCode, errorCode) = (HttpStatusCode.InternalServerError, ErrorCodes.ServerError);

            (statusCode, errorCode) = context.Exception switch
            {
                ServerException _ or CrudException _ or DatabaseException _ or ResetPasswordException _ or
                    TokenExpiredException _ or CannotGenerateTokenException _ or ProfileUpdateException _ or
                    ChangePasswordException _ or UploadFileException _ or DeleteFileException _ or
                    PremiumOperationException _ => (
                        HttpStatusCode.InternalServerError, GetErrorCode(context.Exception)),

                EntityNotFoundException _ => (HttpStatusCode.NotFound, GetErrorCode(context.Exception)),

                AuthException _ or InvalidCredentialsException _ or
                    AccountNotConfirmedException _ => (HttpStatusCode.Unauthorized, GetErrorCode(context.Exception)),

                ServiceException _ or CaptchaException _ => (HttpStatusCode.ServiceUnavailable,
                    GetErrorCode(context.Exception)),

                NoPermissionsException _ or BlockException _ => (HttpStatusCode.Forbidden,
                    GetErrorCode(context.Exception)),

                DuplicateException _ => (HttpStatusCode.Conflict, GetErrorCode(context.Exception)),

                PaymentException _ => (HttpStatusCode.PaymentRequired, GetErrorCode(context.Exception)),

                OldPasswordInvalidException _ => (HttpStatusCode.BadRequest, GetErrorCode(context.Exception)),

                _ => (HttpStatusCode.InternalServerError, ErrorCodes.ServerError)
            };

            var jsonResponse =
                (new BaseResponse(Error.Build(errorCode, errorMessage, statusCode: statusCode))).ToJSON();

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int) statusCode;
            context.HttpContext.Response.ContentLength = Encoding.UTF8.GetBytes(jsonResponse).Length;

            context.HttpContext.Response.AddApplicationError(errorMessage);

            await context.HttpContext.Response.WriteAsync(jsonResponse);

            var databaseRestorer = context.HttpContext.RequestServices.GetRequiredService<IDatabaseRestorer>();
            databaseRestorer.EnqueueFromConnectionDatabaseRestorePoints(context.HttpContext.GetConnectionId());

            Log.Error($"{context.Exception.GetType().Name}: {errorMessage} [{errorCode}] [HTTP {(int) statusCode}]");

            await base.OnExceptionAsync(context);
        }

        #region private

        private string GetErrorCode(Exception exception) =>
            (string) exception.GetType().GetProperty("ErrorCode").GetValue(exception, null);

        #endregion
    }
}