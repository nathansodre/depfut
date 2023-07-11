using DepFutAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DepFutAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayersController : ControllerBase
    {
        private static List<Player> players = new List<Player>();
        private static int nextId = 1;

        [HttpGet]
        public ActionResult<IEnumerable<Player>> GetPlayers()
        {
            return players;
        }

        [HttpPost]
        public ActionResult<Player> CreatePlayer(Player player)
        {
            player.Id = nextId++;
            players.Add(player);
            return player;
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePlayer(int id, Player updatedPlayer)
        {
            var player = players.Find(p => p.Id == id);
            if (player == null)
            {
                return NotFound();
            }

            if (updatedPlayer.Nome != null)
            {
                player.Nome = updatedPlayer.Nome;
            }

            if (updatedPlayer.Idade > 0)
            {
                player.Idade = updatedPlayer.Idade;
            }

            if (updatedPlayer.Posicao != null)
            {
                player.Posicao = updatedPlayer.Posicao;
            }

            if (updatedPlayer.Nacionalidade != null)
            {
                player.Nacionalidade = updatedPlayer.Nacionalidade;
            }

            if (updatedPlayer.Gols > 0)
            {
                player.Gols = updatedPlayer.Gols;
            }

            if (updatedPlayer.Assistencias > 0)
            {
                player.Assistencias = updatedPlayer.Assistencias;
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
        public IActionResult DeletePlayer(int id)
        {
            var player = players.Find(p => p.Id == id);
            if (player == null)
            {
                return NotFound();
            }

            players.Remove(player);
            return NoContent();
        }
    }
}

