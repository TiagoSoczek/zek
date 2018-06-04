using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Zek.Shared.Api.Dtos;

namespace Zek.Shared.Api
{
    public abstract class BaseController : Controller
    {
        protected BaseController(IMediator mediator, IMapper mapper)
        {
            Mediator = mediator;
            Mapper = mapper;
        }

        protected IMediator Mediator { get; }
        protected IMapper Mapper { get; }

        protected T MapTo<T>(object source) => Mapper.Map<T>(source);

        protected ActionResult<T> As<T>(object source)
        {
            if (source is IResult result)
            {
                return AsResult<T>(result);
            }

            return Ok(MapTo<T>(source));
        }

        protected ActionResult<T> AsResult<T>(IResult result)
        {
            if (result.IsSuccess)
            {
                if (result.HasValue)
                {
                    return Ok(MapTo<T>(result.GetObjectValue()));
                }

                return Ok();
            }

            return HandleFailure(result);
        }

        protected IActionResult AsResult(IResult result)
        {
            return result.IsSuccess ? Ok() : HandleFailure(result);
        }

        private ActionResult HandleFailure(IResult result)
        {
            var errorResultDto = new ErrorResultDto
            {
                Error = result.Error
            };

            switch (result.Code)
            {
                case ResultCode.NotFound:
                    return NotFound(errorResultDto);
                default:
                    return BadRequest(errorResultDto);
            }
        }
    }
}