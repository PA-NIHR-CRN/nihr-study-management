using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NIHR.StudyManagement.Api.Configuration;
using NIHR.StudyManagement.Api.Mappers;
using NIHR.StudyManagement.Api.Models.Dto;
using NIHR.StudyManagement.Domain.Abstractions;
using NIHR.StudyManagement.Domain.Exceptions;

namespace NIHR.StudyManagement.Api.Controllers
{
    /// <summary>
    /// An API for operations related to creating/retrieving a Government Research Identifier study registry
    /// and it's associated records e.g. IRAS or LPMS records.
    /// </summary>
    [Authorize]
    [Route("api/identifier")]
    public class GovernmentResearchIdentifierController : ApiControllerBase
    {
        private readonly IGovernmentResearchIdentifierService _governmentResearchIdentifierService;
        private readonly IGovernmentResearchIdentifierDtoMapper _dtoMapper;

        public GovernmentResearchIdentifierController(IGovernmentResearchIdentifierService governmentResearchIdentifierService,
            IGovernmentResearchIdentifierDtoMapper dtoMapper)
        {
            this._governmentResearchIdentifierService = governmentResearchIdentifierService;
            this._dtoMapper = dtoMapper;
        }

        /// <summary>
        /// This operation registers the given study and generates an associated GRI.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        [ProducesResponseType(typeof(GovernmentResearchIdentifierDto), StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(RegisterStudyRequestDto request, CancellationToken cancellationToken = default)
        {
            var createIdentifierRequest = _dtoMapper.Map(request);

            var identifier = await _governmentResearchIdentifierService.RegisterStudyAsync(createIdentifierRequest, cancellationToken);

            var responseDto = _dtoMapper.Map(identifier);

            return CreatedAtAction(nameof(GetIdentifierAsync), new { identifier = responseDto.Gri }, responseDto);
        }

        /// <summary>
        /// This operation registers the given study with an existing GRI identifier.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="identifier"></param>
        /// <param name="cancellationToken"></param>
        [ProducesResponseType(typeof(GovernmentResearchIdentifierDto), StatusCodes.Status201Created)]
        [HttpPatch]
        [Route("{identifier}")]
        public async Task<IActionResult> RegisterStudyToExistingIdentifierAsync(RegisterStudyRequestDto request, string identifier,
            CancellationToken cancellationToken = default)
        {
            var createIdentifierRequest = _dtoMapper.Map(request, identifier);

            var governmentResearchIdentifier = await _governmentResearchIdentifierService.RegisterStudyAsync(createIdentifierRequest, cancellationToken);

            var responseDto = _dtoMapper.Map((Domain.Models.GovernmentResearchIdentifier)governmentResearchIdentifier);

            return CreatedAtAction(nameof(GetIdentifierAsync), new { identifier = responseDto.Gri }, responseDto);
        }

        /// <summary>
        /// An operation to retrieve the details of the specified identifier.
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(GovernmentResearchIdentifierDto), StatusCodes.Status200OK)]
        [Route("{identifier}")]
        [ActionName(nameof(GetIdentifierAsync))]
        public async Task<IActionResult> GetIdentifierAsync(string identifier, CancellationToken cancellationToken = default)
        {
            try
            {
                var getIdentifierResponse = await _governmentResearchIdentifierService.GetAsync(identifier);

                var identifierResponse = _dtoMapper.Map(getIdentifierResponse);

                return Ok(identifierResponse);
            }
            catch (GriNotFoundException)
            {
                return NotFound($"The identifier '{identifier}' was not found or you do not have access to view the details.");
            }
        }
    }
}
