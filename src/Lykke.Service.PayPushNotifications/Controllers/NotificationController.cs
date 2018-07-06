using AutoMapper;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Service.PayPushNotifications.Core.Services;
using Lykke.Service.PayPushNotifications.Filters;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;

namespace Lykke.Service.PayPushNotifications.Controllers
{
    [ValidateActionParametersFilter]
    [Route("api/[controller]/[action]")]
    public class NotificationController: Controller
    {
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;

        public NotificationController(INotificationService notificationService, IMapper mapper)
        {
            _notificationService = notificationService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get or create notification identities for mobile clients.
        /// </summary>
        /// <param name="employeeEmail">Email of the employee.</param>
        /// <param name="merchantId">Identy of the merchant.</param>
        /// <returns code="200">Notification identities.</returns>
        /// <returns code="400">Input arguments are invalid.</returns>
        [HttpGet]
        [SwaggerOperation("GetNotificationIds")]
        [ProducesResponseType(typeof(Dictionary<string, string[]>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetNotificationIds(string employeeEmail, string merchantId)
        {
            var results = await _notificationService.GetNotificationIdsAsync(employeeEmail, merchantId);
            var models = _mapper.Map<Dictionary<string, string[]>>(results);
            return Ok(models);
        }
    }
}
