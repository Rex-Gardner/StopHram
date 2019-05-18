using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Models.Tags.Exceptions;
using Models.Tags.Repositories;
using Model = Models.Tags;
using Client = ClientModels.Tags;
using Converter = ModelConverters.Tags;

namespace API.Controllers
{
    [Route("api/v1/tags")]
    public class TagsController : ControllerBase
    {
        private readonly ITagRepository repository;

        public TagsController(ITagRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        
        /// <summary>
        /// Создаёт тег
        /// </summary>
        /// <param name="creationInfo">Информация о создаваемом теге</param>
        /// <param name="cancellationToken"></param>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateTagAsync([FromBody]Client.TagCreationInfo creationInfo,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (creationInfo == null)
            {
                var error = Responses.BodyIsMissing(nameof(creationInfo));
                return BadRequest(error);
            }

            var modelCreationInfo = Converter.TagCreationInfoConverter.Convert(creationInfo);
            Model.Tag modelTag;

            try
            {
                modelTag =
                    await repository.CreateAsync(modelCreationInfo, cancellationToken).ConfigureAwait(false);
            }
            catch (TagDuplicationException ex)
            {
                var error = Responses.DuplicationError(ex.Message, "Tag");
                return BadRequest(error);
            }

            var clientTag = Converter.TagConverter.Convert(modelTag);
            return CreatedAtRoute("GetTagRoute", new { id = clientTag.Id }, clientTag);
        }

        /// <summary>
        /// Возвращает список всех тегов
        /// </summary>
        /// <param name="cancellationToken"></param>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetTagsAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var modelTagsList = await repository.GetAllAsync(cancellationToken).ConfigureAwait(false);
            var clientTagsList = modelTagsList.Select(Converter.TagConverter.Convert).ToImmutableList();

            return Ok(clientTagsList);
        }

        /// <summary>
        /// Возвращает тег по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор тега</param>
        /// <param name="cancellationToken"></param>
        [HttpGet]
        [Route("{id}", Name = "GetTagRoute")]
        public async Task<IActionResult> GetTagAsync([FromRoute]string id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Model.Tag modelTag;

            try
            {
                modelTag = await repository.GetAsync(id, cancellationToken).ConfigureAwait(false);
            }
            catch (TagNotFoundException ex)
            {
                var error = Responses.NotFoundError(ex.Message, "Tag");
                return NotFound(error);
            }

            var clientTag = Converter.TagConverter.Convert(modelTag);
            return Ok(clientTag);
        }

        /// <summary>
        /// Изменяет тег
        /// </summary>
        /// <param name="id">Идентификатор тега</param>
        /// <param name="patchInfo">Новые значения параметров для тега</param>
        /// <param name="cancellationToken"></param>
        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> PatchTagAsync([FromRoute] string id,
            [FromBody] Client.TagPatchInfo patchInfo, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (patchInfo == null)
            {
                var error = Responses.BodyIsMissing(nameof(patchInfo));
                return BadRequest(error);
            }

            var modelPatchInfo = Converter.TagPatchInfoConveter.Convert(id, patchInfo);
            Model.Tag modelTag;

            try
            {
                modelTag = await repository.PatchAsync(modelPatchInfo, cancellationToken).ConfigureAwait(false);
            }
            catch (TagNotFoundException ex)
            {
                var error = Responses.NotFoundError(ex.Message, "Tag");
                return NotFound(error);
            }

            var clientTag = Converter.TagConverter.Convert(modelTag);
            return Ok(clientTag);
        }

        /// <summary>
        /// Удаляет тег
        /// </summary>
        /// <param name="id">Идентификатор тега</param>
        /// <param name="cancellationToken"></param>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteTagAsync([FromRoute]string id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                await repository.RemoveAsync(id, cancellationToken).ConfigureAwait(false);
            }
            catch (TagNotFoundException ex)
            {
                var error = Responses.NotFoundError(ex.Message, "Tag");
                return NotFound(error);
            }

            return NoContent();
        }
    }
}