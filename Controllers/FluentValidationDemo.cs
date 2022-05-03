using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestWebApiProject.DTO;
using TestWebApiProject.Validator;

namespace TestWebApiProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FluentValidationDemoController : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> PostMethod(Customer customer)
        {
            try
            {
                await new CustomerValidator().ValidateAndThrowAsync(customer);

                return Ok();
            }
            catch (ValidationException vex)
            {
                var response = new
                {
                    Msg = "Validation failed.",
                    Errorlst = vex.Errors.Select(e => 
                    new ErrorMessage 
                    { 
                        Error = e.ErrorMessage, 
                        Value = e.AttemptedValue?.ToString() 
                    }).ToList()
                };
                return BadRequest(response);
            }
            catch (Exception ex)
            {

                var response = new
                {
                    Msg = "Processing request failed.",
                    Errorlst = new List<ErrorMessage>() { 
                        new ErrorMessage() { Error = ex.Message } }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}