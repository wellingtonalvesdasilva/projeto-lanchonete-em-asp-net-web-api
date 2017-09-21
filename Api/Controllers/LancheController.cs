using Model;
using System;
using System.Web.Http;

namespace Api.Controllers
{
    public class LancheController : ApiBaseController<LanchoneteEntities, Lanche>
    {
        /// <summary>
        /// Criar um lanche
        /// </summary>
        /// <param name="lanche"></param>
        /// <returns></returns>
        public Lanche Post([FromBody] Lanche lanche)
        {
            if (lanche == null)
                throw new ArgumentException("dados inválido");

            BusinessFacade.Lanche.Incluir(lanche);

            return lanche;
        }
    }
}
