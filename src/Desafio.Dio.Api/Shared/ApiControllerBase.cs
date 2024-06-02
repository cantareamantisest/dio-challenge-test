using Microsoft.AspNetCore.Mvc;

namespace Desafio.Dio.Api.Shared
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ApiControllerBase : ControllerBase
    {
    }
}
