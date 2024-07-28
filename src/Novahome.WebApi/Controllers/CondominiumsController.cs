using Novahome.Application.Condominiums.Create;
using Novahome.Application.Condominiums.Delete;
using Novahome.Application.Condominiums.Get;
using Novahome.Application.Condominiums.GetList;
using Novahome.Application.Condominiums.Update;

namespace Novahome.WebApi.Controllers;

/// <summary>
///   The Condominiums endpoint allows you to create, read, update and delete Condominium entities in the application.
/// </summary>
[Authorize]
[ApiController]
[Route("condominiums")]
public class CondominiumsController(ISender mediator) : ControllerBase
{
  /// <summary>
  ///   Gets all the existing Condominiums
  /// </summary>
  /// <param name="request">The filter to apply for fetching the Condominiums</param>
  /// <response code="200">The Condominiums with the specified filter</response>
  [HttpGet("")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  public async Task<ActionResult<List<CondominiumsGetListResponse>>> GetList(
    [FromQuery] CondominiumsGetListRequest request
  )
  {
    return await mediator.Send(request);
  }

  /// <summary>
  ///   Gets an existing Condominium
  /// </summary>
  /// <param name="id">The id of the Condominium to fetch</param>
  /// <response code="200">The Condominium with the specified id</response>
  /// <response code="404">A Condominium with the specified id could not be found</response>
  [HttpGet("{id:guid}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<CondominiumsGetResponse>> Get(
    [FromRoute] Guid id
  )
  {
    CondominiumsGetRequest request = new(id);

    return await mediator.Send(request);
  }

  /// <summary>
  ///   Creates a new Condominium
  /// </summary>
  /// <param name="request">The Condominium entity to create</param>
  /// <response code="200">The id of the newly created Condominium</response>
  /// <response code="400">The supplied Condominium object did not pass validation checks</response>
  [HttpPost("")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<ActionResult<CondominiumsCreateResponse>> Create(
    [FromBody] CondominiumsCreateRequest request
  )
  {
    return await mediator.Send(request);
  }

  /// <summary>
  ///   Updates an existing Condominium
  /// </summary>
  /// <param name="id">The id of the Condominium</param>
  /// <param name="request">The Condominium entity with the updates</param>
  /// <response code="204">The Condominium was correctly updated</response>
  /// <response code="400">The supplied Condominium object did not pass validation checks</response>
  /// <response code="404">A Condominium with the specified id could not be found</response>
  [HttpPut("{id:guid}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult> Update([FromRoute] Guid id, [FromBody] CondominiumsUpdateRequest request)
  {
    request.Id = id;

    await mediator.Send(request);

    return NoContent();
  }

  /// <summary>
  ///   Deletes an existing Condominium
  /// </summary>
  /// <param name="id">The id of the Condominium</param>
  /// <response code="204">The Condominium was correctly deleted</response>
  /// <response code="404">A Condominium with the specified id could not be found</response>
  [HttpDelete("{id:guid}")]
  public async Task<ActionResult> Delete([FromRoute] Guid id)
  {
    CondominiumsDeleteRequest request = new(id);

    await mediator.Send(request);

    return NoContent();
  }
}
