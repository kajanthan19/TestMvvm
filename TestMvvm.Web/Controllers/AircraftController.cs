using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TestMvvm.Core.Exceptions;
using TestMvvm.Core.Handlers.Commands;
using TestMvvm.Core.Handlers.Queries;
using TestMvvm.Core.Responses;
using TestMvvm.Domain.Dtos;

namespace TestMvvm.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AircraftController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AircraftController> _logger;
        public AircraftController(ILogger<AircraftController> logger, IMediator mediator)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AircraftDto>), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(BaseCommandResponse))]
        public async Task<IActionResult> Get()
        {
            try
            {
                var query = new GetAllAircraftsQuery();
                var response = await _mediator.Send(query);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Catch Exception error {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(AircraftDto), (int)HttpStatusCode.Created)]
        [ProducesErrorResponseType(typeof(BaseCommandResponse))]
        public async Task<IActionResult> Post(IFormCollection formdata)
        {
            try
            {
                // File Upload
                var files = HttpContext.Request.Form.Files;
                string? dbfilepath = null;
                if (files.Count > 0)
                {
                    foreach (var file in files)
                     {
                        var filecommand = new UploadFileCommand(file);
                        dbfilepath = await _mediator.Send(filecommand);                      
                     }
                }

                var datetime = DateTimeOffset.Parse(formdata["AircraftSeen"], null);
                var model = new CreateAircraftDto()
                {
                    Registration = formdata["Registration"],
                    Location = formdata["Location"],
                    Make = formdata["Make"],
                    Model = formdata["Model"],
                    AircraftSeen = datetime.DateTime,
                    ImageUrl = dbfilepath
                };

                var command = new CreateAircraftCommand(model);
                var response = await _mediator.Send(command);
                return StatusCode((int)HttpStatusCode.Created, response);
            }
            catch (InvalidRequestBodyException ex)
            {
                _logger.LogError($"Catch Exception error {ex.Message}");
                return BadRequest(new BaseCommandResponse
                {
                    Success = false,
                    Errors = ex.Errors.ToList()
                });
            }
        }


        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(AircraftDto), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(BaseCommandResponse))]
        public async Task<IActionResult> Update(int id, IFormCollection formdata)
        {
            try
            {
                // File Upload
                var files = HttpContext.Request.Form.Files;
                string? dbfilepath = null;
                if (files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        var filecommand = new UploadFileCommand(file);
                        dbfilepath = await _mediator.Send(filecommand);
                    }
                }

                var datetime = Convert.ToDateTime(formdata["AircraftSeen"]);
                var model = new CreateAircraftDto()
                {
                    Registration = formdata["Registration"],
                    Location = formdata["Location"],
                    Make = formdata["Make"],
                    Model = formdata["Model"],
                    AircraftSeen = datetime,
                    ImageUrl = dbfilepath
                };

                var command = new UpdateAircraftCommand(id, model);
                var response = await _mediator.Send(command);
                return Ok(response);
            }
            catch (InvalidRequestBodyException ex)
            {
                _logger.LogError($"Update Catch Exception error {ex.Message}");
                return BadRequest(new BaseCommandResponse
                {
                    Success = false,
                    Errors = ex.Errors.ToList()
                });
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(BaseCommandResponse))]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var command = new DeleteAircraftCommand(id);
                await _mediator.Send(command);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Delete Catch Exception error {ex.Message}");
                return BadRequest(new BaseCommandResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
    }
}
