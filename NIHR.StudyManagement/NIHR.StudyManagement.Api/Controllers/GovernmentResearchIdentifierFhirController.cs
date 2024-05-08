using Hl7.Fhir.Model;
using Hl7.Fhir.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using NIHR.StudyManagement.Api.Mappers;
using NIHR.StudyManagement.Api.Models.Dto;
using NIHR.StudyManagement.Domain.Abstractions;
using NIHR.StudyManagement.Domain.Exceptions;
using Swashbuckle.AspNetCore.Filters;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Emit;
using System.Runtime.Intrinsics.X86;
using System.Xml.Linq;
using System;
using NIHR.StudyManagement.Api.Documentation;

namespace NIHR.StudyManagement.Api.Controllers
{
    /// <summary>
    /// An API for operations related to creating (in FHIR format) a Government Research Identifier study registry
    /// and it's associated records e.g. IRAS or LPMS records.
    /// </summary>

    //[Authorize]
    [Route("api/identifier/fhir")]
    public class GovernmentResearchIdentifierFhirController : ApiControllerBase
    {
        private readonly IGovernmentResearchIdentifierService _governmentResearchIdentifierService;
        private readonly IGovernmentResearchIdentifierDtoMapper _dtoMapper;
        private readonly IFhirMapper _fhirMapper;

        public GovernmentResearchIdentifierFhirController(IGovernmentResearchIdentifierService governmentResearchIdentifierService,
            IGovernmentResearchIdentifierDtoMapper dtoMapper,
            IFhirMapper fhirMapper)
        {
            this._governmentResearchIdentifierService = governmentResearchIdentifierService;
            this._dtoMapper = dtoMapper;
            this._fhirMapper = fhirMapper;
        }

        /// <summary>
        /// This operation registers the given study (represented as a FHIR bundle) and generates a new, associated GRI identifier.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        [ProducesResponseType(typeof(GovernmentResearchIdentifierDto), StatusCodes.Status201Created)]
        [SwaggerRequestExample(typeof(Bundle), typeof(RegisterNewStudyBundleRequestExampleV1))]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(Bundle request, CancellationToken cancellationToken = default)
        {
            var createIdentifierRequest = _fhirMapper.MapCreateRequestBundle(request);

            var identifier = await _governmentResearchIdentifierService.RegisterStudyAsync(createIdentifierRequest, cancellationToken);

            var responseDto = _dtoMapper.Map(identifier);

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
