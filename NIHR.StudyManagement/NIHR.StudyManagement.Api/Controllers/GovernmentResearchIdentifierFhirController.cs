using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using NIHR.StudyManagement.Api.Documentation;
using NIHR.StudyManagement.Api.Mappers;
using NIHR.StudyManagement.Api.Models;
using NIHR.StudyManagement.Api.Models.Dto;
using NIHR.StudyManagement.Domain.Abstractions;
using NIHR.StudyManagement.Domain.Exceptions;
using NIHR.StudyManagement.Domain.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace NIHR.StudyManagement.Api.Controllers
{
    /// <summary>
    /// An API for operations related to creating (in FHIR format) a Government Research Identifier study registry
    /// and it's associated records e.g. IRAS or LPMS records.
    /// </summary>

    [Authorize]
    [Route("api/fhir/identifier")]
    public class GovernmentResearchIdentifierFhirController : ApiControllerBase
    {
        private readonly IGovernmentResearchIdentifierService _governmentResearchIdentifierService;
        private readonly IFhirMapper _fhirMapper;

        public GovernmentResearchIdentifierFhirController(IGovernmentResearchIdentifierService governmentResearchIdentifierService,
            IFhirMapper fhirMapper)
        {
            this._governmentResearchIdentifierService = governmentResearchIdentifierService;
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
            var createIdentifierRequest = _fhirMapper.MapCreateRequestBundle(request, ApiConsumerSystemName, "");

            var identifier = await _governmentResearchIdentifierService.RegisterStudyAsync(createIdentifierRequest, cancellationToken);

            var responseDto = _fhirMapper.MapToResearchStudyBundle(identifier,
                    new HttpRequestResponseFhirContext
                    {
                        Method = HttpMethod.Post,
                        Status = (int)HttpStatusCode.Created,
                        Url = HttpContext.Request.GetEncodedUrl()
                    });

            return CreatedAtAction(nameof(GetIdentifierAsync), new { identifier = identifier.GrisId}, responseDto);
        }

        /// <summary>
        /// This operation registers the given study (represented as a FHIR bundle) and generates a new, associated GRI identifier.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="identifier"></param>
        /// <param name="cancellationToken"></param>
        [ProducesResponseType(typeof(GovernmentResearchIdentifierDto), StatusCodes.Status201Created)]
        [SwaggerRequestExample(typeof(Bundle), typeof(RegisterNewStudyBundleRequestExampleV1))]
        [HttpPatch]
        [Route("{identifier}")]
        public async Task<IActionResult> CreateAsync(Bundle request, string identifier , CancellationToken cancellationToken = default)
        {
            var createIdentifierRequest = _fhirMapper.MapCreateRequestBundle(request, ApiConsumerSystemName, identifier);

            var researchStudy = await _governmentResearchIdentifierService.RegisterStudyAsync((RegisterStudyToExistingIdentifierRequest)createIdentifierRequest, cancellationToken);

            var responseDto = _fhirMapper.MapToResearchStudyBundle(researchStudy,
                    new HttpRequestResponseFhirContext
                    {
                        Method = HttpMethod.Patch,
                        Status = (int)HttpStatusCode.Created,
                        Url = HttpContext.Request.GetEncodedUrl()
                    });

            return CreatedAtAction(nameof(GetIdentifierAsync), new { identifier = identifier }, responseDto);
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

                var identifierResponse = _fhirMapper.MapToResearchStudyBundle(getIdentifierResponse,
                    new HttpRequestResponseFhirContext
                    {
                        Method = HttpMethod.Get,
                        Status = (int)HttpStatusCode.OK,
                        Url = HttpContext.Request.GetEncodedUrl()
                    });

                return Ok(identifierResponse);
            }
            catch (GriNotFoundException)
            {
                return NotFound($"The identifier '{identifier}' was not found or you do not have access to view the details.");
            }
        }
    }
}
