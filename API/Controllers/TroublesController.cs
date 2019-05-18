using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Models.Troubles.Exceptions;
using Models.Troubles.Repositories;
using Model = Models.Troubles;
using Client = ClientModels.Troubles;
using Converter = ModelConverters.Troubles;

namespace API.Controllers
{
    [Route("api/v1/troubles")]
    public class TroublesController : ControllerBase
    {
        private readonly ITroubleRepository repository;

        public TroublesController(ITroubleRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        
        /// <summary>
        /// Создаёт проблему
        /// </summary>
        /// <param name="creationInfo">Информация о создаваемой проблеме</param>
        /// <param name="cancellationToken"></param>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateTroubleAsync([FromBody]Client.TroubleCreationInfo creationInfo,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (creationInfo == null)
            {
                var error = Responses.BodyIsMissing(nameof(creationInfo));
                return BadRequest(error);
            }

            var modelCreationInfo = Converter.TroubleCreationInfoConverter.Convert(creationInfo);
            Model.Trouble modelTrouble;

            try
            {
                modelTrouble =
                    await repository.CreateAsync(modelCreationInfo, cancellationToken).ConfigureAwait(false);
            }
            catch (TroubleDuplicationException ex)
            {
                var error = Responses.DuplicationError(ex.Message, "Trouble");
                return BadRequest(error);
            }

            var clientTrouble = Converter.TroubleConverter.Convert(modelTrouble);
            return CreatedAtRoute("GetTroubleRoute", new { id = clientTrouble.Id }, clientTrouble);
        }

        /// <summary>
        /// Выполняет выборку проблем
        /// </summary>
        /// <param name="searchInfo">Параметры запроса</param>
        /// <param name="cancellationToken"></param>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> SearchTroubleAsync([FromQuery] Client.TroubleSearchInfo searchInfo,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var modelSearchInfo =
                Converter.TroubleSearchInfoConverter.Convert(searchInfo ?? new Client.TroubleSearchInfo());
            var modelTroubleList =
                await repository.SearchAsync(modelSearchInfo, cancellationToken).ConfigureAwait(false);
            var clientTroubleList = modelTroubleList
                .Select(Converter.TroubleConverter.Convert)
                .ToImmutableList();

            return Ok(clientTroubleList);
        }

        /// <summary>
        /// Возвращает проблему по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор проблему</param>
        /// <param name="cancellationToken"></param>
        [HttpGet]
        [Route("{id}", Name = "GetTroubleRoute")]
        public async Task<IActionResult> GetTroubleAsync([FromRoute]string id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Guid guid;
            
            try
            {
                guid = Converter.TroubleConverterUtils.ConvertId(id);
            }
            catch (InvalidDataException ex)
            {
                var error = Responses.InvalidId(ex.Message, "Trouble");
                return BadRequest(error);
            }
            
            Model.Trouble modelTrouble;

            try
            {
                modelTrouble = await repository.GetAsync(guid, cancellationToken).ConfigureAwait(false);
            }
            catch (TroubleNotFoundException ex)
            {
                var error = Responses.NotFoundError(ex.Message, "Trouble");
                return BadRequest(error);
            }

            var clientTrouble = Converter.TroubleConverter.Convert(modelTrouble);
            return Ok(clientTrouble);
        }
        
        /// <summary>
        /// Изменяет информацию о проблеме
        /// </summary>
        /// <param name="id">Идентификатор проблемы</param>
        /// <param name="patchInfo">Новые значения параметров проблемы</param>
        /// <param name="cancellationToken"></param>
        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> PatchPlaceAsync([FromRoute] string id,
            [FromBody] Client.TroublePatchInfo patchInfo, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (patchInfo == null)
            {
                var error = Responses.BodyIsMissing(nameof(patchInfo));
                return BadRequest(error);
            }

            var modelPatchInfo = Converter.TroublePatchInfoConverter.Convert(id, patchInfo);
            Model.Trouble modelTrouble;

            try
            {
                modelTrouble = await repository.PatchAsync(modelPatchInfo, cancellationToken).ConfigureAwait(false);
            }
            catch (TroubleNotFoundException ex)
            {
                var error = Responses.NotFoundError(ex.Message, "Trouble");
                return BadRequest(error);
            }

            var clientTrouble = Converter.TroubleConverter.Convert(modelTrouble);
            return Ok(clientTrouble);
        }
        
        /// <summary>
        /// Удаляет проблему
        /// </summary>
        /// <param name="id">Идентификатор проблемы</param>
        /// <param name="cancellationToken"></param>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteTagAsync([FromRoute]string id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var guid = Converter.TroubleConverterUtils.ConvertId(id);
            
            try
            {
                await repository.RemoveAsync(guid, cancellationToken).ConfigureAwait(false);
            }
            catch (TroubleNotFoundException ex)
            {
                var error = Responses.NotFoundError(ex.Message, "Trouble");
                return BadRequest(error);
            }

            return NoContent();
        }
    }
}