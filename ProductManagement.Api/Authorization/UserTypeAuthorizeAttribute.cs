using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Security.Claims;

namespace Management.Api.Authorization
{
    public class UserTypeAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly UserType[] _allowedUserTypes;

        // Birden fazla UserType parametresi alabilmesi için params kullanıldı.
        public UserTypeAuthorizeAttribute(params UserType[] allowedUserTypes)
        {
            _allowedUserTypes = allowedUserTypes;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userTypeClaim = context.HttpContext.User.FindFirst("UserType")?.Value;

            if (userTypeClaim == null || !Enum.TryParse(userTypeClaim, out UserType userType))
            {
                context.Result = new ForbidResult();
                return;
            }

            // SuperAdmin için tüm Admin yetkilerini genişletildi
            if (userType == UserType.Superadmin)
            {
                // SuperAdmin tüm erişim haklarına sahipse yetkilendirme başarılıdır.
                return;
            }

            if (Array.IndexOf(_allowedUserTypes, userType) < 0)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
