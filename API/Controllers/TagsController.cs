using System;
using Microsoft.AspNetCore.Mvc;
using Models.Tags.Repositories;

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
    }
}