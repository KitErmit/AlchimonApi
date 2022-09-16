using System;
using Microsoft.AspNetCore.Mvc;

namespace AlchimonAng.Controllers
{
    public class BaseControllerApi : ControllerBase
    {
        public BaseControllerApi()
        {
        }
        protected async Task<IActionResult> MakeResponse<T>(Task<T> action)
        {
            try
            {
                T response = await action;
                return Ok(response);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

