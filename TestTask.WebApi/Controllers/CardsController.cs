using System.Drawing.Imaging;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TestTask.Shared.Dto;
using TestTask.Shared.Entities;
using TestTask.Shared.Extensions;
using TestTask.WebApi.Extensions;
using TestTask.WebApi.Repositories;
using TestTask.WebApi.Requests;

namespace TestTask.WebApi.Controllers
{
    [Route("api/cards")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStaticFilesService _staticFilesService;
        private readonly ILogger<CardsController> _logger;

        public CardsController(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IStaticFilesService staticFilesService,
            ILogger<CardsController> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _staticFilesService = staticFilesService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                IEnumerable<Card> cards = _unitOfWork.Cards.GetAll();
                _logger.LogInformation("Returned all the cards from the database");

                var cardsResult = _mapper.Map<IEnumerable<CardDto>>(cards);
                cardsResult.ForEach(card => card.ImageUrl = BuildFileUrl(card.ImageUrl));

                return Ok(cardsResult);
            }
            catch (Exception exception)
            {
                _logger.LogError("Something went wrong inside GetAllAsync action: {Message}", exception.Message);

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id:int}", Name = "CardById")]
        public IActionResult GetById(int id)
        {
            try
            {
                Card? card = _unitOfWork.Cards.GetById(id);
                if (card is null)
                {
                    _logger.LogError("The card with id: {Id} hasn't been found in the database", id);
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation("Returned the card with id: {Id}", id);

                    var cardResult = _mapper.Map<CardDto>(card);
                    cardResult.ImageUrl = BuildFileUrl(cardResult.ImageUrl);

                    return Ok(cardResult);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError("Something went wrong inside GetCardById action: {Message}", exception.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] CreateCardRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("The request is invalid");
                    return BadRequest();
                }

                string fileExtension = Path.GetExtension(request.File.FileName)[1..];
                if (!_staticFilesService.SupportedImageExtensions.Contains(fileExtension))
                {
                    return BadRequest("You can only use .jpg images");
                }

                var cardEntity = new Card
                {
                    Label = request.Label,
                    ImageUrl = CreateImageFromBytes(await request.File.GetBytesAsync())
                };

                _unitOfWork.Cards.Create(cardEntity);
                _unitOfWork.Save();

                var createdCard = _mapper.Map<CardDto>(cardEntity);
                createdCard.ImageUrl = BuildFileUrl(cardEntity.ImageUrl);

                _logger.LogInformation("The card with id: {Id} was successfully created", createdCard.Id);

                return CreatedAtRoute("CardById", new {id = createdCard.Id}, createdCard);
            }
            catch (Exception exception)
            {
                _logger.LogError("Something went wrong inside CreateAsync action: {Message}", exception.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromForm] UpdateCardRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("The request is invalid");
                    return BadRequest();
                }

                if (request.File != null)
                {
                    string fileExtension = Path.GetExtension(request.File.FileName)[1..];
                    if (!_staticFilesService.SupportedImageExtensions.Contains(fileExtension))
                    {
                        return BadRequest("You can only use .jpg images");
                    }
                }

                Card? cardEntity = _unitOfWork.Cards.GetById(id);
                if (cardEntity is null)
                {
                    _logger.LogError("The card with id: {Id} hasn't been found in the database", id);
                    return NotFound();
                }

                cardEntity.Label = request.Label;

                if (request.File != null)
                {
                    cardEntity.ImageUrl = CreateImageFromBytes(await request.File.GetBytesAsync());
                }

                _unitOfWork.Cards.Update(cardEntity);
                _unitOfWork.Save();

                _logger.LogInformation("The card with id: {Id} was successfully updated", cardEntity.Id);

                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError("Something went wrong inside UpdateAsync action: {Message}", exception.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            try
            {
                Card? card = _unitOfWork.Cards.GetById(id);
                if (card == null)
                {
                    _logger.LogError("The card with id: {Id} hasn't been found in the database", id);
                    return NotFound();
                }

                _unitOfWork.Cards.Delete(card);
                _unitOfWork.Save();

                _logger.LogInformation("The card with id: {Id} was successfully deleted", card.Id);

                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError("Something went wrong inside Delete action: {Message}", exception.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        private string CreateImageFromBytes(byte[] bytes)
        {
            return _staticFilesService
                .CreateImageFromBytes(
                    bytes,
                    $"{Guid.NewGuid().ToString().Replace("-", "")}.jpg", ImageFormat.Jpeg)
                .RequestPath;
        }

        private string? BuildFileUrl(string? filename)
        {
            return string.IsNullOrWhiteSpace(filename) ? null : $"{GetHostAddress()}{filename}";
        }

        private string GetHostAddress()
        {
            return $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
        }
    }
}