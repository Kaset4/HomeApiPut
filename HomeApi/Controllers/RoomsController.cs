using System;
using System.Threading.Tasks;
using AutoMapper;
using HomeApi.Contracts.Models.Devices;
using HomeApi.Contracts.Models.Rooms;
using HomeApi.Data.Models;
using HomeApi.Data.Queries;
using HomeApi.Data.Repos;
using Microsoft.AspNetCore.Mvc;

namespace HomeApi.Controllers
{
    /// <summary>
    /// Контроллер комнат
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RoomsController : ControllerBase
    {
        private IRoomRepository _repository;
        private IRoomRepository _rooms;
        private IMapper _mapper;

        public RoomsController(IRoomRepository repository, IMapper mapper, IRoomRepository room)
        {
            _repository = repository;
            _mapper = mapper;
            _rooms = room;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetRooms()
        {
            var rooms = await _rooms.GetRooms();

            var resp = new GetRoomsResponse
            {
                RoomAmount = rooms.Length,
                Rooms = _mapper.Map<Room[], RoomView[]>(rooms)
            };

            return StatusCode(200, resp);
        }

        /// <summary>
        /// Добавление комнаты
        /// </summary>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Add([FromBody] AddRoomRequest request)
        {
            var existingRoom = await _repository.GetRoomByName(request.Name);
            if (existingRoom == null)
            {
                var newRoom = _mapper.Map<AddRoomRequest, Room>(request);
                await _repository.AddRoom(newRoom);
                return StatusCode(201, $"Комната {request.Name} добавлена!");
            }

            return StatusCode(409, $"Ошибка: Комната {request.Name} уже существует.");
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Edit(
            [FromRoute] Guid id,
            [FromBody] EditRoomRequest request)
        {
            var room = await _rooms.GetRoomById(id);
            if (room == null)
                return StatusCode(400, $"Ошибка: Комната с идентификатором {id} не существует.");            


            var withSameId = await _rooms.GetRoomByName(request.NewName);
            if (withSameId != null)
            {
                if (withSameId.Name != request.NewName)
                {
                    return StatusCode(400, $"Ошибка: Комната c именем {request.NewName} уже подключена. Выберите другое имя!");
                }
            }


            await _rooms.UpdateRoom(
                room,
                new UpdateRoomQuery(request.NewName, request.Area, request.GasConnected, request.Voltage)
            );

            return StatusCode(200, $"Комната обновлена! Имя - {room.Name}, Площадь - {room.Area},  Подключение газа - {room.GasConnected}, Напряжение комнаты - {room.Voltage}");
        }
    }
}