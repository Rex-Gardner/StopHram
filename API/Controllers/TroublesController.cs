using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Tags.Repositories;
using Models.Troubles.Exceptions;
using Models.Troubles.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Model = Models.Troubles;
using Client = ClientModels.Troubles;
using Converter = ModelConverters.Troubles;

namespace API.Controllers
{
    [Route("api/v1/troubles")]
    public class TroublesController : ControllerBase
    {
        private readonly ITroubleRepository troubleRepository;
        private readonly ITagRepository tagRepository;
        private const string Target = "Trouble";
        private const int RuquiredLikesCount = 100;

        public TroublesController(ITroubleRepository troubleRepository, ITagRepository tagRepository)
        {
            this.troubleRepository = troubleRepository ?? throw new ArgumentNullException(nameof(troubleRepository));
            this.tagRepository = tagRepository ?? throw new ArgumentNullException(nameof(tagRepository));
        }
        
        /// <summary>
        /// Создаёт проблему
        /// </summary>
        /// <param name="creationInfo">Информация о создаваемой проблеме</param>
        /// <param name="cancellationToken"></param>
        [HttpPost]
        [Route("")]
        [Authorize]
        public async Task<IActionResult> CreateTroubleAsync([FromBody]Client.TroubleCreationInfo creationInfo,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (creationInfo == null)
            {
                var error = Responses.BodyIsMissing(nameof(creationInfo));
                return BadRequest(error);
            }

            var author = HttpContext.User.Identity.Name;

            if (author == null)
            {
                var error = Responses.Unauthorized(nameof(author));
                return Unauthorized(error);
            }

            Model.Trouble modelTrouble;

            try
            {
                var modelTags = await tagRepository.GetAllAsync(cancellationToken).ConfigureAwait(false);
                var modelCreationInfo = Converter.TroubleCreationInfoConverter.Convert(creationInfo, modelTags);
                modelTrouble =
                    await troubleRepository.CreateAsync(modelCreationInfo, author, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (ex is TroubleDuplicationException)
                {
                    var error = Responses.DuplicationError(ex.Message, Target);
                    return BadRequest(error);                    
                }
                else //if (ex is InvalidDataException)
                {
                    var error = Responses.InvalidData(ex.Message, Target);
                    return BadRequest(error);
                }
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
        [AllowAnonymous]
        public async Task<IActionResult> SearchTroubleAsync([FromQuery] Client.TroubleSearchInfo searchInfo,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var modelSearchInfo =
                Converter.TroubleSearchInfoConverter.Convert(searchInfo ?? new Client.TroubleSearchInfo());
            var modelTroubleList =
                await troubleRepository.SearchAsync(modelSearchInfo, cancellationToken).ConfigureAwait(false);
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
        [AllowAnonymous]
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
                var error = Responses.InvalidId(ex.Message, Target);
                return BadRequest(error);
            }
            
            Model.Trouble modelTrouble;

            try
            {
                modelTrouble = await troubleRepository.GetAsync(guid, cancellationToken).ConfigureAwait(false);
            }
            catch (TroubleNotFoundException ex)
            {
                var error = Responses.NotFoundError(ex.Message, Target);
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
        [Authorize]
        public async Task<IActionResult> PatchTroubleAsync([FromRoute] string id,
            [FromBody] Client.TroublePatchInfo patchInfo, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (patchInfo == null)
            {
                var error = Responses.BodyIsMissing(nameof(patchInfo));
                return BadRequest(error);
            }

            var guid = Converter.TroubleConverterUtils.ConvertId(id);

            Model.Trouble trouble;
            try
            {
                trouble = await troubleRepository.GetAsync(guid, cancellationToken).ConfigureAwait(false);
            }
            catch (TroubleNotFoundException ex)
            {
                var error = Responses.NotFoundError(ex.Message, Target);
                return BadRequest(error);
            }

            var isAuthorized = trouble.Author == HttpContext.User.Identity.Name || HttpContext.User.IsInRole("admin");
            if (!isAuthorized)
            {
                return Forbid();
            }

            if (!HttpContext.User.IsInRole("admin"))
            {
                patchInfo.Status = null;
            }


            Model.Trouble modelTrouble;

            try
            {
                var modelTags = await tagRepository.GetAllAsync(cancellationToken).ConfigureAwait(false);
                var modelPatchInfo = Converter.TroublePatchInfoConverter.Convert(id, patchInfo, modelTags);
                modelTrouble = await troubleRepository.PatchAsync(modelPatchInfo, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (ex is TroubleNotFoundException)
                {
                    var error = Responses.NotFoundError(ex.Message, Target);
                    return BadRequest(error);                    
                }
                else
                {
                    var error = Responses.InvalidData(ex.Message, Target);
                    return BadRequest(error);
                }
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
        [Authorize]
        public async Task<IActionResult> DeleteTagAsync([FromRoute]string id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var guid = Converter.TroubleConverterUtils.ConvertId(id);

            Model.Trouble trouble;
            try
            {
                trouble = await troubleRepository.GetAsync(guid, cancellationToken).ConfigureAwait(false);
            }
            catch (TroubleNotFoundException ex)
            {
                var error = Responses.NotFoundError(ex.Message, Target);
                return BadRequest(error);
            }

            var isAuthorized = trouble.Author == HttpContext.User.Identity.Name || HttpContext.User.IsInRole("admin");
            if (!isAuthorized)
            {
                return Forbid();
            }
            
            await troubleRepository.RemoveAsync(guid, cancellationToken).ConfigureAwait(false);

            return NoContent();
        }
        
        /// <summary>
        /// Лайкает и убирает лайк с проблемы
        /// </summary>
        /// <param name="id">Идентификатор проблемы</param>
        /// <param name="cancellationToken"></param>
        [HttpPost]
        [Route("toggle-like/{id}")]
        [Authorize]
        public async Task<IActionResult> ToggleTroubleLikeAsync([FromRoute] string id, 
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var guid = Converter.TroubleConverterUtils.ConvertId(id);
            var userName = HttpContext.User.Identity.Name;

            if (userName == null)
            {
                var error = Responses.Unauthorized(nameof(userName));
                return Unauthorized(error);
            }
            
            Model.Trouble modelTrouble;

            try
            {
                modelTrouble = await troubleRepository.ToggleLikeAsync(guid, userName, cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (TroubleNotFoundException ex)
            {
                var error = Responses.NotFoundError(ex.Message, Target);
                return BadRequest(error);
            }

            if (modelTrouble.Status == Model.TroubleStatus.Created && modelTrouble.LikedUsers.Count >= RuquiredLikesCount)
            {
                var httpClient = new HttpClient();
                //todo uri для разных гор советов
                //todo разные представители округов по координатам проблемы

                var request = new ClientModels.GorSovetRequests.GorSovetRequest(userName, modelTrouble.Description);
                var jsonString = JsonConvert.SerializeObject(request);
                
                var response = await httpClient.PostAsync("http://gorsovetnsk.ru/feedback/new/", 
                    new StringContent(jsonString, Encoding.UTF8,"application/json"), cancellationToken);
                
                response.EnsureSuccessStatusCode();
                //todo отображать информацию об отправленном заявлении

                modelTrouble = await troubleRepository.PatchAsync(new Model.TroublePatchInfo(guid, null, null, null,
                    null, null, null, null, Model.TroubleStatus.Proved), cancellationToken);
            }

            var clientTrouble = Converter.TroubleConverter.Convert(modelTrouble);
            return Ok(clientTrouble);
        }
    }
}